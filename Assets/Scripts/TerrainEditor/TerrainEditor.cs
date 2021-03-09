using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using System.Linq;
using System.IO;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainEditor : MonoBehaviour
{
    public List<Obstacle> obstacles = new List<Obstacle>();
    public Transform obstaclesTransform;

    public TerrainSaveData saveData;

    public string editMode = "Move";

    public GameObject rockPrefab;
    public GameObject spikePrefab;

    public Texture2D movePointer;

    GameObject[] obstaclePrefabs;

    public GameObject vertPrefab;

    int groundSize = 10000;
    int groundRes = 2;

    Mesh mesh;

    Vector3[] verticies;
    int[] triangles;

    EdgeCollider2D col;

    int saveSlot = 1;
    bool newSim = false;

    void Start()
    {
        saveSlot = PlayerPrefs.GetInt("SaveSlot");
        newSim = PlayerPrefs.GetInt("newSim") == 1;

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        saveData = new TerrainSaveData();

        col = GetComponent<EdgeCollider2D>();

        obstaclePrefabs = new GameObject[2]
        {
            rockPrefab,
            spikePrefab
        };

        if (newSim)
        {
            SaveDefaultMesh();
        }

        LoadTerrain();
    }

    // Updates the edge collider so it is on the top of the ground
    public void UpdateEdgeCollider()
    {
        // As EdgeCollider2D requires Vector2s, the verticies list must be iterated
        // through and every second vertex is added to topEdgeVerts, which stores the 
        // verticies on the top

        var topEdgeVerts = new List<Vector2>();

        for (int i = 0; i < verticies.Length; i += 2)
        {
            topEdgeVerts.Add(new Vector2(verticies[i].x, verticies[i].y));
        }

        col.points = topEdgeVerts.ToArray();
    }

    // Updates the mesh with the new verticies and triangles
    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
    }

    void AddTriangles()
    {
        List<int> tempTriangles = new List<int>();
        for (int i = 0; i < verticies.Length - 2; i += 2)
        {
            tempTriangles.Add(i + 0);
            tempTriangles.Add(i + 2);
            tempTriangles.Add(i + 1);
            tempTriangles.Add(i + 1);
            tempTriangles.Add(i + 2);
            tempTriangles.Add(i + 3);
        }

        triangles = tempTriangles.ToArray();
    }

    public void Save()
    {
        saveData.points = VertsToHeights();
        UpdateObstacles();

        // Saves the data
        SerializationManager.Save(saveSlot, saveData);
    }

    public float GetHeightAtPoint(float x)
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(x, 100f), Vector2.down, 150, 9);
        return 100 - hit.distance;
    }

    public float GetAngleAtPoint(float x)
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(x, 100f), Vector2.down, 150, 9);
        float angle = Vector2.Angle(hit.normal, new Vector2(1, 0));
        return angle;
    }

    // Creates the default mesh for the ground
    void SaveDefaultMesh()
    {
        // The temporary lists are converted back into arrays
        verticies = HeightsToVerts(Enumerable.Repeat(1.0f, groundSize / groundRes + 1).ToArray());
        Save();
    }

    Vector3[] HeightsToVerts(float[] heights)
    {
        List<Vector3> tempVerticies = new List<Vector3>();

        GameObject newVert;

        // Deletes all Vert gameobjects
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        int j = 0;
        for (int i = -groundSize / 2; i <= groundSize / 2; i += groundRes)
        {
            try
            {
                tempVerticies.Add(new Vector3(i, heights[j]));
            }
            catch
            {
                Debug.Log(heights.Length);
                Debug.Log(j);
            }

            // Creates the vertex 
            newVert = Instantiate(vertPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
            newVert.transform.position = new Vector3(i, heights[j] + transform.position.y, 0);
            Vert newVertScript = newVert.GetComponent<Vert>();
            newVertScript.pointLockIndex = tempVerticies.Count - 1;
            newVertScript.terrainManager = this;

            tempVerticies.Add(new Vector3(i, -1));

            j++;
        }

        return tempVerticies.ToArray();
    }

    float[] VertsToHeights()
    {
        var heights = new List<float>();

        foreach (Vector3 vert in verticies)
        {
            if (vert.y == -1)
            {
                continue;
            }

            heights.Add(vert.y);
        }

        return heights.ToArray();
    }

    public void LoadTerrain()
    {
        TerrainSaveData data = (TerrainSaveData)SerializationManager.Load(saveSlot);

        if (data == null)
        {
            SaveDefaultMesh();
            data = (TerrainSaveData)SerializationManager.Load(saveSlot);
        }

        verticies = HeightsToVerts(data.points);

        float[] loadObstacles = data.obstacles;
        for (int i = 0; i < loadObstacles.Length; i += 2)
        {
            int typeCode = (int)loadObstacles[i];
            float x = loadObstacles[i + 1];
            GameObject newObstacle = Instantiate(obstaclePrefabs[typeCode], new Vector3(x, 0, 5), Quaternion.identity, obstaclesTransform);
            Obstacle script = newObstacle.AddComponent<Obstacle>();
            script.typeCode = typeCode;
            obstacles.Add(script);
        }

        AddTriangles();
        UpdateMesh();
        UpdateEdgeCollider();

        foreach (Obstacle obstacle in obstacles)
        {
            obstacle.UpdatePosAndAngle(obstacle.gameObject.transform.position.x);
        }
    }

    public void DeleteObstacle(Obstacle obstacle)
    {
        obstacles.Remove(obstacle);
        Destroy(obstacle.gameObject);
        Save();
    }

    public void UpdateObstacles()
    {
        var obstaclesList = new List<float>();

        foreach (Obstacle obstacle in obstacles)
        {
            obstacle.UpdatePosAndAngle(obstacle.gameObject.transform.position.x);
            obstaclesList.Add(obstacle.typeCode);
            obstaclesList.Add(obstacle.transform.position.x);
        }

        saveData.obstacles = obstaclesList.ToArray();
    }

    public void UpdatePoint(int i, Vector3 newPoint)
    {
        // Convert local coords to global coords
        newPoint = transform.InverseTransformPoint(newPoint);

        // Changes the point
        verticies[i] = newPoint;

        UpdateMesh();

        UpdateEdgeCollider();
        UpdateObstacles();
    }
}
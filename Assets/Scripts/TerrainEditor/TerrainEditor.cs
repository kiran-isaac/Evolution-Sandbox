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
    Terrain terrain = new Terrain();

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

    bool newSim = false;

    void Start()
    {
        newSim = PlayerPrefs.GetInt("newSim") == 1;

        GetComponent<MeshFilter>().mesh = terrain.mesh;

        terrain.col = GetComponent<EdgeCollider2D>();

        terrain.obstaclePrefabs = new GameObject[2]
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

    void SaveDefaultMesh()
    {
        terrain.verticies = HeightsToVerts(Enumerable.Repeat(1f, groundSize / groundRes).ToArray());
        terrain.Save();
    }

    // Converts float[] heights to Vector3[] of verticies
    public Vector3[] HeightsToVerts(float[] heights)
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
            newVertScript.terrain = terrain;

            tempVerticies.Add(new Vector3(i, -1));

            j++;
        }

        return tempVerticies.ToArray();
    }

    public void UpdatePoint(int i, Vector3 newPoint)
    {
        // Convert local coords to global coords
        newPoint = transform.InverseTransformPoint(newPoint);

        // Changes the point
        terrain.verticies[i] = newPoint;

        terrain.UpdateMesh();

        terrain.UpdateEdgeCollider();
        terrain.UpdateObstacles();
    }

    public void LoadTerrain()
    {
        TerrainSaveData data = (TerrainSaveData)SerializationManager.Load(terrain.saveSlot);

        terrain.verticies = HeightsToVerts(data.points);

        float[] loadObstacles = data.obstacles;
        for (int i = 0; i < loadObstacles.Length; i += 2)
        {
            int typeCode = (int)loadObstacles[i];
            float x = loadObstacles[i + 1];
            GameObject newObstacle = Instantiate(obstaclePrefabs[typeCode], new Vector3(x, 0, 5),
                Quaternion.identity, obstaclesTransform);
            Obstacle script = newObstacle.AddComponent<Obstacle>();
            script.typeCode = typeCode;
            obstacles.Add(script);
        }

        terrain.AddTriangles();
        terrain.UpdateMesh();
        terrain.UpdateEdgeCollider();

        foreach (Obstacle obstacle in obstacles)
        {
            obstacle.UpdatePosAndAngle(obstacle.gameObject.transform.position.x);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SimTerrain : MonoBehaviour
{
    public SerializationManager terrainSerializer;

    public GameObject rockPrefab;
    public GameObject spikePrefab;
    public Transform obstaclesTransform;
    public GameObject flagPrefab;

    GameObject[] obstaclePrefabs;
    List<SimObstacle> obstacles = new List<SimObstacle>();

    int groundSize = 10000;
    int groundRes = 2;

    Mesh mesh;

    Vector3[] verticies;
    int[] triangles;
    EdgeCollider2D col;

    int saveSlot = 1;

    private void Start()
    {
        saveSlot = PlayerPrefs.GetInt("SaveSlot");

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        obstaclePrefabs = new GameObject[2]
        {
            rockPrefab,
            spikePrefab
        };

        col = GetComponent<EdgeCollider2D>();

        LoadTerrain();
    }

    public void GenerateFlags()
    {
        while (transform.childCount != 0)
        {
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }

        for (int x = -groundSize / 2; x < groundSize / 2; x += 5)
        {
            GameObject flag = Instantiate(flagPrefab, new Vector3(x, GetHeightAtPoint(x), 5), Quaternion.identity, transform);
            GameObject dist = flag.transform.GetChild(2).gameObject;
            TextMeshPro text = dist.GetComponent<TextMeshPro>();
            text.SetText(x.ToString() + "m");
        }
    }
    
    // Loads the data from the appropriate save slot file
    public void LoadTerrain()
    {
        TerrainSaveData data = (TerrainSaveData)SerializationManager.Load(saveSlot);

        verticies = HeightsToVerts(data.points);

        GenerateTriangles();
        UpdateMesh();
        UpdateEdgeCollider();
        GenerateFlags();

        float[] loadObstacles = data.obstacles;
        for (int i = 0; i < loadObstacles.Length; i += 2)
        {
            int typeCode = (int)loadObstacles[i];
            float x = loadObstacles[i + 1];
            GameObject newObstacle = Instantiate(obstaclePrefabs[typeCode], new Vector3(x, 0, 0), Quaternion.identity, obstaclesTransform);
            SimObstacle script = newObstacle.AddComponent<SimObstacle>();
            obstacles.Add(script);
        }

        foreach (SimObstacle obstacle in obstacles)
        {
            obstacle.UpdatePosAndAngle(obstacle.gameObject.transform.position.x);
        }
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

    // Updates the triangles array
    void GenerateTriangles()
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

    // Returns the height of the ground at x in world coords
    public float GetHeightAtPoint(float x)
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(x, 101f), Vector2.down, 150, 11);

        return 101 - hit.distance;
    }

    // Gets the angle between the normal and the positive x of the ground at x
    public float GetAngleAtPoint(float x)
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(x, 100f), Vector2.down, 150, 9);
        float angle = Vector2.Angle(hit.normal, new Vector2(1, 0));
        return angle;
    }

    // Converts a list of heights to a list of verticies
    Vector3[] HeightsToVerts(float[] heights)
    {
        List<Vector3> tempVerticies = new List<Vector3>();

        int j = 0;
        for (int i = -groundSize / 2; i <= groundSize / 2; i += groundRes)
        {
            tempVerticies.Add(new Vector3(i, heights[j]));

            tempVerticies.Add(new Vector3(i, -1));

            j++;
        }

        return tempVerticies.ToArray();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain
{
    public GameObject parent;

    public List<Obstacle> obstacles = new List<Obstacle>();
    public Transform obstaclesTransform;

    int groundSize = 10000;
    int groundRes = 2;

    public Mesh mesh;

    public TerrainSaveData saveData;

    public GameObject[] obstaclePrefabs;

    public GameObject vertPrefab;

    public Vector3[] verticies;
    int[] triangles;

    public EdgeCollider2D col;

    public int saveSlot = 1;

    public Terrain()
    {
        mesh = new Mesh();

        saveSlot = PlayerPrefs.GetInt("SaveSlot");

        saveData = new TerrainSaveData();
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

    // Updates the mesh with the new verticies and triangles
    public void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
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

    public void Save()
    {
        saveData.points = VertsToHeights();
        UpdateObstacles();

        // Saves the data
        SerializationManager.Save(saveSlot, saveData);
    }

    public void AddTriangles()
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
}
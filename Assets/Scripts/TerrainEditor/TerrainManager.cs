using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Linq;
using System.IO;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainManager : MonoBehaviour
{
    public SerializationManager groundSave;

    public TerrainSaveData saveData;

    public GameObject vertPrefab;

    int groundSize = 10000;
    int groundRes = 2;

    Mesh mesh;

    Vector3[] verticies;
    int[] triangles;

    EdgeCollider2D col;

    int saveSlot = 1;
    bool newSim = false;

    private void Awake()
    {
        GameObject menuData = GameObject.Find("MainMenuData");

        try
        {
            MainMenuManager menuManager = menuData.GetComponent<MainMenuManager>();

            saveSlot = menuManager.saveSlot;
            newSim = menuManager.newSim;

            Destroy(menuData);
        }
        catch
        {
            saveSlot = 1;
            newSim = false;
        }
    }

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        groundSave = new SerializationManager();
        saveData = new TerrainSaveData();

        col = GetComponent<EdgeCollider2D>();

        if (newSim)
        {
            SaveDefaultMesh();
        }

        LoadTerrain();

        UpdateEdgeCollider();
        UpdateMesh();
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

        // Saves the data
        SerializationManager.Save(saveSlot, saveData);
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
            newVertScript.groundManager = this;

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

    private void OnValidate()
    {
        
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
        AddTriangles();
        UpdateMesh();
    }

    public void UpdatePoint(int i, Vector3 newPoint)
    {
        // Convert local coords to global coords
        newPoint = transform.InverseTransformPoint(newPoint);

        // Changes the point
        verticies[i] = newPoint;

        UpdateMesh();
    }
}
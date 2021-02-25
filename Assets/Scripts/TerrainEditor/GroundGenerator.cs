using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Mono.Data.Sqlite;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GroundGenerator : MonoBehaviour
{
    public GameObject vertPrefab;

    public int groundSize = 10;
    public int groundRes = 2;

    IDbConnection dbconn;

    Mesh mesh;

    Vector3[] verticies;
    int[] triangles;

    EdgeCollider2D col;

    int saveSlot = 1;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        col = GetComponent<EdgeCollider2D>();

        ConnectToDB();

        CreateDefaultMesh();
        UpdateEdgeCollider();
        UpdateMesh();
    }

    private void ConnectToDB()
    {
        string conn = "URI=file:" + Application.dataPath + "/data.db"; //Path to database.
        dbconn = new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
    }

    // Updates the edge collider so it is on the top of the ground
    void UpdateEdgeCollider()
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

    // Creates the default mesh for the ground
    void CreateDefaultMesh()
    {
        // As the lengths of the arrays are unknown for now, lists are used temporarily
        List<Vector3> tempVerticies = new List<Vector3>();
        List<int> tempTriangles = new List<int>();

        GameObject newVert;
        for (int i = -groundSize/2; i <= groundSize/2; i += groundRes)
        {
            tempVerticies.Add(new Vector3(i, 1));

            // Creates the vertex 
            newVert = Instantiate(vertPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
            newVert.transform.position = new Vector3(i, 1 + transform.position.y, 0);
            Vert newVertScript = newVert.GetComponent<Vert>();
            newVertScript.pointLockIndex = tempVerticies.Count - 1;
            newVertScript.groundGenerator = this;

            tempVerticies.Add(new Vector3(i, -1));
        }

        for (int i = 0; i < tempVerticies.Count - 2; i += 2)
        {
            // The triangle mesh pattern for a square
            tempTriangles.Add(i + 0);
            tempTriangles.Add(i + 2);
            tempTriangles.Add(i + 1);
            tempTriangles.Add(i + 1);
            tempTriangles.Add(i + 2);
            tempTriangles.Add(i + 3);
        }

        // The temporary lists are converted back into arrays
        verticies = tempVerticies.ToArray();
        triangles = tempTriangles.ToArray();

        UpdateDB();
    }

    public void UpdateDB()
    {
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "DELETE FROM t" + saveSlot.ToString();
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteReader();

        dbcmd = dbconn.CreateCommand();
        sqlQuery = "INSERT INTO t" + saveSlot.ToString() + "(X, Y) " +
            "VALUES";

        int vertIndex = 0;
        for (int i = -groundSize / 2; i <= groundSize / 2; i += groundRes)
        {
            sqlQuery += "(" + i.ToString() + ", " + (verticies[vertIndex].y + transform.position.y).ToString() + "),";
            vertIndex += groundRes;
        }

        dbcmd.CommandText = sqlQuery.Remove(sqlQuery.Length - 1);
        dbcmd.ExecuteReader();
    }

    public void UpdatePoint(int i, Vector3 newPoint)
    {
        // Convert local coords to global coords
        newPoint = transform.InverseTransformPoint(newPoint);

        // Changes the point
        verticies[i] = newPoint;

        UpdateMesh();
        UpdateEdgeCollider();
    }
}

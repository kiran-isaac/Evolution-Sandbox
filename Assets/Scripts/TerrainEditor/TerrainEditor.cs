using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainEditor : MonoBehaviour
{
    
    public Transform obstaclesTransform;

    public TerrainSaveData saveData;

    public string editMode = "Move";

    public GameObject rockPrefab;
    public GameObject spikePrefab;

    public Texture2D movePointer;

    GameObject[] _obstaclePrefabs;

    public GameObject vertPrefab;

    int groundSize = 10000;
    int groundRes = 2;

    Mesh _mesh;

    Vector3[] _vertices;
    int[] _triangles;

    EdgeCollider2D _col;

    int _saveSlot = 1;
    bool _newSim = false;

    private void Start()
    {
        _saveSlot = PlayerPrefs.GetInt("SaveSlot");
        _newSim = PlayerPrefs.GetInt("newSim") == 1;

        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;

        saveData = new TerrainSaveData();

        _col = GetComponent<EdgeCollider2D>();

        _obstaclePrefabs = new GameObject[2]
        {
            rockPrefab,
            spikePrefab
        };

        if (_newSim)
        {
            SaveDefaultMesh();
        }

        LoadTerrain();
    }

    // Updates the edge collider so it is on the top of the ground
    public void UpdateEdgeCollider()
    {
        // As EdgeCollider2D requires Vector2s, the vertices list must be iterated
        // through and every second vertex is added to topEdgeVerts, which stores the 
        // vertices on the top

        var topEdgeVerts = new List<Vector2>();

        for (int i = 0; i < _vertices.Length; i += 2)
        {
            topEdgeVerts.Add(new Vector2(_vertices[i].x, _vertices[i].y));
        }

        _col.points = topEdgeVerts.ToArray();
    }

    // Updates the mesh with the new vertices and triangles
    private void UpdateMesh()
    {
        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
    }

    private void AddTriangles()
    {
        List<int> tempTriangles = new List<int>();
        for (int i = 0; i < _vertices.Length - 2; i += 2)
        {
            tempTriangles.Add(i + 0);
            tempTriangles.Add(i + 2);
            tempTriangles.Add(i + 1);
            tempTriangles.Add(i + 1);
            tempTriangles.Add(i + 2);
            tempTriangles.Add(i + 3);
        }

        _triangles = tempTriangles.ToArray();
    }

    public void Save()
    {
        saveData.points = VertsToHeights();
        UpdateObstacles();

        // Saves the data
        SerializationManager.Save("/Terrain Saves", "saveslot" + _saveSlot.ToString() + ".trn", saveData);
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
        _vertices = HeightsToVerts(Enumerable.Repeat(1.0f, groundSize / groundRes + 1).ToArray());
        Save();
    }

    Vector3[] HeightsToVerts(float[] heights)
    {
        List<Vector3> tempVertices = new List<Vector3>();

        // Deletes all Vert gameObjects
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        int j = 0;
        for (int i = -groundSize / 2; i <= groundSize / 2; i += groundRes)
        {
            try
            {
                tempVertices.Add(new Vector3(i, heights[j]));
            }
            catch
            {
                Debug.Log(heights.Length);
                Debug.Log(j);
            }

            // Creates the vertex 
            var newVert = Instantiate(vertPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
            newVert.transform.position = new Vector3(i, heights[j] + transform.position.y, 0);
            var newVertScript = newVert.GetComponent<Vert>();
            newVertScript.pointLockIndex = tempVertices.Count - 1;
            newVertScript.terrainManager = this;

            tempVertices.Add(new Vector3(i, -1));

            j++;
        }

        return tempVertices.ToArray();
    }

    float[] VertsToHeights()
    {
        var heights = new List<float>();

        foreach (Vector3 vert in _vertices)
        {
            if (vert.y == -1)
            {
                continue;
            }

            heights.Add(vert.y);
        }

        return heights.ToArray();
    }

    private void LoadTerrain()
    {
        TerrainSaveData data = (TerrainSaveData)SerializationManager.Load("/Terrain Saves/saveslot" + _saveSlot.ToString() + ".trn");

        if (data == null)
        {
            SaveDefaultMesh();
            data = (TerrainSaveData)SerializationManager.Load("/Terrain Saves/saveslot" + _saveSlot.ToString());
        }

        _vertices = HeightsToVerts(data.points);

        var loadObstacles = data.obstacles;
        for (var i = 0; i < loadObstacles.Length; i += 2)
        {
            var typeCode = (int)loadObstacles[i];
            var x = loadObstacles[i + 1];
            var newObstacle = Instantiate(_obstaclePrefabs[typeCode], new Vector3(x, 0, 5), Quaternion.identity, obstaclesTransform);
            var script = newObstacle.AddComponent<Obstacle>();
            script.typeCode = typeCode;
            obstacles.Add(script);
        }

        AddTriangles();
        UpdateMesh();
        UpdateEdgeCollider();

        foreach (var obstacle in obstacles)
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

        foreach (var obstacle in obstacles)
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
        _vertices[i] = newPoint;

        UpdateMesh();

        UpdateEdgeCollider();
        UpdateObstacles();
    }
}
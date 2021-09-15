using UnityEngine;

public class TerrainEditor : MonoBehaviour
{
    public Terrain terrain;

    [HideInInspector]
    public string editMode = "Move";

    bool _newSim = false;

    private void Start()
    {
        _newSim = PlayerPrefs.GetInt("newSim") == 1;

        if (_newSim)
        {
            terrain.SaveDefaultMesh();
        }

        terrain.LoadTerrain();
    }

    public void AddObstacle(int typeCode)
    {
        GameObject obstacleEditor = new GameObject().AddComponent<ObstacleEditor>().gameObject;
        obstacleEditor.transform.parent = terrain.obstaclesTransform;
        GameObject newObstacle = Instantiate(terrain.obstaclePrefabs[typeCode], Camera.main.transform.position, Quaternion.identity, obstacleEditor.transform);
        Obstacle obstacle = newObstacle.AddComponent<Obstacle>();
        obstacle.typeCode = typeCode;
        terrain.AddObstacle(obstacle);
    }
    
    public void DeleteObstacle(Obstacle obstacle)
    {
        terrain.RemoveObstacle(obstacle);
        Destroy(obstacle.gameObject);
        terrain.Save();
    }
}
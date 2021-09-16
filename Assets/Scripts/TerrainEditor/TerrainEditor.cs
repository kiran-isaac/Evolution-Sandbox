using UnityEngine;

public class TerrainEditor : TerrainBase
{
    [HideInInspector]
    public string editMode = "Move";

    bool newSim = false;

    private void Start()
    {
        isEditor = true;

        Init();

        newSim = PlayerPrefs.GetInt("newSim") == 1;

        if (newSim)
        {
            SaveDefaultMesh();
        }

        LoadTerrain();
    }

    protected override void LoadObstacle(int typeCode, float x)
    {
        var newObstacle = Instantiate(obstaclePrefabs[typeCode], new Vector3(x, 0, 5), Quaternion.identity, obstaclesTransform);
        var script = newObstacle.AddComponent<ObstacleEditor>();
        script.typeCode = typeCode;
        obstacles.Add(script);
    }

    public void AddObstacle(int typeCode)
    {
        GameObject newObstacle = Instantiate(obstaclePrefabs[typeCode], Camera.main.transform.position, Quaternion.identity, obstaclesTransform);
        ObstacleEditor obstacle = newObstacle.AddComponent<ObstacleEditor>();
        obstacle.typeCode = typeCode;
        AddObstacle(obstacle);
    }
    
    public void DeleteObstacle(Obstacle obstacle)
    {
        RemoveObstacle(obstacle);
        Destroy(obstacle.gameObject);
        Save();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainSaveData
{
    public float[] points;

    public float[] obstacles;

    [System.NonSerialized]
    public void AddObstacle(float type, float x)
    {
        var tempObstacles = new List<float>();

        tempObstacles.AddRange(obstacles);

        tempObstacles.Add(type, x);

        obstacles = tempObstacles.ToArray();
    }
}

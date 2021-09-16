using System;
using UnityEngine;

public class ObstacleEditor : Obstacle
{
    private TerrainEditor terrainEditor;

    private void Start()
    {
        terrainEditor = GameObject.Find("Terrain").GetComponent<TerrainEditor>();
    }

    private void OnMouseDrag()
    {
        if (terrainEditor.editMode == "Move")
        {
            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            UpdatePosAndAngle(Mathf.Max(Mathf.Min(newPos.x, terrainEditor.groundSize - 1), 1));
        }
    }

    private void OnMouseUp()
    {
        terrain.Save();
    }

    private void OnMouseDown()
    {
        if (terrainEditor.editMode == "Delete")
        {
            terrainEditor.DeleteObstacle(this);
        }
    }
}

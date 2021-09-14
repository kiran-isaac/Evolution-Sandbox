using System;
using UnityEngine;

public class ObstacleEditor : Obstacle
{
    private TerrainEditor _terrainEditor;

    private void Start()
    {
        _terrainEditor = GameObject.Find("Terrain").GetComponent<TerrainEditor>();
    }

    private void OnMouseDrag()
    {
        if (_terrainEditor.editMode == "Move")
        {
            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            UpdatePosAndAngle();
        }
    }

    private void OnMouseUp()
    {
        terrain.Save();
    }

    private void OnMouseDown()
    {
        if (_terrainEditor.editMode == "Delete")
        {
            _terrainEditor.DeleteObstacle(this);
        }
    }
}

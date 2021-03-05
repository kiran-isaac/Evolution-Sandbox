﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vert : MonoBehaviour
{
    public int pointLockIndex;

    public TerrainManager terrainManager;

    private void OnMouseDrag()
    {
        // Gets global coords of mouse pos
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        newPos.y = Mathf.Min(Mathf.Max(newPos.y, transform.parent.position.y + 1), 100);
        
        // Removes x motion
        newPos = new Vector3(transform.position.x, newPos.y, 0);

        // Moves point to newPos
        transform.position = newPos;

        // Updates terrain to match
        terrainManager.UpdatePoint(pointLockIndex, newPos);
    }

    private void OnMouseUp()
    {
        terrainManager.UpdateEdgeCollider();
        terrainManager.UpdateObstacles();

        terrainManager.Save();
    }
}

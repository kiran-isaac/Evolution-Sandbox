﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vert : MonoBehaviour
{
    public int pointLockIndex;

    public TerrainBase terrain;

    private void Awake()
    {
        terrain = GameObject.Find("Terrain").GetComponent<TerrainBase>();
    }

    private void OnMouseDrag()
    {
        // Gets global coords of mouse pos
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        newPos.y = Mathf.Min(Mathf.Max(newPos.y, transform.parent.position.y + 1), 100);
        
        // Removes x motion
        var trans = transform;
        
        newPos = new Vector3(trans.position.x, newPos.y, 0);

        // Moves point to newPos
        trans.position = newPos;

        // Updates terrain to match
        terrain.UpdatePoint(pointLockIndex, newPos);
    }

    private void OnMouseUp()
    {
        terrain.UpdateEdgeCollider();
        terrain.UpdateObstacles();

        terrain.Save();
    }
}

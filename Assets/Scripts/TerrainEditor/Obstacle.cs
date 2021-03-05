using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Unity with github test

    public Texture2D moveCursor;

    public TerrainManager terrainManager;

    SpriteRenderer render;

    Vector3 lastPosition;

    private void Start()
    {
        render = GetComponent<SpriteRenderer>();
        terrainManager = GameObject.Find("Ground").GetComponent<TerrainManager>();
    }

    private void OnMouseEnter()
    {
        Cursor.SetCursor(moveCursor, new Vector2(16, 16), CursorMode.ForceSoftware);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void OnMouseDrag()
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(newPos.x, terrainManager.GetHeightAtPoint(newPos.x) + 0.4f, 5f);
        transform.rotation = Quaternion.Euler(0f, 0f, terrainManager.GetAngleAtPoint(newPos.x) - 90);
    }
}

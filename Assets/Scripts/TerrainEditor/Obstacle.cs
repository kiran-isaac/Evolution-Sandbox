using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Texture2D moveCursor;

    public int typeCode;

    public TerrainManager terrainManager;

    SpriteRenderer render;

    Vector3 lastPosition;

    private void Awake()
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

    public void UpdatePosAndAngle(float x)
    {
        float angle = terrainManager.GetAngleAtPoint(x) - 90;

        Vector3 newPos = new Vector3(x, terrainManager.GetHeightAtPoint(x), 5f);
        transform.position = newPos;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void OnMouseDrag()
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        UpdatePosAndAngle(newPos.x);
    }

    private void OnMouseUp()
    {
        terrainManager.Save();
    }
}

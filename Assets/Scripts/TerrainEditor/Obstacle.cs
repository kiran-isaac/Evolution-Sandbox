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
        transform.position = new Vector3(x, terrainManager.GetHeightAtPoint(x) + 0.35f, 5f);
        transform.rotation = Quaternion.Euler(0f, 0f, terrainManager.GetAngleAtPoint(x) - 90);
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

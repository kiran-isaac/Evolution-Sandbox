using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int typeCode;

    public TerrainManager terrainManager;

    private void Awake()
    {
        terrainManager = GameObject.Find("Ground").GetComponent<TerrainManager>();
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

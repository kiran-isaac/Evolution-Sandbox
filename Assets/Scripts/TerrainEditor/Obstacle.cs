using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int typeCode;

    public TerrainEditor terrainEditor;

    private void Awake()
    {
        terrainEditor = GameObject.Find("Ground").GetComponent<TerrainEditor>();
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void UpdatePosAndAngle(float x)
    {
        float angle = terrainEditor.GetAngleAtPoint(x) - 90;

        Vector3 newPos = new Vector3(x, terrainEditor.GetHeightAtPoint(x), 5f);
        transform.position = newPos;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void OnMouseDrag()
    {
        if (terrainEditor.editMode == "Move")
        {
            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            UpdatePosAndAngle(newPos.x);
        }
    }

    private void OnMouseUp()
    {
        terrainEditor.Save();
    }

    private void OnMouseDown()
    {
        if (terrainEditor.editMode == "Delete")
        {
            terrainEditor.DeleteObstacle(this);
        }
    }
}

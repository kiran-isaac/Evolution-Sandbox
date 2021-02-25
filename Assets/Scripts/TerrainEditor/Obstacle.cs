using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Unity with github test

    public Texture2D moveCursor;

    SpriteRenderer render;

    bool selected = false;

    Vector3 lastPosition;

    private void Start()
    {
        render = GetComponent<SpriteRenderer>();
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
        Vector3 delta = newPos - lastPosition;

        transform.Translate(new Vector3(delta.x, delta.y));

        lastPosition = new Vector3(newPos.x, newPos.y);
    }
}

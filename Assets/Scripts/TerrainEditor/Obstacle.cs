using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int typeCode;
    public Terrain terrain;

    private void Awake()
    {
        terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void UpdatePosAndAngle()
    {
        float x = transform.position.x;
        
        float angle = terrain.GetAngleAtPoint(x) - 90;

        Vector3 newPos = new Vector3(x, terrain.GetHeightAtPoint(x), 5f);
        transform.position = newPos;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}

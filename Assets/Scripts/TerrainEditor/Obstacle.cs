using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int typeCode;
    public TerrainSim terrain;

    private void Awake()
    {
        terrain = GameObject.Find("Terrain").GetComponent<TerrainSim>();
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void UpdatePosAndAngle(float x = float.NaN)
    {
        if (float.IsNaN(x))
        {
            x = transform.position.x;
        }
        
        
        float angle = terrain.GetAngleAtPoint(x);

        Vector3 newPos = new Vector3(x, terrain.GetHeightAtPoint(x) - 0.01f, 5f);
        transform.position = newPos;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
    }
}

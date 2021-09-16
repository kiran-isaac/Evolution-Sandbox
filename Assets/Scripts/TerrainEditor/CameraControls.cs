using UnityEngine;

public class CameraControls : MonoBehaviour
{
    Vector3 lastPosition;

    readonly float vel = 0.02f;

    int groundSize;

    void Start()
    {
        groundSize = GameObject.Find("Terrain").GetComponent<TerrainBase>().groundSize;
    }

    void Update()
    {
        Pan();
    }

    private void Pan()
    {
        if (Input.GetMouseButtonDown(1))
        {
            lastPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 newPos = Input.mousePosition;
            Vector3 delta = newPos - lastPosition;

            transform.Translate(-new Vector3(delta.x * vel, delta.y * vel));

            // Makes it so the camera can't go below 0 or above 100
            transform.position = new Vector3(Mathf.Max(Mathf.Min(transform.position.x, groundSize), 0),
                Mathf.Min(Mathf.Max(-1, transform.position.y), 100), -10);

            lastPosition = newPos;
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimObstacle : MonoBehaviour
{
    public Texture2D moveCursor;

    public int typeCode;

    public SimTerrain terrainManager;


    private void Awake()
    {
        terrainManager = GameObject.Find("Ground").GetComponent<SimTerrain>();
    }

    public void UpdatePosAndAngle(float x)
    {
        float angle = terrainManager.GetAngleAtPoint(x) - 90;

        Vector3 newPos = new Vector3(x, terrainManager.GetHeightAtPoint(x), 0);
        transform.position = newPos;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}

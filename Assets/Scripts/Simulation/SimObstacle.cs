using UnityEngine;

namespace Simulation
{
    public class SimObstacle : MonoBehaviour
    {
        public Texture2D moveCursor;

        public int typeCode;

        public TerrainSim terrainManager;


        private void Awake()
        {
            terrainManager = GameObject.Find("Ground").GetComponent<TerrainSim>();
        }

        public void UpdatePosAndAngle(float x)
        {
            float angle = terrainManager.GetAngleAtPoint(x) - 90;

            Vector3 newPos = new Vector3(x, terrainManager.GetHeightAtPoint(x), 5);
            transform.position = newPos;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}

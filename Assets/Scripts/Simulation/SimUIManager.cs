using UnityEngine;
using UnityEngine.SceneManagement;

namespace Simulation
{
    public class SimUIManager : MonoBehaviour
    {
        public void OnBack()
        {
            PlayerPrefs.SetInt("newSim", 0);
            SceneManager.LoadScene("TerrainEditor");
        }
    }
}

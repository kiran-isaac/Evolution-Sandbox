using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimUIManager : MonoBehaviour
{
    public void OnBack()
    {
        PlayerPrefs.SetInt("newSim", 0);
        SceneManager.LoadScene("TerrainEditor");
    }
}

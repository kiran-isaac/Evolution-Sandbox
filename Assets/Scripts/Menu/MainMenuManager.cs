using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenuManager : MonoBehaviour
{
    public bool newSim;
    public int saveSlot;

    public GameObject confirmPanel;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void OnNewSimulation()
    {
        newSim = true;
    }

    public void OnLoadSimulation()
    {
        newSim = false;
    }

    public void OnConfirm()
    {
        SceneManager.LoadScene("TerrainEditor");
    }

    public void OnSlot(int i)
    {
        saveSlot = i;

        if (newSim && 
            File.Exists(Application.persistentDataPath + "/Terrain Saves/saveslot" + saveSlot.ToString() + ".trn"))
        {
            confirmPanel.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("TerrainEditor");
        }
    }
}
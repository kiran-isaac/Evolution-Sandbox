using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MainMenuManager : MonoBehaviour
    {
        public bool newSim;
        public int saveSlot;

        public GameObject confirmPanel;

        public void OnNewSimulation()
        {
            PlayerPrefs.SetInt("newSim", 1);
            newSim = true;
        }

        public void OnLoadSimulation()
        {
            PlayerPrefs.SetInt("newSim", 0);
            newSim = false;
        }

        public void OnConfirm()
        {
            SceneManager.LoadScene("TerrainEditor");
        }

        public void OnSlot(int i)
        {
            PlayerPrefs.SetInt("SaveSlot", i);
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
}
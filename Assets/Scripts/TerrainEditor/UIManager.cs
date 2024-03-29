﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Animator anim;

    public GameObject dropdownPanel;

    public TerrainEditor terrainEditor;

    public GameObject rockPrefab;
    public GameObject spikePrefab;

    public GameObject move;
    public GameObject bin;

    public Transform obstaclesTransform;

    bool open = false;

    // Makes sure the buttons are not pressed while the animations are still playing, 
    // as each are 0.2 seconds long
    float timer = 0;

    private void Start()
    {
        terrainEditor = GameObject.Find("Terrain").GetComponent<TerrainEditor>();

        terrainEditor.editMode = "Move";

        move.GetComponent<Image>().color = Color.gray;
    }

    public void OnMove()
    {
        terrainEditor.editMode = "Move";

        move.GetComponent<Image>().color = Color.grey;
        bin.GetComponent<Image>().color = Color.white;
    }

    public void OnBin()
    {
        terrainEditor.editMode = "Delete";

        bin.GetComponent<Image>().color = Color.gray;
        move.GetComponent<Image>().color = Color.white;
    }

    public void Back()
    {
        terrainEditor.Save();
        SceneManager.LoadScene("Menu");
    }

    public void AddRock()
    {
        terrainEditor.AddObstacle(0);
    }

    public void AddSpike()
    {
        terrainEditor.AddObstacle(1);
    }

    // Is called when the "Start Simulation" button is pressed
    public void StartSimulation()
    {
        SceneManager.LoadScene("Simulation");
    }

    // Is called when the obstacles button is clicked
    public void DropdownToggle()
    {
        if (timer > 0.2)
        {
            if (open)
            {
                DropdownClose();
            }
            else
            {
                timer = 0;
                anim.Play("dropdownOpen");
                EventSystem.current.SetSelectedGameObject(dropdownPanel);
                open = true;
            }
        }
    }

    // Is called when a the panel containing the buttons is deselected, or any of the
    // buttons pressed
    public void DropdownClose()
    {
        if (timer > 0.2)
        {
            anim.Play("dropdownClose");
            open = false;
            timer = 0;
        }
    }

    private void Update()
    {
        // Adds the length of time between frames each frame
        timer += Time.deltaTime;
    }
}

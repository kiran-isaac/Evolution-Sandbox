﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Animator anim;

    public GameObject dropdownPanel;

    public TerrainManager terrainManager;

    public GameObject rockPrefab;

    public Transform obstaclesTransform;

    bool open = false;

    // Makes sure the buttons are not pressed while the animations are still playing, 
    // as each are 0.2 seconds long
    float timer = 0;

    private void Start()
    {
        terrainManager = GameObject.Find("Ground").GetComponent<TerrainManager>();
    }

    public void Back()
    {
        terrainManager.Save();
        SceneManager.LoadScene("Menu");
    }

    public void AddRock()
    {
        GameObject newObstacle = Instantiate(rockPrefab, Vector3.zero, Quaternion.identity, obstaclesTransform);
        Obstacle script = newObstacle.GetComponent<Obstacle>();
        terrainManager.obstacles.Add(script);
        script.UpdatePosAndAngle(newObstacle.transform.position.x);
        script.typeCode = 0;
        terrainManager.UpdateObstacles();
        terrainManager.Save();
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

using System.Collections;
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
        SceneManager.LoadScene("Menu");
    }

    public void AddRock()
    {
        Instantiate(rockPrefab, new Vector3(0, terrainManager.GetHeightAtPoint(0) + 0.3f, 5), Quaternion.identity, obstaclesTransform);
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

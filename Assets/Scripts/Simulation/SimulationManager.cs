using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SimulationManager : MonoBehaviour
{
    public GameObject nodePrefab;

    public void Test()
    {
        Instantiate(nodePrefab, new Vector3(-0.5f, 5, 0), Quaternion.identity, transform);
    }
}

[CustomEditor(typeof(SimulationManager))]
class TestSimEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SimulationManager simManager = (SimulationManager)target;

        if (GUILayout.Button("Generate N Test Creatures"))
        {
            simManager.Test();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SimulationManager : MonoBehaviour
{
    public GameObject nodePrefab;
    public GameObject musclePrefab;

    public int n;
    public int m;

    public void GenerateTestCreature()
    {
        Creature.Generate(n, m, nodePrefab, musclePrefab);
    }

    private void OnValidate()
    {
        n = Mathf.Max(Mathf.Min(n, 10), 2);
        m = Mathf.Min(Mathf.Max(m, n - 1), (n * (n - 1)) / 2);
    }
}

[CustomEditor(typeof(SimulationManager))]
class SimEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SimulationManager simManager = (SimulationManager)target;

        if (GUILayout.Button("Generate Test Creature"))
        {
            simManager.GenerateTestCreature();
        }
    }
}

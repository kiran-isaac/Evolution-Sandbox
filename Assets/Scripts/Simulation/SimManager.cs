using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SimManager : MonoBehaviour
{
    public GameObject nodePrefab;
    public GameObject musclePrefab;

    public SimTerrain ground;

    public int n;
    public int m;

    public void GenerateTestCreature()
    {
        Creature.Generate(n, m, nodePrefab, musclePrefab, ground.GetHeightAtPoint(0) + 6);
    }

    private void OnValidate()
    {
        n = Mathf.Max(Mathf.Min(n, 10), 2);
        m = Mathf.Min(Mathf.Max(m, n - 1), (n * (n - 1)) / 2);
    }
}

[CustomEditor(typeof(SimManager))]
class SimEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SimManager simManager = (SimManager)target;

        if (GUILayout.Button("Generate Test Creature"))
        {
            simManager.GenerateTestCreature();
        }
    }
}

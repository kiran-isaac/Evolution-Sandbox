using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WheelTest : MonoBehaviour
{
    Rigidbody2D rb;
    public float torque;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.AddTorque(-torque/100);
    }
}

//[CustomEditor(typeof(CircleTest))]
//class TestEditor : Editor
//{
//    CircleTest circle;

//    public override void OnInspectorGUI()
//    {
//        circle = GameObject.Find("Wheel").GetComponent<CircleTest>();

//        DrawDefaultInspector();

//        if (GUILayout.Button("Regenerate"))
//        {
//            circle.Regenerate();
//        }
//    }
//}
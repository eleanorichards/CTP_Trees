//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[RequireComponent(typeof(MeshFilter))]
//[CustomEditor(typeof(BezierCurve))]
//public class BezierInspector : Editor
//{
//    private BezierCurve curve;
//    private Vector3[] nodes;
//    private const int lineSteps = 10;
//    private const int stepsPerCurve = 10;
//    private float directionScale = 0.5f;

//    private void Start()
//    {
//    }

//    private void OnSceneGUI()
//    {
//        //somehow need to set these separately
//        //foreach curve in curves
//        curve = target as BezierCurve;
//        nodes = curve.nodes;

//        //draw base lines
//        for (int x = 0; x < nodes.Length - 1; x++)
//        {
//            Handles.color = Color.green;
//            Handles.DrawLine(nodes[x] + curve.GetPoint(0f), nodes[x + 1] + curve.GetPoint(0f));
//        }

//        //Draw main lines
//        Vector3 p0 = nodes[0];
//        for (int y = 1; y < curve.nodes.Length; y += 3)
//        {
//            //Handles.color = Color.black;
//            Vector3 p1 = nodes[y];
//            Vector3 p2 = nodes[y + 1];
//            Vector3 p3 = nodes[y + 2];

//            //Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
//            Handles.DrawBezier(p0 + curve.GetPoint(0f), p3 + curve.GetPoint(0f), p1 + curve.GetPoint(0f), p2 + curve.GetPoint(0f), Color.white, null, 2f);
//            p0 = p3;
//            ShowDirections();
//        }
//    }

//    private void ShowDirections()
//    {
//        Handles.color = Color.red;
//        Vector3 point = curve.GetPoint(0f);
//        Handles.DrawLine(point, point + curve.GetDirection(0f) * directionScale);
//        int steps = stepsPerCurve * curve.CurveCount;
//        for (int x = 1; x <= steps; x++)
//        {
//            point = curve.GetPoint(x / (float)steps);
//            Handles.DrawLine(point, point + curve.GetDirection(x / (float)steps) * directionScale);
//            Handles.color = Color.yellow;
//            // Handles.DrawLine(point, point + curve.GetTangent(i / (float)steps) * directionScale);
//        }
//    }

//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        curve = target as BezierCurve;
//        if (GUILayout.Button("Add Curve"))
//        {
//            Undo.RecordObject(curve, "Add Curve");
//            curve.AddCurve();
//            EditorUtility.SetDirty(curve);
//        }
//        if (GUILayout.Button("Place Shapes Along Spline"))
//        {
//            curve.ExtrudeShape();
//        }
//        if (GUILayout.Button("Place branches"))
//        {
//            curve.AddBranches();
//        }
//    }
//}
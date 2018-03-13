using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(MeshFilter))]
[CustomEditor(typeof(BezierCurve))]
public class BezierInspector : Editor
{
    private BezierCurve spline;
    private Vector3[] nodes;
    private const int lineSteps = 10;
    private const int stepsPerCurve = 10;
    private float directionScale = 0.5f;

    private void Start()
    {
    }

    private void OnSceneGUI()
    {
        //somehow need to set these separately
        //foreach curve in curves
        spline = target as BezierCurve;
        nodes = spline.nodes;

        //draw base lines
        for (int x = 0; x < nodes.Length - 1; x++)
        {
            Handles.color = Color.green;
            Handles.DrawLine(nodes[x] + spline.GetPoint(0f), nodes[x + 1] + spline.GetPoint(0f));
        }

        //Draw main lines
        Vector3 p0 = nodes[0];
        for (int y = 1; y < spline.nodes.Length; y += 3)
        {
            if (y >= spline.nodes.Length - 2)
                return;
            //Handles.color = Color.black;
            Vector3 p1 = nodes[y];
            Vector3 p2 = nodes[y + 1];
            Vector3 p3 = nodes[y + 2];

            //Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
            Handles.DrawBezier(p0 + spline.GetPoint(0f) + spline.transform.position, p3 + spline.GetPoint(0f), p1 + spline.GetPoint(0f), p2 + spline.GetPoint(0f), Color.red, null, 2f);
            p0 = p3;
            // ShowDirections();
        }
    }

    private void ShowDirections()
    {
        Handles.color = Color.yellow;
        Vector3 point = spline.GetPoint(0f);
        Handles.DrawLine(point, point + spline.GetDirection(0f) * directionScale);
        int steps = stepsPerCurve * spline.CurveCount;
        for (int x = 1; x <= steps; x++)
        {
            point = spline.GetPoint(x / (float)steps);
            Handles.DrawLine(point, point + spline.GetDirection(x / (float)steps) * directionScale);
            // Handles.DrawLine(point, point + curve.GetTangent(i / (float)steps) * directionScale);
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        spline = target as BezierCurve;
        if (GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(spline, "Add Curve");
            spline.AddCurve();
            EditorUtility.SetDirty(spline);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierCurve))]
public class BezierInspector : Editor {

    private BezierCurve curve;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private Vector3[] nodes;
    private const int lineSteps = 10;
    private int incrementor;
    private const int stepsPerCurve = 10;
    private float directionScale = 0.5f;
    private void OnSceneGUI()
    {
        curve = target as BezierCurve;
        nodes = curve.nodes;
        //draw base lines
        for (int i = 0; i < curve.nodes.Length - 1; i ++)
        {
            Handles.color = Color.green;
            Handles.DrawLine(nodes[i], nodes[i + 1]);
        }

        //Draw main lines
        Vector3 p0 = nodes[0];
        for(int i = 1; i < curve.nodes.Length; i+=3)
        {
            Handles.color = Color.black;
            Vector3 p1 = nodes[i];
            Vector3 p2 = nodes[i+1];
            Vector3 p3 = nodes[i+2];

            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
            p0 = p3;
            ShowDirections();
        }
    }

    private void ShowPoint(int index)
    {
        nodes[index] = handleTransform.TransformPoint(curve.nodes[index]);
        EditorGUI.BeginChangeCheck();
        Handles.DoPositionHandle(nodes[index], handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "Move Point");
            EditorUtility.SetDirty(curve);
            curve.nodes[index] = handleTransform.InverseTransformPoint(nodes[index]);
        }
    }

    private void ShowDirections()
    {
        Handles.color = Color.red;
        Vector3 point = curve.GetPoint(0f);
        Handles.DrawLine(point, point + curve.GetDirection(0f) * directionScale);
        int steps = stepsPerCurve * curve.CurveCount;
        for (int i = 1; i <= steps; i++)
        {
            point = curve.GetPoint(i / (float)steps);
            Handles.DrawLine(point, point + curve.GetDirection(i / (float)steps) * directionScale);
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        curve = target as BezierCurve;
        if (GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(curve, "Add Curve");
            curve.AddCurve();
            EditorUtility.SetDirty(curve);
        }
    }
}

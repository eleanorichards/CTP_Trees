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

    private void OnSceneGUI()
    {
        curve = target as BezierCurve;
        nodes = curve.nodes;
        handleTransform = curve.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;
        for (int i = 0; i < nodes.Length - 1; i ++)
        {
            Handles.color = Color.green;
            Handles.DrawLine(nodes[i], nodes[i + 1]);
        }

        //Draw main lines
        for(int i = 1; i < nodes.Length - 3; i+=3)
        {
            Handles.color = Color.black;
            Vector3 lineStart = curve.GetPoint(nodes, i, 0f);
            for (int x = 1; x <= lineSteps; x++)
            {
                Vector3 lineEnd = curve.GetPoint(nodes, i, x / (float)lineSteps);
                Handles.DrawLine(lineStart, lineEnd);
                
                lineStart = lineEnd;
            }
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

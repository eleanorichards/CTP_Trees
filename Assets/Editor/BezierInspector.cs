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

        for(int i = 0; i < nodes.Length; i++)
        {
            //draw transform handles for each node
            ShowPoint(i);
        }
        //Draw main lines
        for(int i = 1; i < nodes.Length; i++)
        {
            Handles.color = Color.grey;
            Handles.DrawLine(nodes[i], nodes[i - 1]);

            Handles.color = Color.cyan;
            Vector3 lineStart = curve.GetPoint(0f);
            for (int x = 1; x <= lineSteps; x++)
            {
                Vector3 lineEnd = curve.GetPoint(x / (float)lineSteps);
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

   
}

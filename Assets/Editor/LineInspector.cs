using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Line))]
public class LineInspector : Editor {
    private Line line;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private Vector3[] nodes;

    private void OnSceneGUI()
    {
        line = target as Line;
        nodes = line.nodes;

        handleTransform = line.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

        for (int i = 0; i < nodes.Length; i++)
        {
            //draw transform handles for each node
            ShowPoint(i);         
        }
        Handles.color = Color.blue;
        for (int i = 1; i < nodes.Length; i++)
        {
            Handles.DrawLine(nodes[i], nodes[i-1]);
        }   

    }

    private void ShowPoint(int index)
    {
        nodes[index] = handleTransform.TransformPoint(line.nodes[index]);
        EditorGUI.BeginChangeCheck();
        Handles.DoPositionHandle(nodes[index], handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(line, "Move Point");
            EditorUtility.SetDirty(line);
            line.nodes[index] = handleTransform.InverseTransformPoint(nodes[index]);
        }
    }
}

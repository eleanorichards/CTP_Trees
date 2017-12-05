using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Line))]
public class LineInspector : Editor {
    private Vector3[] nodes;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnSceneGUI()
    {
        Line line = target as Line;
        nodes = line.nodes;

        Transform handleTransform = line.transform;
        Quaternion handleRotation = handleTransform.rotation;
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = handleTransform.TransformPoint(line.nodes[i]);
        }
        //Vector3 p0 = handleTransform.TransformPoint(line.p0);
       // Vector3 p1 = handleTransform.TransformPoint(line.p1);

        Handles.color = Color.blue;
        for (int i = 0; i < nodes.Length - 1; i++)
        {
            Handles.DrawLine(nodes[i], nodes[i+1]);
            Handles.DoPositionHandle(nodes[i], handleRotation);
            
        }
        
    }
}

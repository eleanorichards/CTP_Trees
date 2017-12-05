using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour {
    public Vector3[] nodes;

    public Vector3 GetPoint(float t)
    {
        Vector3 transformPoint = Vector3.zero;
        for(int i = 0; i < nodes.Length; i += 3)
        {
            transformPoint =  transform.TransformPoint(Bezier.GetPoint(nodes[i], nodes[i+1], nodes[i+2], t));
        }
        return transformPoint;
    }

    public void Reset()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = new Vector3(i, 0f, 0f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour {
    public Vector3[] nodes;

    public Vector3 GetPoint(float t)
    {
        return transform.TransformPoint(Bezier.GetPoint(nodes[0], nodes[1], nodes[2], t));
        //Vector3 transformPoint = Vector3.zero;
        //for (int i = 3; i < nodes.Length; i++)
        //{
        //    transformPoint = transform.TransformPoint(Bezier.GetPoint(nodes[i], nodes[i - 1], nodes[i - 2], t));
        //    i += 2;
        //}
        //return transformPoint;
    }

    public void Reset()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = new Vector3(i, 0f, 0f);
        }
    }
}

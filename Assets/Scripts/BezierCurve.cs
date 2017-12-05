using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour {
    public Vector3[] nodes;
    void Start()
    {

    }
    public Vector3 GetPoint(Vector3[] nodes, float t)
    {
        Vector3 transformPoint = Vector3.zero;

        
        transformPoint = transform.TransformPoint(Bezier.GetPoint(nodes, t));
        
        return transformPoint;
    }

    public void Reset()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = new Vector3(i, 0f, 0f);
        }
    }

    //public Vector3 GetVelocity(float t)
    //{
    //    return transform.TransformPoint(Bezier.GetFirstDerivative(nodes[0], nodes[1], nodes[2], t)) -
    //        transform.position;
    //}
}

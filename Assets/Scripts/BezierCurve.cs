using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BezierCurve : MonoBehaviour
{
    public Vector3[] nodes;

    void Start()
    {

    }

    public Vector3 GetPoint(Vector3[] nodes, int startNode, float t)
    {
        Vector3 transformPoint = Vector3.zero;
        transformPoint = transform.TransformPoint(Bezier.GetPoint(nodes, startNode, t));
        return transformPoint;
    }

    public void Reset()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = new Vector3(0f, i, 0f);
        }
    }

    public void AddCurve()
    {
        Vector3 node = nodes[nodes.Length - 1];
        System.Array.Resize(ref nodes, nodes.Length + 3);
        node.x += 1f;
        nodes[nodes.Length - 3] = node;
        node.x += 1f;
        nodes[nodes.Length - 2] = node;
        node.x += 1f;
        nodes[nodes.Length - 1] = node;
    }

    
}

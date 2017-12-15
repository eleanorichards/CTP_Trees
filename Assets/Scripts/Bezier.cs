using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Bezier
{
 
    public static Vector3 GetPoint(Vector3 node1, Vector3 node2, Vector3 node3, Vector3 node4, float t)
    {
   
        float omt = 1f - t;
        float omt2 = omt * omt;
        float t2 = t * t;
        return node1 * (omt2 * omt) +
            node2 * (3f * omt2 * t) +
            node3 * (3f * (omt * t2)) +
            node4 * (t2 * t);

    }

    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        float omt = 1f - t;
        float omt2 = omt * omt;
        float t2 = t * t;

        return
            3f * omt2 * (p1 - p0) +
            6f * omt * t * (p2 - p1) +
            3f * t2 * (p3 - p2);
    }

}

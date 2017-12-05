using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Bezier
{
    public Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t);
    }
    //public static Vector3 GetPoint(Vector3[] nodes, float t)
    //{
    //    float omt = 1f - t;
    //    float omt2 = omt * omt;
    //    float t2 = t * t;
    //    return nodes[0] * (omt2 * omt) +
    //        nodes[1] * (3f * omt2 * t) +
    //        nodes[2] * (3f * (omt * t2)) +
    //        nodes[3] * (t2 * t);
    //}

    //SIMPLYFYING LERP MATH
    /*      
    pt = 
    p0((1-t)^3) + 
    p1(3(1-t)^2t) +
    p2(3(1-t)t^2) +
    p3(t^3)
    
    */
}

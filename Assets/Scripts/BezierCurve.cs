using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BezierCurve : MonoBehaviour
{
    public Vector3[] nodes;
    public GameObject segments;
    public splineGeometry splineGeo;

    public Vector3 GetPoint(float t)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = nodes.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }

        return transform.TransformPoint(Bezier.GetPoint(nodes[i], nodes[i + 1] , nodes[i + 2], nodes[i + 3], t));      
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

    public int CurveCount
    {
        get
        {
            return (nodes.Length - 1) / 3;
        }
    }

    //show directional lines (for following splines)
    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }

    //returns magnitude of direction
    public Vector3 GetVelocity(float t)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = nodes.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }
        return transform.TransformPoint(Bezier.GetFirstDerivative(
            nodes[i], nodes[i + 1], nodes[i + 2], nodes[i + 3], t)) - transform.position;
    }

    public Vector3 GetTangent(float t)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = nodes.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }
        float omt = 1f - t;
        float omt2 = omt * omt;
        float t2 = t * t;
        Vector3 tangent =
                    nodes[i] * (-omt2) +
                    nodes[i+1] * (3 * omt2 - 2 * omt) +
                    nodes[i+2] * (-3 * t2 + 2 * t) +
                    nodes[i+3] * (t2);
        return tangent.normalized;
    }

    public Vector3 GetNormal2D(float t)
    {
        Vector3 tng = GetTangent(t);
        return new Vector3(-tng.y, tng.x, 0f);
    }


    public Vector3 GetNormal3D(float t, Vector3 up)
    {
        Vector3 tng = GetTangent(t);
        Vector3 binormal = Vector3.Cross(up, tng).normalized;
        return Vector3.Cross(tng, binormal);

    }

    public Quaternion GetOrientation2D( float t)
    {
        Vector3 tng = GetTangent(t);
        Vector3 nrm = GetNormal2D(t);
        return Quaternion.LookRotation(tng, nrm);
    }

    public Quaternion GetOrientation3D(float t, Vector3 up)
    {
        Vector3 tng = GetTangent( t);
        Vector3 nrm = GetNormal3D( t, up);
        return Quaternion.LookRotation(tng, nrm);
    }

    public void ExtrudeShape()
    {
        //curve.segments.GetComponent<segmentPlacer>().PlaceShapes();
        Mesh mesh = splineGeo.GetMesh();
        var shape = splineGeo.GetExtrudeShape();
        var path = splineGeo.GetPath();

        splineGeo.Extrude(mesh, shape, path);
    }

}


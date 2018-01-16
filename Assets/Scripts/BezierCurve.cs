using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BezierCurve : MonoBehaviour
{
    public Vector3[] nodes;
    public GameObject segments;
    public GameObject branchPlacer;

    public int hierachyIndex;
    public int memberIndex;

    public int parentHierachy;
    public int parentIndex;
    public int globalIndex;

    public bool firstGroup = false;
    public bool secondGroup = false;
    public bool thirdgroup = false;

    //Initialisation
    void Start()
    {

    }

    public void SetInitialStatus(int _hierachyIndex, int _memberIndex)
    {
        hierachyIndex = _hierachyIndex;
        memberIndex = _memberIndex;
        parentHierachy = hierachyIndex - 1;
    }

    public void SetAllNodes(Vector3 node0Pos, Vector3 node1Pos)
    {
        float x = nodes[0].x;
        float y = nodes[0].y;
        float z = nodes[0].z;

        nodes[0] = node0Pos;
        nodes[1] = node1Pos;
        Debug.Log(node0Pos + " : " + node1Pos);
        Vector3 initialStep = node1Pos - node0Pos;

        switch (hierachyIndex)
        {
            case 0:
                PlaceTrunk();
                break;
            case 1:
                for (int i = 2; i < nodes.Length; i++)
                {
                    nodes[i] = nodes[i - 1] + initialStep;
                }
                break;
            case 2:
                for (int i = 2; i < nodes.Length; i++)
                {
                    nodes[i] = nodes[i - 1] + initialStep;
                }

                break;
            default:
                break;
        }
    }

   

    /// <summary>
    /// Returns set coordinates for index along tree
    /// </summary>
    public Vector2 GetSplineIndex()
    {
        return new Vector2(hierachyIndex, memberIndex);
    }


    public void SetParentIndex(int _parentIndex)
    {
        //parentHierachy = _parentHierachy;
        parentIndex = _parentIndex;
    }


    public void PlaceTrunk()
    {
        nodes[0] = new Vector3(0.0f, 0.0f, 0.0f);
        for (int i = 1; i < nodes.Length; i++)
        {
            nodes[i] = new Vector3(Random.Range(-0.5f, 0.5f), i, Random.Range(-0.5f, 0.5f));
        }
    }


    public void SetGlobalIndex(int _globalIndex)
    {
        globalIndex = _globalIndex;
    }

    public void AddBranches()
    {
        branchPlacer.GetComponent<BranchPlacer>().AddBranch();
    }
    

    public void AddCurve()
    {
        Vector3 node = nodes[nodes.Length - 1];
        System.Array.Resize(ref nodes, nodes.Length + 3);
        node.y += 1f;
        nodes[nodes.Length - 3] = node;
        node.y += 1f;
        nodes[nodes.Length - 2] = node;
        node.y += 1f;
        nodes[nodes.Length - 1] = node;
    }

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

    /// <summary>
    /// returns (nodes.length - 1) / 3
    /// </summary>
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

    

    public Vector3 GetNormal2D(float t)
    {
        Vector3 tng = GetDirection(t);
        return new Vector3(-tng.y, tng.x, 0f);
    }


    public Vector3 GetNormal3D(float t, Vector3 up)
    {
        Vector3 tng = GetDirection(t);
        Vector3 binormal = Vector3.Cross(up, tng).normalized;
        return Vector3.Cross(tng, binormal);

    }

    public Quaternion GetOrientation2D( float t)
    {
        Vector3 tng = GetDirection(t);
        Vector3 nrm = GetNormal2D(t);
        return Quaternion.LookRotation(tng, nrm);
    }

    public Quaternion GetOrientation3D(float t, Vector3 up)
    {
        Vector3 tng = GetDirection( t);
        Vector3 nrm = GetNormal3D( t, up);
        return Quaternion.LookRotation(tng, nrm);
    }

    public void ExtrudeShape()
    {
        segments.GetComponent<segmentPlacer>().PlaceShapes();
        //Mesh mesh = splineGeo.GetMesh();
        //var shape = splineGeo.GetExtrudeShape();
        //var path = splineGeo.GetPath();

        //splineGeo.Extrude(mesh, shape, path);
    }

}


﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BezierCurve : MonoBehaviour
{
    public Vector3[] nodes;

    public GameObject segments;
    public GameObject branchPlacer;
    private BezierCurve[] curves;
    private int branch_num = 5;
    private int hierachyIndex;
    private int memberIndex;

    private BezierCurve[] BranchGroup1;
    private BezierCurve[] BranchGroup2;
    private BezierCurve[] BranchGroup3;


    void Start()
    {
        for(int i = 0; i < nodes.Length; i++)
        {
            switch (gameObject.tag)
            {
                case "FIRST":
                    //excecute first algorithm...
                case "SECOND":
                    //etc...
                    break;
                case "THIRD":
                    //etc...
                    break;
                case "FOURTH":
                    //etc...
                default:
                    break;
            }
            
        }



    }
    /*  
    Vector3 position = new Vector3(Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f));
    */

    public void SetInitialStatus(int _hierachyIndex, int _memberIndex)
    {
        hierachyIndex = _hierachyIndex;
        memberIndex = _memberIndex;

        switch (hierachyIndex)
        {
            case 1:
                nodes[0] = new Vector3(0.0f,0.0f,0.0f);
                break;
            case 2:
                //for(int i = 0; i < branchGroup1.Length; i++)
                //nodes[0] = branchGroup1[i].GetPoint(memberIndex.Normalize());
                break;
            default:
                break;
        }

    }

    void FindAllCurves()
    {
       // curves = GameObject.FindGameObjectsWithTag("BezierSpline");
       /*   
        *   if(curves[i].CompareTag("FIRST")
        *   {
        *       BranchGroup1.Add(curves[i]);
        *   }   
        */
    }

    /// <summary>
    /// Sets branch shape for Trunks
    /// Should move this to own class eventually
    /// </summary>
    void SetGroupOneBranches()
    {

    }

    void SetGroupTwoBranches()
    {

    }

    void SetGroupThreeBranches()
    {

    }

    void SetGroupFourBranches()
    {

    }

    void SetGroupFiveBranches()
    {

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


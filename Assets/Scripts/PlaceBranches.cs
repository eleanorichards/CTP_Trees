﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBranches : MonoBehaviour
{
    private FractalGen fractalGen;
    private List<GameObject> BranchTransforms = new List<GameObject>();

    [Tooltip("Should be multiple of predecessor")]
    public int[] tierCount;

    private float[] tierProgress = new float[10];

    private int branchNum;
    private int stepsPerCurve = 5; //quality of curves
    private float thickness = 1.0f; //Reverse direction of hierachy

    private Vector3 newRot = Vector3.zero;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            BuildTree();
        }
    }

    private void BuildTree()
    {
        foreach (GameObject oldBranch in BranchTransforms)
        {
            //BranchTransforms.Remove(oldBranch);
            Destroy(oldBranch);
        }
        BranchTransforms.Clear();

        newRot = Vector3.zero;
        branchNum = 0;

        for (int i = 0; i < tierCount.Length; i++)
        {
            branchNum += tierCount[i];
        }

        for (int i = 0; i < branchNum; i++)
        {
            GameObject branch = new GameObject("branch" + i);
            BranchTransforms.Add(branch);
            LineRenderer line = BranchTransforms[i].AddComponent<LineRenderer>();
            BranchData _BD = BranchTransforms[i].AddComponent<BranchData>();
            BezierCurve spline = BranchTransforms[i].AddComponent<BezierCurve>();
            InitBranchValues(i, _BD);
            InitLineRenderer(line);
            InitBranchSpline(i, line, _BD, spline);
        }
        //Fractal pass
        for (int i = (tierCount[0] + tierCount[1]); i < branchNum; i++)
        {
            //AddFractals(BranchTransforms[i].GetComponent<BranchData>());
        }
        for (int i = branchNum - 1; i > -1; i--)
        {
            BranchData _BD = BranchTransforms[i].GetComponent<BranchData>();
            InitBranchRot(i, _BD);
        }
    }

    // Use this for initialization
    private void Start()
    {
        for (int i = 0; i < tierCount.Length; i++)
        {
            branchNum += tierCount[i];
        }
        for (int i = 0; i < tierProgress.Length; i++)
        {
            tierProgress[i] = 0.0f;
        }

        for (int i = 0; i < branchNum; i++)
        {
            GameObject branch = new GameObject("branch" + i);
            BranchTransforms.Add(branch);
            LineRenderer line = BranchTransforms[i].AddComponent<LineRenderer>();
            BranchData _BD = BranchTransforms[i].AddComponent<BranchData>();
            BezierCurve spline = BranchTransforms[i].AddComponent<BezierCurve>();
            InitBranchValues(i, _BD);
            InitLineRenderer(line);
            InitBranchSpline(i, line, _BD, spline);
        }
        for (int i = branchNum - 1; i > -1; i--)
        {
            BranchData _BD = BranchTransforms[i].GetComponent<BranchData>();
            InitBranchRot(i, _BD);
        }
    }

    private void AddFractals(BranchData _BD)
    {
        GameObject fractal = Resources.Load("FractalObj") as GameObject;
        Instantiate(fractal, ReturnBranchParent(_BD).GetComponent<BezierCurve>().GetPoint(0.98f), Quaternion.identity);
    }

    private void InitLineRenderer(LineRenderer line)
    {
        int tiers = tierCount.Length;

        line.GetComponent<Renderer>().enabled = true;
        line.useWorldSpace = false;
        line.SetPosition(1, new Vector3(0, 1, 0));
        line.material = Resources.Load("bark") as Material;
    }

    private void InitBranchValues(int globalID, BranchData _BD)
    {
        if (globalID < tierCount[0])
        {
            _BD.Hierachy = 0;
            _BD.GlobalID = globalID; //Set global ID
            _BD.GroupID = globalID;
        }
        else if (globalID < tierCount[1] + tierCount[0])
        {
            _BD.Hierachy = 1;
            _BD.GlobalID = globalID; //Set global ID
            _BD.GroupID = globalID - (tierCount[0]); //Set Group ID
        }
        else if (globalID < tierCount[2] + tierCount[1] + tierCount[0])
        {
            _BD.Hierachy = 2;
            _BD.GlobalID = globalID; //Set global ID
            _BD.GroupID = globalID - (tierCount[1] + 1); //Set Group ID
        }
        else if (globalID < tierCount[3] + tierCount[2] + tierCount[1] + tierCount[0])
        {
            _BD.Hierachy = 3;
            _BD.GlobalID = globalID; //Set global ID
            _BD.GroupID = globalID - (tierCount[2] + 1); //Set Group ID
        }
    }

    private void InitBranchRot(int globalID, BranchData _BD)
    {
        //need to iterate in reverse to avoid affecting children
        switch (_BD.Hierachy)
        {
            case 0:
                newRot = Vector3.zero;
                break;

            case 1:
                newRot.y += 360.0f / tierCount[1]; //IN A CIRCLE
                newRot.x = -55.0f;
                break;

            case 2:
                newRot.y += 360.0f / (tierCount[2] / tierCount[1]); //IN A CIRCLE
                newRot.x = -55.0f;
                break;

            case 3:
                newRot.y += 360.0f / (tierCount[3] / tierCount[2]); //IN A CIRCLE
                newRot.x = -55.0f;
                break;

            default:
                break;
        }
        BranchTransforms[globalID].transform.SetParent(ReturnBranchParent(_BD).transform);
        BranchTransforms[globalID].transform.Rotate(newRot);
    }

    private GameObject ReturnBranchParent(BranchData _BD)
    {
        if (_BD.Hierachy > 0)
            _BD.ParentGroupID = (_BD.GroupID / (tierCount[_BD.Hierachy] / tierCount[_BD.Hierachy - 1]));

        foreach (GameObject otherBranch in BranchTransforms)
        {
            BranchData _OtherBD = otherBranch.GetComponent<BranchData>();
            if (_OtherBD.Hierachy == _BD.Hierachy - 1 &&    //if Hierachy is one up ^
                _OtherBD.GroupID == _BD.ParentGroupID)  //and others group ID == parent groupID
            {
                return _OtherBD.gameObject;
            }
        }
        print("null parent");
        return gameObject;
    }

    private void InitBranchSpline(int globalID, LineRenderer line, BranchData _BD, BezierCurve spline)
    {
        if (_BD.Hierachy < 1) //TRUNK
        {
            line.positionCount = 16; //2 Curves long
            line.startWidth = thickness * 1.1f; //Following Darwin's ratio
            line.endWidth = thickness * 0.4f;
            spline.DrawSpline(transform.position, line.positionCount);
            SetLineToSpline(line, spline);
            BranchTransforms[globalID].transform.position
                = Vector3.zero;
        }
        else
        {
            if (tierProgress[_BD.Hierachy] > 1) tierProgress[_BD.Hierachy] = 0;
            line.positionCount = 16 / _BD.Hierachy;
            tierProgress[_BD.Hierachy] += 1.0f / ((float)tierCount[_BD.Hierachy] / (float)tierCount[_BD.Hierachy - 1]);
            line.startWidth = (thickness / tierCount[_BD.Hierachy]) * 1.1f;
            line.endWidth = (thickness / tierCount[_BD.Hierachy]) * 0.5f;
            spline.DrawSpline(transform.position, line.positionCount);
            SetLineToSpline(line, spline);
            BranchTransforms[globalID].transform.position
            = ReturnBranchParent(_BD).GetComponent<BezierCurve>().GetPoint(tierProgress[_BD.Hierachy]);
        }
    }

    private void SetLineToSpline(LineRenderer line, BezierCurve spline)
    {
        if (line.positionCount >= 4)
        {
            //Internal Calculations
            Vector3 point = spline.GetPoint(0f);
            line.SetPosition(0, point);
            int steps = stepsPerCurve * spline.CurveCount;
            line.positionCount = steps;
            line.SetPosition(0, spline.GetPoint(0));
            for (int x = 1; x < steps - 1; x++)
            {
                point = spline.GetPoint((float)x / (float)steps);
                //this is the curves within line rednerer
                line.SetPosition(x, point);
            }
            line.SetPosition(line.positionCount - 1, spline.GetPoint(1));
        }
        else
        {
            print("more nodes needed");
            return;
        }
    }
}
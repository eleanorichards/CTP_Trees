using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBranches : MonoBehaviour
{
    private DrawBranches drawBranches;
    private FractalGen fractalGen;
    private List<GameObject> BranchTransforms = new List<GameObject>();

    [Tooltip("Should be multiple of predecessor")]
    public int[] tierCount;

    private int branchNum;
    private int stepsPerCurve = 5; //quality of curves
    private float thickness = 1.0f; //Reverse direction of hierachy

    private float tier1Progress = 0.0f;
    private float tier2Progress = 0.0f;
    private float tier3Progress = 0.0f;

    // Use this for initialization
    private void Start()
    {
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
            InitBranchPos(i, _BD);
            InitLineRenderer(line);
            InitBranchSpline(i, line, _BD, spline);
        }
    }

    private void InitLineRenderer(LineRenderer line)
    {
        int tiers = tierCount.Length;

        line.GetComponent<Renderer>().enabled = true;
        line.useWorldSpace = false;
        line.SetPosition(1, new Vector3(0, 1, 0));
        line.startWidth = 0.1f;
        line.endWidth = 0.08f;
        //line.startWidth = _depth * 0.08f;
        //line.endWidth = _depth * 0.06f;
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

    private void InitBranchPos(int globalID, BranchData _BD)
    {
        BranchTransforms[globalID].transform.Rotate(globalID * 1.5f, 0, 0);
        BranchTransforms[globalID].transform.SetParent(ReturnBranchParent(_BD).transform);
    }

    private GameObject ReturnBranchParent(BranchData _BD)
    {
        if (_BD.Hierachy > 0)
            _BD.ParentGroupID = _BD.GroupID / (tierCount[_BD.Hierachy] / tierCount[_BD.Hierachy - 1]);

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
        switch (_BD.Hierachy)
        {
            case 0:
                line.positionCount = 16; //2 Curves long
                line.startWidth = thickness * 1.1f; //Following Darwin's ratio
                line.endWidth = thickness * 0.8f;
                spline.DrawSpline(transform.position, line.positionCount);
                SetLineToSpline(line, spline);
                BranchTransforms[globalID].transform.position
                    = Vector3.zero;
                break;

            case 1:
                if (tier1Progress > 1.0f) tier1Progress = 0;
                tier1Progress += 1.0f / ((float)tierCount[1] / (float)tierCount[0]);
                line.positionCount = 12; //1 curve long
                line.startWidth = (thickness / tierCount[1]) * 1.1f;
                line.endWidth = (thickness / tierCount[1]) * 0.8f;
                spline.DrawSpline(transform.position, line.positionCount);
                SetLineToSpline(line, spline);
                BranchTransforms[globalID].transform.position
    = ReturnBranchParent(_BD).GetComponent<BezierCurve>().GetPoint(tier1Progress);

                break;

            case 2:
                if (tier2Progress > 1.0f) tier2Progress = 0;
                tier2Progress += 1.0f / ((float)tierCount[2] / (float)tierCount[1]);
                line.positionCount = 8; // 1 Curve long
                line.startWidth = (thickness / tierCount[2]) * 1.1f;
                line.endWidth = (thickness / tierCount[2]) * 0.8f;
                spline.DrawSpline(transform.position, line.positionCount);
                SetLineToSpline(line, spline);
                BranchTransforms[globalID].transform.position
                    = ReturnBranchParent(_BD).GetComponent<BezierCurve>().GetPoint(tier2Progress);

                break;

            case 3:
                if (tier3Progress > 1.0f) tier3Progress = 0;
                tier3Progress += 1.0f / ((float)tierCount[3] / (float)tierCount[2]);
                line.positionCount = 4;
                line.startWidth = (thickness / tierCount[3]) * 1.1f;
                line.endWidth = (thickness / tierCount[3]) * 0.8f;
                spline.DrawSpline(transform.position, line.positionCount);
                SetLineToSpline(line, spline);
                BranchTransforms[globalID].transform.position
                    = ReturnBranchParent(_BD).GetComponent<BezierCurve>().GetPoint(tier3Progress);

                break;

            default:
                break;
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
            for (int x = 1; x < steps; x++)
            {
                point = spline.GetPoint(x / (float)steps);
                //this is the curves within line rednerer
                line.SetPosition(x, point + spline.GetDirection(x / (float)steps));
            }
        }
        else
        {
            print("more nodes needed");
            return;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBranches : MonoBehaviour
{
    private DrawBranches drawBranch;
    private GameData _GD;
    private List<GameObject> BranchTransforms = new List<GameObject>();
    private List<GameObject> fractalList = new List<GameObject>();

    [Tooltip("Should be multiple of predecessor")]
    public int[] tierCount;

    private float[] tierProgress = new float[10];

    private int branchNum;
    private int stepsPerCurve = 5; //quality of curves
    private float thickness = 1.0f; //Reverse direction of hierachy

    private Vector3 newRot = Vector3.zero;

    // Use this for initialization
    private void Start()
    {
        _GD = GameObject.Find("CONTROLLER").GetComponent<GameData>();
        drawBranch = GetComponent<DrawBranches>();
        BuildTree();
    }

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
        fractalList.Clear();

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
            if (i > (branchNum - tierCount[tierCount.Length - 1]))
                fractalList.Add(drawBranch.AddFractals(BranchTransforms[i], BranchTransforms[i].GetComponent<BezierCurve>()));
        }

        //rotationPass - in reverse to avoid parent's effects
        for (int i = branchNum - 1; i > 0; i--)
        {
            BranchData _BD = BranchTransforms[i].GetComponent<BranchData>();
            InitBranchRot(i, _BD);
        }
        for (int i = branchNum - tierCount[tierCount.Length - 1] - 1; i < branchNum - 1; i++)
        {
            //Fractal pass
            RotateFractals(BranchTransforms[i], i);
        }
    }

    private void RotateFractals(GameObject _parent, int index)
    {
        foreach (GameObject fractal in fractalList)
        {
            // fractal.transform.SetParent(BranchTransforms[index].transform);
            //fractal.transform.position = _parent.GetPoint(1);
            fractal.transform.Rotate(fractal.transform.parent.transform.rotation.eulerAngles);
        }
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
            _BD.GroupID = globalID - (tierCount[1] + tierCount[0]); //Set Group ID
        }
        else if (globalID < tierCount[3] + tierCount[2] + tierCount[1] + tierCount[0])
        {
            _BD.Hierachy = 3;
            _BD.GlobalID = globalID; //Set global ID
            _BD.GroupID = globalID - (tierCount[2] + tierCount[1] + tierCount[0]); //Set Group ID
        }
    }

    /// <summary>
    ///         N[270]
    ///
    ///     E[180]       W[0]
    ///
    ///         S[90]
    /// </summary>
    private void InitBranchRot(int globalID, BranchData _BD)
    {
        //First pass for default pos
        newRot = SetRotationWeighting(newRot, _BD);

        BranchTransforms[globalID].transform.SetParent(ReturnBranchParent(_BD).transform);
        BranchTransforms[globalID].transform.Rotate(newRot);
    }

    private Vector3 SetRotationWeighting(Vector3 oldRot, BranchData _BD)
    {
        Vector3 newRot = oldRot;
        Vector3 v1 = drawBranch.InitBranchDefaults(newRot, _BD);
        Vector3 v2 = drawBranch.WindHeadingRot(newRot, _BD);
        Vector3 v3 = drawBranch.SunDirectionRot(newRot, _BD);

        newRot += (v3 + v2 + v1) * 0.01f; //etc ect
        return newRot;
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
        if (tierProgress[_BD.Hierachy] > 1) //If branch
        {
            tierProgress[_BD.Hierachy] = 0;
        }

        line.positionCount = 16 / (_BD.Hierachy + 1); //Cannot divide by 0
        line.startWidth = (thickness / tierCount[_BD.Hierachy]) * 1.1f;
        line.endWidth = (thickness / tierCount[_BD.Hierachy]) * 0.5f;
        drawBranch.DrawSplineBranches(spline, _BD);
        //spline.DrawSpline(transform.position, line.positionCount, _BD);
        SetLineToSpline(line, spline);
        if (_BD.Hierachy > 0) //TRUNK
        {
            tierProgress[_BD.Hierachy] += 1.0f / ((float)tierCount[_BD.Hierachy] / (float)tierCount[_BD.Hierachy - 1]);
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
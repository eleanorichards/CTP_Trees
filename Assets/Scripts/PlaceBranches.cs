using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBranches : MonoBehaviour
{
    private DrawBranches drawBranch;
    public GameData _GD;
    private List<GameObject> BranchTransforms = new List<GameObject>();
    private List<GameObject> fractalList = new List<GameObject>();

    [Tooltip("Should be multiple of predecessor")]
    public int[] tierCount;

    private float[] tierProgress = new float[10];

    private int branchNum; //density!
    private int stepsPerCurve = 5; //quality of curves
    private float thickness = 0.8f; //Reverse direction of hierachy

    private Vector3 newRot = Vector3.zero;
    private TubeRenderer tubeRend;
    public bool editor = true;

    // Use this for initialization
    private void Start()
    {
        // drawBranch = GetComponent<DrawBranches>();
        // BuildTree();
        //tubeRend = GameObject.Find("TubeRenderer").GetComponent<TubeRenderer>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
             BuildTree();
        }
    }

    public void BuildTree()
    {
        //print(transform.position);
        drawBranch = GetComponent<DrawBranches>();
        drawBranch.height = 50.0f;
        //_GD.copy
        if (!editor)
            _GD = GameObject.Find("CONTROLLER").GetComponent<GameData>();
        else
        {
            //_GD = GetComponentInParent<GameData>();
            //drawBranch._GD = _GD;
        }

        foreach (GameObject oldBranch in BranchTransforms)
        {
            Destroy(oldBranch);
        }
        BranchTransforms.Clear();
        fractalList.Clear();

        newRot = Vector3.zero;
        branchNum = 0;

        for (int i = 0; i < tierCount.Length; i++)
        {
            tierProgress[i] = 0;
            branchNum += tierCount[i];
        }

        thickness = drawBranch.GetTreeHeight() / 15.0f;
        _GD._tangliness = 0.3f;
        drawBranch.SetTreeTangliness();
        drawBranch.GetTreeHeight();
        for (int i = 0; i < branchNum; i++)
        {
            GameObject branch = new GameObject("branch" + i);
            BranchTransforms.Add(branch);
            BranchTransforms[i].transform.SetParent(this.transform);
            LineRenderer line = BranchTransforms[i].AddComponent<LineRenderer>();
            BranchData _BD = BranchTransforms[i].AddComponent<BranchData>();
            BezierCurve spline = BranchTransforms[i].AddComponent<BezierCurve>();
            InitBranchValues(i, _BD);
            InitLineRenderer(line);
            InitBranchSpline(i, line, _BD, spline);
            if (_GD.leaves)
                if (i > (branchNum - tierCount[tierCount.Length - 1]))
                    // fractalList.Add(drawBranch.AddFractals(BranchTransforms[i], BranchTransforms[i].GetComponent<BezierCurve>()));
                    fractalList.Add(drawBranch.AddLeaves(BranchTransforms[i], BranchTransforms[i].GetComponent<BezierCurve>()));
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
            //RotateFractals(BranchTransforms[i], i);
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
        Vector3 initialRot = drawBranch.InitBranchDefaults(newRot, _BD);

        Vector3 v2 = drawBranch.WindHeadingRot(initialRot, _BD);
        Vector3 v3 = drawBranch.SunHeadingRot(initialRot, _BD);

        newRot = initialRot + v2 + v3; //etc ect
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
        line.positionCount = 16 / (_BD.Hierachy + 1); //Cannot divide by 0
        line.startWidth = SetLineThickness(_BD);
        line.endWidth = SetLineThickness(_BD) * 0.5f; //set end thickness to lower to 'point' branch
        drawBranch.DrawSplineBranches(spline, _BD);
        SetLineToSpline(line, spline);
        if (_BD.Hierachy > 0) //TRUNK
        {
            tierProgress[_BD.Hierachy] += (1.0f / ((float)tierCount[_BD.Hierachy] / (float)tierCount[_BD.Hierachy - 1]));
            if (tierProgress[_BD.Hierachy] > 1) //If branch
            {
                tierProgress[_BD.Hierachy] = 0;
            }
            BranchTransforms[globalID].transform.position
        = ReturnBranchParent(_BD).GetComponent<BezierCurve>().GetPoint(tierProgress[_BD.Hierachy]);
        }
    }

    private float SetLineThickness(BranchData _BD)
    {
        float lineWidth = thickness;
        switch (_BD.Hierachy)
        {
            case 0:
                break;

            case 1:
                lineWidth = thickness * 0.6f;
                break;

            case 2:
                lineWidth = thickness * 0.2f;
                break;

            case 3:
                lineWidth = thickness * 0.1f;
                break;

            default:
                break;
        }
        return lineWidth;
    }

    private void SetLineToSpline(LineRenderer line, BezierCurve spline)
    {
        int steps = stepsPerCurve * spline.CurveCount;
        Vector3[] points = new Vector3[steps];

        if (line.positionCount >= 4)
        {
            //Internal Calculations
            Vector3 point = spline.GetPoint(0f);
            points[0] = point;
            line.SetPosition(0, point);
            line.positionCount = steps;
            line.SetPosition(0, spline.GetPoint(0));
            for (int x = 1; x < steps - 1; x++) //EACH STEP OF LINE RENDERER
            {
                point = spline.GetPoint((float)x / (float)steps);
                //tubeRend.vertices.SetValue()
                points[x] = point;
                line.SetPosition(x, point);
            }
            line.SetPosition(steps - 1, spline.GetPoint(1));
        }
        else
        {
            print("more nodes needed");
            return;
        }
        //rendering cylinder
        /*
        points[steps - 1] = spline.GetPoint(1);
        tubeRend.SetVertices(points, points.Length, 1.0f);
        tubeRend.RenderMesh();
        */
    }
}
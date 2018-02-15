using UnityEngine;

public class CurveControl : MonoBehaviour
{
    public BezierCurve[] splines;
    public GameObject branchPrefab = null;
    public GameObject[] curves;
    public LineRenderer[] lineRend;

    private const int branchCount = 41;  //No. of branches per tree (including trunk)
    private const int lineSteps = 10;   //no of steps
    private const int stepsPerCurve = 10;

    private int group1Count = 0; //Trunk n(ideally 1/2/3)
    private int group2Count = 0; //FORMAT: 8 each, 1 prev branch = 1*8 = 8
    private int group3Count = 0; //5 each, 8 prev branches = 5*8 = 40

    private int group2PerBranch = 0; // no. of branches per prev. branch (8)
    private int group3PerBranch = 0; //(5)

    private float angleVariation = 90.0f;
    private float branchStep = 0.0f;
    private float angle = 0;

    private GameData _GD;

    // Use this for initialization
    private void Start()
    {
        //GAMEDATA
        _GD = GetComponent<GameData>();

        //INITIALISE BRANCH PARTS
        curves = new GameObject[branchCount];
        splines = new BezierCurve[branchCount];
        lineRend = new LineRenderer[branchCount];
        branchPrefab = Resources.Load<GameObject>("Bezier");

        //Initialising Data
        int group1Count = 1; //one trunk
        int group2Count = group1Count * group2PerBranch; //8 coming off the trunk
        int group3Count = group2Count * group3PerBranch; //5 each / 40 coming off tier 2

        CreateSplines();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    /// <summary>
    /// Initialisation of Splines
    /// </summary>
    private void CreateSplines()
    {
        for (int i = 0; i < branchCount; i++)
        {
            curves[i] = Instantiate(branchPrefab, transform);
            splines[i] = curves[i].GetComponent<BezierCurve>();
            lineRend[i] = curves[i].GetComponent<LineRenderer>();
            lineRend[i].positionCount = stepsPerCurve;
            lineRend[i].startColor = Color.black;
            lineRend[i].endColor = Color.grey;
        }
    }

    /// <summary>
    /// Sets index numbers and parent branch
    /// </summary>
    public void SetBranchType()
    {
        switch (_GD._treeType)
        {
            case TreeType.DICHOTOMOUS:
                angleVariation = 180;
                group2PerBranch = 8;
                group3PerBranch = 8;
                group1Count = 1; //one trunk
                group2Count = group1Count * group2PerBranch; //8 coming off the trunk
                group3Count = group2Count * group3PerBranch; //5 each / 40 coming off tier 2

                for (int i = 1; i < splines.Length; i++)
                {
                    //ANGLE stuff
                    angle += angleVariation;
                    if (angle >= 360)
                        angle = 0;

                    splines[i].SetGlobalIndex(i);
                    //TRUNK
                    if (i <= group1Count)
                    {
                        lineRend[i].startWidth = 0.5f;
                        lineRend[i].endWidth = 0.2f;
                        splines[i].SetInitialStatus(0, 0);
                        SetStartLocation(0, 1, i, 0);
                    }
                    //TIER 1
                    if (i <= group2Count && i > group1Count)
                    {
                        lineRend[i].startWidth = 0.3f;
                        lineRend[i].endWidth = 0.1f;
                        splines[i].SetInitialStatus(1, i - group1Count);
                        branchStep += ((float)1 / (float)group2PerBranch);
                        SetStartLocation(group1Count, group2PerBranch, i, 1);
                    }
                    //TIER 2
                    if (i <= group3Count && i > group2Count)
                    {
                        lineRend[i].startWidth = 0.1f;
                        lineRend[i].endWidth = 0.00f;
                        splines[i].SetInitialStatus(2, i - group2Count);
                        branchStep += ((float)1 / (float)group3PerBranch);
                        SetStartLocation(group2Count, group3PerBranch, i, 2);
                    }
                    //draw lines from points
                    DrawLines(i);
                }
                break;

            case TreeType.SYMPODIAL:
                angleVariation = 137.5f;
                group2PerBranch = 8;
                group3PerBranch = 5;
                group1Count = 1; //two trunk
                group2Count = group1Count * group2PerBranch; //8 coming off the trunk
                group3Count = group2Count * group3PerBranch; //5 each / 40 coming off tier 2

                for (int i = 0; i < splines.Length; i++)
                {
                    //ANGLE stuff
                    angle += angleVariation;
                    if (angle >= 360)
                        angle = 0;

                    splines[i].SetGlobalIndex(i);
                    //TRUNK
                    if (i < group1Count)
                    {
                        lineRend[i].startWidth = 0.5f;
                        lineRend[i].endWidth = 0.2f;
                        splines[i].SetInitialStatus(0, 0);
                        SetStartLocation(0, 1, i, 0);
                    }
                    //TIER 1
                    if (i < group2Count && i >= group1Count)
                    {
                        lineRend[i].startWidth = 0.3f;
                        lineRend[i].endWidth = 0.1f;
                        splines[i].SetInitialStatus(1, i - group1Count);
                        branchStep += ((float)1 / (float)group2PerBranch);
                        SetStartLocation(group1Count, group2PerBranch, i, 1);
                    }
                    //TIER 2
                    if (i < group3Count && i >= group2Count)
                    {
                        lineRend[i].startWidth = 0.1f;
                        lineRend[i].endWidth = 0.00f;
                        splines[i].SetInitialStatus(2, i - group2Count);
                        branchStep += ((float)1 / (float)group3PerBranch);
                        SetStartLocation(group2Count, group3PerBranch, i, 2);
                    }
                    //draw lines from points
                    DrawLines(i);
                }
                break;

            case TreeType.MONOPODIAL:
                angleVariation = 90;
                group2PerBranch = 8;
                group3PerBranch = 5;
                group1Count = 1; //one trunk
                group2Count = group1Count * group2PerBranch; //8 coming off the trunk
                group3Count = group2Count * group3PerBranch; //5 each / 40 coming off tier 2

                for (int i = 0; i < splines.Length; i++)
                {
                    //ANGLE stuff
                    angle += angleVariation;
                    if (angle > 1)
                        angle = 0;

                    splines[i].SetGlobalIndex(i);
                    //TRUNK
                    if (i < group1Count)
                    {
                        lineRend[i].startWidth = 0.5f;
                        lineRend[i].endWidth = 0.2f;
                        splines[i].SetInitialStatus(0, 0);
                        SetStartLocation(0, 1, i, 0);
                    }
                    //TIER 1
                    if (i <= group2Count && i >= group1Count)
                    {
                        angleVariation = 1.0f / group2Count;
                        lineRend[i].startWidth = 0.25f;
                        lineRend[i].endWidth = 0.1f;
                        splines[i].SetInitialStatus(1, i - group1Count);
                        branchStep += ((float)1 / (float)group2PerBranch);
                        SetStartLocation(group1Count, group2PerBranch, i, 1);
                    }
                    //TIER 2
                    if (i <= group3Count && i > group2Count)
                    {
                        angleVariation = 1.0f / group3Count;
                        lineRend[i].startWidth = 0.1f;
                        lineRend[i].endWidth = 0.00f;
                        splines[i].SetInitialStatus(2, i - group2Count);
                        branchStep += ((float)1 / (float)group3PerBranch);
                        SetStartLocation(group2Count, group3PerBranch, i, 2);
                    }
                    //draw lines from points
                    DrawLines(i);
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// sets start node pos
    /// </summary>
    private void SetStartLocation(int _noOfParents, int _groupBranchCount, int _splineNo, int hierachy)
    {
        int memberIndex = _splineNo - _noOfParents;
        int noOfParents = _noOfParents;
        int parentMemberIndex = (int)(memberIndex / _groupBranchCount);
        Vector3 startPos = Vector3.zero;
        splines[_splineNo].SetParentIndex(parentMemberIndex);

        //globalIndex = MemberIndex + noOfParents
        for (int i = 0; i < splines.Length; i++)
        {
            //If splines[i] == parent Spline
            if (splines[i].hierachyIndex == (splines[_splineNo].hierachyIndex - 1)
                && splines[i].memberIndex == parentMemberIndex)
            {
                if (branchStep >= 1.0f)
                {
                    branchStep = 0.3f;
                }

                startPos = splines[i].GetPoint(branchStep);
            }
            if (_splineNo == 0)
            {
                splines[i].SetAllNodes(Vector3.zero, new Vector3(0.0f, 1.0f, 0.0f));
            }
        }

        //the important line
        splines[_splineNo].SetAllNodes(startPos, circleParentBranch(startPos, angle, hierachy));
    }

    /// <summary>
    /// set the first node of each branch in a circular radius around the current parent
    /// node[1] position = parent branch + small amount (add amount
    /// </summary>
    public Vector3 circleParentBranch(Vector3 parentpos, float angle, int hierachy)
    {
        float radius = 0.0f;
        Vector3 pos = Vector3.zero;

        switch (hierachy)
        {
            case 0:
                break;

            case 1:
                radius = 1f;
                pos.x = parentpos.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
                pos.y = parentpos.y;
                pos.z = parentpos.z + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
                break;

            case 2:
                radius = 0.1f;
                pos.x = parentpos.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
                pos.y = parentpos.y + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
                pos.z = parentpos.z;
                break;

            default:
                break;
        }

        return pos;
    }

    /// <summary>
    /// Set points for bezier curve,
    /// Place linerenderer steps along curve
    /// </summary>
    private void DrawLines(int i)
    {
        //Internal Calculations
        Vector3 point = splines[i].GetPoint(0f);
        lineRend[i].SetPosition(0, point);
        int steps = stepsPerCurve * splines[i].CurveCount;
        for (int x = 1; x < steps; x++)
        {
            point = splines[i].GetPoint(x / (float)steps);
            //this is the curves within line rednerer
            lineRend[i].SetPosition(x, point + splines[i].GetDirection(x / (float)steps));
        }
    }

    /// <summary>
    /// Place shapes along spline at increments
    /// </summary>
    public void ExtrudeSpline()
    {
        foreach (BezierCurve spline in splines)
        {
            spline.ExtrudeShape();
        }
    }
}
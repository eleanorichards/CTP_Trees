using UnityEngine;

public class CurveControl : MonoBehaviour
{
    public BezierCurve[] splines;
    public GameObject branchPrefab = null;
    public GameObject[] curves;
    public LineRenderer[] lineRend;

    public const int branchCount = 41;  //No. of branches per tree (including trunk)
    private const int lineSteps = 10;   //no of steps
    private const int stepsPerCurve = 10;
    private float angleVariation = 90.0f;

    private int group1Count = 0; //Trunk n(ideally 1/2/3)
    private int group2Count = 0; //FORMAT: 8 each, 1 prev branch = 1*8 = 8
    private int group3Count = 0; //5 each, 8 prev branches = 5*8 = 40

    private int group2PerBranch = 0; // no. of branches per prev. branch (8)
    private int group3PerBranch = 0; //(5)

    private TreeType treeType;
    private float y = 0.0f;
    float angle = 0;

    // Use this for initialization
    private void Start()
    {
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
        switch (treeType)
        {
            case TreeType.DISSECANT:
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
                    if (angle >= 360)
                        angle = 0;

                    splines[i].SetGlobalIndex(i);
                    if (i < group1Count)
                    {
                        angleVariation = 90;

                        lineRend[i].startWidth = 0.5f;
                        lineRend[i].endWidth = 0.4f;
                        splines[i].SetInitialStatus(0, 0);
                        SetStartLocation(0, 1, i, 0);
                    }
                    if (i <= group2Count && i >= group1Count)
                    {
                        lineRend[i].startWidth = 0.3f;
                        lineRend[i].endWidth = 0.2f;
                        splines[i].SetInitialStatus(1, i - group1Count);
                        SetStartLocation(group1Count, group2PerBranch, i, 1);
                        y += ((float)1 / (float)group2PerBranch);
                    }
                    if (i <= group3Count && i > group2Count)
                    {
                        angleVariation = 90;

                        lineRend[i].startWidth = 0.1f;
                        lineRend[i].endWidth = 0.05f;
                        splines[i].SetInitialStatus(2, i - group2Count);
                        SetStartLocation(group2Count, group3PerBranch, i, 2);
                        y += ((float)1 / (float)group3PerBranch);
                    }
                    //So on...
                    DrawLines(i);
                }
                break;

            case TreeType.FIBBONACI:
                angleVariation = 37.5f;
                break;

            case TreeType.DROOPY:
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
                if (y <= 1)
                {
                    startPos = splines[i].GetPoint(y);
                }
                else
                {
                    y = 0;
                    startPos = splines[i].GetPoint(y);
                }
            }
            if (_splineNo == 0)
            {
                splines[i].SetAllNodes(Vector3.zero, new Vector3(0.0f,1.0f,0.0f));
            }

            //the important line        
        }
        splines[_splineNo].SetAllNodes(startPos , circleParentBranch(startPos, 1.0f, angle, hierachy));              
    }

    public Vector3 circleParentBranch(Vector3 parentpos, float radius, float angle, int hierachy)
    {
        Debug.Log(angle + "(angle!)");
        //float ang = 360 / numOfChildren;
        Vector3 pos =  Vector3.zero;
        switch (hierachy)
        {
            case 0:
                break;
            case 1:
                pos.x = parentpos.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
                pos.y = parentpos.y;
                pos.z = parentpos.z + radius * Mathf.Cos(angle * Mathf.Deg2Rad); 
                break;
            case 2:
                pos.x = parentpos.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
                pos.y = parentpos.y + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
                pos.z = parentpos.z ;
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
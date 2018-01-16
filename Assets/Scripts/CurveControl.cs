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

    private int group1Count = 0; //Trunk n(ideally 1/2/3)
    private int group2Count = 0; //FORMAT: 8 each, 1 prev branch = 1*8 = 8
    private int group3Count = 0; //5 each, 8 prev branches = 5*8 = 40

    private int group2PerBranch = 0; // no. of branches per prev. branch (8)
    private int group3PerBranch = 0; //(5)

    private TreeType treeType;
    private float y = 0.0f;

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
                group2PerBranch = 8;
                group3PerBranch = 5;
                group1Count = 1; //one trunk
                group2Count = group1Count * group2PerBranch; //8 coming off the trunk
                group3Count = group2Count * group3PerBranch; //5 each / 40 coming off tier 2

                for (int i = 0; i < splines.Length; i++)
                {
                    splines[i].SetGlobalIndex(i);
                    if (i < group1Count)
                    {
                        lineRend[i].startWidth = 0.5f;
                        lineRend[i].endWidth = 0.4f;
                        splines[i].SetInitialStatus(0, 0);
                        SetStartLocation(0, 1, i);
                    }
                    if (i <= group2Count && i > group1Count)
                    {
                        lineRend[i].startWidth = 0.3f;
                        lineRend[i].endWidth = 0.2f;
                        splines[i].SetInitialStatus(1, i - group1Count);
                        SetStartLocation(group1Count, group2PerBranch, i);
                        y += ((float)1 / (float)group2PerBranch);
                    }
                    if (i <= group3Count && i > group2Count)
                    {
                        lineRend[i].startWidth = 0.1f;
                        lineRend[i].endWidth = 0.05f;
                        splines[i].SetInitialStatus(2, i - group2Count);
                        SetStartLocation(group2Count, group3PerBranch, i);
                        y += ((float)1 / (float)group3PerBranch);
                    }
                    //So on...
                    DrawLines(i);
                }
                break;

            case TreeType.FIBBONACI:
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
    private void SetStartLocation(int _noOfParents, int _groupBranchCount, int _splineNo)
    {
        float angle = 0;
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
            angle = _groupBranchCount / (_splineNo + 1);
        }
        splines[_splineNo].SetAllNodes(startPos , circleParentBranch(startPos, 1.0f, angle));              
    }

    public Vector3 circleParentBranch(Vector3 parentpos, float radius, float angle)
    {
        //float ang = 360 / numOfChildren;
        Vector3 pos;
        pos.x = parentpos.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        pos.y = parentpos.y;
        pos.z = parentpos.z + radius * Mathf.Cos(angle * Mathf.Deg2Rad); 
        return pos;
    }

    /// <summary>
    /// sets pos and orientation of all nodes
    /// </summary>
    private Vector3 GetNodePos(BezierCurve spline)
    {
        Vector3 newNode = Vector3.zero;
        
        float yAngle = 5.0f;

        float axisXLength = (spline.nodes[1].x
             - spline.nodes[0].x);
        float axisYLength = (spline.nodes[1].y
             - spline.nodes[0].y);
        float axisZLength = (spline.nodes[1].z
             - spline.nodes[0].z);

        float XYDistance = Mathf.Sqrt(Mathf.Pow(axisXLength, 2) + Mathf.Pow(axisYLength, 2));
        float YZDistance = Mathf.Sqrt(Mathf.Pow(axisYLength, 2) + Mathf.Pow(axisZLength, 2));
        float ZXDistance = Mathf.Sqrt(Mathf.Pow(axisZLength, 2) + Mathf.Pow(axisXLength, 2));

        //What we now want is this only for the 1st node
        //Then add on the distance between this and original branch
        //set this to new vec3
        float sideCLength = (ZXDistance * Mathf.Sin(yAngle))
            / Mathf.Sin((180 - yAngle) / 2);

        //these aren't returning numbers                                                      
        newNode.x = spline.nodes[0].x + axisXLength;
        newNode.y = spline.nodes[0].y + axisYLength;
        newNode.z = spline.nodes[0].z + axisZLength;

        return newNode;

       
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
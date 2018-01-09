using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveControl : MonoBehaviour
{
    public BezierCurve[] splines;
    public GameObject branchPrefab = null;
    public GameObject[] curves;
    public LineRenderer[] lineRend;

    private const int branchCount = 40;
    private const int lineSteps = 10;
    private const int stepsPerCurve = 10;

    TreeType treeType;
    float y = 0.0f;

    // Use this for initialization
    void Start () {
        //INITIALISE BRANCH PARTS
        curves = new GameObject[branchCount];
        splines = new BezierCurve[branchCount];
        lineRend = new LineRenderer[branchCount];
        branchPrefab = Resources.Load<GameObject>("Bezier");
        CreateSplines();
	}
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    
    /// <summary>
    /// Sets index numbers and parent branch
    /// </summary>
    public void SetBranchType()
    {
        int group2PerBranch = 8;
        int group3PerBranch = 5;

        switch(treeType)
        {
            case TreeType.DISSECANT:                            
                int group1Count = 1; //one trunk
                int group2Count = group1Count * group2PerBranch; //8 coming off the trunk
                int group3Count = group2Count * group3PerBranch; //5 each / 40 coming off tier 2
                

                for (int i = 0; i < splines.Length; i++)
                {
                    splines[i].SetGlobalIndex(i);
                    if(i < group1Count)
                    {
                        lineRend[i].startWidth = 0.5f;
                        lineRend[i].endWidth = 0.4f;
                        splines[i].SetInitialStatus(0, 0);
                        SetStartLocation(0, 1, i);
                    }
                    if(i < group2Count && i >= group1Count)
                    {
                        lineRend[i].startWidth = 0.3f;
                        lineRend[i].endWidth = 0.2f;
                        splines[i].SetInitialStatus(1, i - group1Count);
                        SetStartLocation(group1Count, group2PerBranch, i);
                        y += ((float)1 / (float)group2PerBranch);
                    }
                    if(i < group3Count && i >= group2Count)
                    {
                        lineRend[i].startWidth = 0.1f;
                        lineRend[i].endWidth = 0.05f;
                        splines[i].SetInitialStatus(2, i - group2Count);
                        SetStartLocation(group2Count, group3PerBranch, i);
                        y += (1 / group3PerBranch);
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

    void SetStartLocation(int _noOfParents, int _groupBranchCount, int _splineNo)
    {
        int memberIndex = _splineNo - _noOfParents;
        int noOfParents = _noOfParents;
        int parentMemberIndex = (int)(memberIndex / _groupBranchCount);
        splines[_splineNo].SetParentIndex(parentMemberIndex);

        //globalIndex = MemberIndex + noOfParents
        for (int i = 0; i < splines.Length; i++)
        {
            if (splines[i].hierachyIndex == (splines[_splineNo].hierachyIndex - 1)
                && splines[i].memberIndex == parentMemberIndex) 
            {
                if (y <= 1)
                {
                    Vector3 startPos = splines[i].GetPoint(y);
                    splines[_splineNo].nodes[0] = startPos;
                }
                else
                    y = 0;                
            }
            
        }
    }

    void DrawLines(int i)
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
    /// Initialisation of Splines
    /// </summary>
    void CreateSplines()
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

    public void ExtrudeSpline()
    {
        foreach (BezierCurve spline in splines)
        {
            spline.ExtrudeShape();
        }
    }
}

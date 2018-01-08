using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveControl : MonoBehaviour
{
    public BezierCurve[] splines;
    public GameObject branchPrefab = null;
    public GameObject[] curves;
    public LineRenderer[] lineRend;

    private const int branchCount = 100;
    private const int lineSteps = 10;
    private const int stepsPerCurve = 10;
    TreeType treeType;

    // Use this for initialization
    void Start () {
        //INITIALISE BRANCH PARTS
        curves = new GameObject[branchCount];
        splines = new BezierCurve[branchCount];
        lineRend = new LineRenderer[branchCount];
        branchPrefab = Resources.Load<GameObject>("Bezier");
        FindAllCurves();
	}
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    

    public void SetBranchType()
    {

        switch(treeType)
        {
            case TreeType.DISSECANT:
                //for each spline in the scene, set position
                //Fibonacci sequence would mean
                //1, 2, 3, 5, 8, 13, 21
                //Hierachy Calculations                   
                int partsInterval1 = 1; //1
                int partsInterval2 = 6*1; //37
                int partsInterval3 = 8*8 ; //101
                
                for (int i = 0; i < splines.Length; i++)
                {
                    if(i < partsInterval1)
                    {
                        splines[i].SetInitialStatus(0, 0);
                    }
                    if(i < partsInterval2 && i >= partsInterval1)
                    {
                        splines[i].SetInitialStatus(1, i - partsInterval1);                        
                    }
                    if(i < partsInterval3 && i >= partsInterval2)
                    {
                        splines[i].SetInitialStatus(2, i - partsInterval2 );
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
    void FindAllCurves()
    {
        // curves = GameObject.FindGameObjectsWithTag("BranchSpline");
        // splines = new BezierCurve[curves.Length];
        //splines = new BezierCurve[branchCount];
        //lineRend = new LineRenderer[branchCount];
        for (int i = 0; i < branchCount; i++)
        {
            curves[i] = Instantiate(branchPrefab, transform);
            //splines[i] = curves[i].GetComponent<BezierCurve>();
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

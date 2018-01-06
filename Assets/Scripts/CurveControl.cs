using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveControl : MonoBehaviour
{
    private BezierCurve[] splines = null;
    public GameObject[] curves = null;
    public LineRenderer[] lineRend = null;
    private const int lineSteps = 10;
    private const int stepsPerCurve = 10;
    private float directionScale = 0.5f;

    // Use this for initialization
    void Start () {
        FindAllCurves();
	}
	
	// Update is called once per frame
	void Update ()
    {
    }

    public void ExtrudeSpline()
    {
        foreach(BezierCurve spline in splines)
        {
            spline.ExtrudeShape();
        }
    }

    public void ShowDirections()
    {
        for (int i = 0; i < splines.Length; i++)
        {
            // Handles.color = Color.red;
            Vector3 point = splines[i].GetPoint(0f);
            lineRend[i].SetPosition(0, point);
            lineRend[i].SetPosition(1, point + splines[i].GetDirection(0f) * directionScale);
            //DrawLine(point, point + spline.GetDirection(0f) * directionScale);
            int steps = stepsPerCurve * splines[i].CurveCount;
            for (int x = 1; x < steps; x++)
            {
                point = splines[i].GetPoint(x / (float)steps);
                lineRend[i].SetPosition(x, point + splines[i].GetDirection(x / (float)steps) * directionScale);
                //Handles.DrawLine(point, point + spline.GetDirection(x / (float)steps) * directionScale);
            }
        }

    }

    void FindAllCurves()
    {
        curves = GameObject.FindGameObjectsWithTag("BranchSpline");
        splines = new BezierCurve[curves.Length];
        lineRend = new LineRenderer[curves.Length];
        for (int i = 0; i < curves.Length; i++)
        {
            splines[i] = curves[i].GetComponent<BezierCurve>();
            lineRend[i] = splines[i].GetComponent<LineRenderer>();
            lineRend[i].positionCount = stepsPerCurve;
        }
    }


}

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
        if (Input.GetButton("Fire1"))
        {
            FindAllCurves();
            //ExtrudeSpline();
            //somehow need to set these separately
            //foreach curve in curves

            for (int i = 0; i < splines.Length; i++)
            { 
            //draw base lines
                //for (int x = 0; x < splines[i].nodes.Length - 1; x++)
                //{
                //    //lineRend.
                //   // Handles.color = Color.green;
                //   // Handles.DrawLine(nodes[x], nodes[x + 1]);
                //}

                //Draw main lines
                Vector3 p0 = splines[i].nodes[0];
                for (int y = 1; y < splines[i].nodes.Length - 3; y += 3)
                {
                    //lineRend[i]
                    //Handles.color = Color.black;
                    Vector3 p1 = splines[i].nodes[y];
                    Vector3 p2 = splines[i].nodes[y + 1];
                    Vector3 p3 = splines[i].nodes[y + 2];

                  //  Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
                    p0 = p3;
                    ShowDirections(splines[i], i);
                }
            }
        }

    }

    public void ExtrudeSpline()
    {
        foreach(BezierCurve spline in splines)
        {
            spline.ExtrudeShape();
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

    private void ShowDirections(BezierCurve spline, int indexNumber)
    {
        // Handles.color = Color.red;
        Vector3 point = spline.GetPoint(0f);
        lineRend[indexNumber].SetPosition(0, point);
        lineRend[indexNumber].SetPosition(1, point + spline.GetDirection(0f) * directionScale);
        //DrawLine(point, point + spline.GetDirection(0f) * directionScale);
        int steps = stepsPerCurve * spline.CurveCount;
        for (int x = 1; x < steps; x++)
        {
            point = spline.GetPoint(x / (float)steps);
            lineRend[indexNumber].SetPosition(x, point + spline.GetDirection(x / (float)steps) * directionScale);
            //Handles.DrawLine(point, point + spline.GetDirection(x / (float)steps) * directionScale);
        }

    }

}

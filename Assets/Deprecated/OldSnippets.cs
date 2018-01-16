using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldSnippets : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //private Vector3 GetNodePos(BezierCurve spline)
    //{
    //    Vector3 newNode = Vector3.zero;

    //    float yAngle = 5.0f;

    //    float axisXLength = (spline.nodes[1].x
    //         - spline.nodes[0].x);
    //    float axisYLength = (spline.nodes[1].y
    //         - spline.nodes[0].y);
    //    float axisZLength = (spline.nodes[1].z
    //         - spline.nodes[0].z);

    //    float XYDistance = Mathf.Sqrt(Mathf.Pow(axisXLength, 2) + Mathf.Pow(axisYLength, 2));
    //    float YZDistance = Mathf.Sqrt(Mathf.Pow(axisYLength, 2) + Mathf.Pow(axisZLength, 2));
    //    float ZXDistance = Mathf.Sqrt(Mathf.Pow(axisZLength, 2) + Mathf.Pow(axisXLength, 2));

    //    //What we now want is this only for the 1st node
    //    //Then add on the distance between this and original branch
    //    //set this to new vec3
    //    float sideCLength = (ZXDistance * Mathf.Sin(yAngle))
    //        / Mathf.Sin((180 - yAngle) / 2);

    //    //these aren't returning numbers                                                      
    //    newNode.x = spline.nodes[0].x + axisXLength;
    //    newNode.y = spline.nodes[0].y + axisYLength;
    //    newNode.z = spline.nodes[0].z + axisZLength;

    //    return newNode;


    //}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class segmentPlacer : MonoBehaviour
{
    public BezierCurve spline;
    //public float progress;
    public bool lookForward = false;
    public int frequency = 0;
    public Transform[] segments;

	// Update is called once per frame

    public void PlaceShapes()
    {
        DestroyShapes();
        if (frequency <= 0 || segments == null || segments.Length == 0)
        {
            return;
        }

        float stepSize = 1f / (frequency * segments.Length);
        for (int f = 0; f < frequency; f++)
        {
            for (int i = 0; i < segments.Length; i++)
            {
                //int segLength = segments.Length;
                // System.Array.Resize(ref segments, segments.Length - segLength);
              
                Transform part = Instantiate(segments[i]) as Transform;
                part.transform.rotation = spline.GetOrientation3D((f * stepSize), Vector3.up);
                part.transform.localPosition = spline.GetPoint(f * stepSize); 
                part.transform.parent = transform;
            }
        }

    }

    void DestroyShapes()
    {

        GameObject[] temp = GameObject.FindGameObjectsWithTag("segment");
        foreach(GameObject part in temp)
        {
            DestroyImmediate(part);

        }
        
    }
}

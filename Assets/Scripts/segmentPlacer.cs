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
                Vector3 pos = spline.GetPoint(f * stepSize);
                
                Vector3 rot = part.transform.eulerAngles;
                rot.x += 90;
                part.transform.eulerAngles = rot;
                part.transform.localPosition = pos;
                part.transform.LookAt(pos + spline.GetDirection(f * stepSize));
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
  //      progress += Time.deltaTime * 0.1f;
		//if(progress > 1.0f)
  //      {
  //          progress = 0.0f;
  //      }
  //      Vector3 position = spline.GetPoint(progress);
  //      transform.localPosition = position;

  //      if (lookForward)
  //      {
  //          transform.LookAt(position + spline.GetDirection(progress));
  //      } 
    
}

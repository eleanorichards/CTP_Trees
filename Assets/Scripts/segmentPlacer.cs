using UnityEngine;

public class segmentPlacer : MonoBehaviour
{
    private BezierCurve spline;
    //private GameObject[] splines;

    //public float progress;
    public bool lookForward = false;

    public int frequency = 1;
    public Transform segments;

    // Update is called once per frame
    private void Start()
    {
        spline = gameObject.GetComponentInParent<BezierCurve>();
    }

    public void PlaceShapes()
    {
        DestroyShapes();
        if (frequency <= 0 || segments == null)
        {
            return;
        }

        float stepSize = 1f / (frequency);
        for (int f = 0; f < frequency; f++)
        {
            Transform part = Instantiate(segments) as Transform;
            part.transform.rotation = spline.GetOrientation3D((f * stepSize), Vector3.up);
            part.transform.localPosition = spline.GetPoint(f * stepSize);
            part.transform.parent = transform;
        }
    }

    private void DestroyShapes()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("segment");
        foreach (GameObject part in temp)
        {
            DestroyImmediate(part);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class splineWalker : MonoBehaviour {
    public BezierCurve spline;
    public float duration;
    public float progress;
    public bool lookForward = false;

	// Update is called once per frame
	void Update () {
        progress += Time.deltaTime * 0.1f;
		if(progress > 1.0f)
        {
            progress = 0.0f;
        }
        Vector3 position = spline.GetPoint(progress);
        transform.localPosition = position;

        if (lookForward)
        {
            transform.LookAt(position + spline.GetDirection(progress));
        }
    }
}

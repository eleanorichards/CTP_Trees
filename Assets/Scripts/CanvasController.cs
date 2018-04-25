using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public bool closing = false;
    public bool opening = false;

    private float smoothTime = 1.0f;

    private Vector3 refBoi = Vector3.zero;

    private RectTransform canvasRect;

    // Use this for initialization
    private void Start()
    {
        canvasRect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (closing)
        {
            canvasRect.position = Vector3.SmoothDamp(canvasRect.position, canvasRect.localPosition - (Vector3.up * 500), ref refBoi, smoothTime);
        }
    }

    public void CollapseCanvas()
    {
        closing = true;
    }

    public void ExpandCanvas()
    {
        this.GetComponent<Renderer>().enabled = true;
    }
}
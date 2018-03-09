using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBranches : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void drawLine(float x1, float y1, float x2, float y2, int _depth) // color variable is current depth, could be used for coloring different depths
    {
        // create gameObject for 1 branch
        GameObject branch = new GameObject("branch");

        // make this branch child of our main gameobject
        branch.transform.parent = gameObject.transform;

        // add line renderer to our gameobject
        LineRenderer line = branch.AddComponent<LineRenderer>();
        line.GetComponent<Renderer>().enabled = true;
        line.useWorldSpace = false;
        line.startWidth = _depth * 0.08f;
        line.endWidth = _depth * 0.06f;
        line.startColor = Color.black;
        line.endColor = Color.green;
        line.material = Resources.Load("bark") as Material;
        //draw the line: original script is 2D,  Z=0
        line.SetPosition(0, new Vector3(x1, y1, 0));
        line.SetPosition(1, new Vector3(x2, y2, 0));
    }
}
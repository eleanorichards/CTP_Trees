using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalGen : MonoBehaviour
{
    private float deg_to_rad = Mathf.PI / 180.0f;
    public int depth = 9; // gets slow over 15
    private float scale = 0.2f;

    private void Start()
    {
        // draw a fractal tree
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            drawTree(0 + transform.position.x, 0 + transform.position.y, 90, depth); // x, y, angle, depth
        }
    }

    // this function is pretty much straight from the original code
    private void drawTree(float x1, float y1, float angle, int _depth)
    {
        if (_depth != 0)
        {
            float x2 = x1 + (Mathf.Cos(angle * deg_to_rad) * _depth * scale);
            float y2 = y1 + (Mathf.Sin(angle * deg_to_rad) * _depth * scale);
            drawLine(x1, y1, x2, y2, _depth);
            drawTree(x2, y2, angle - 20, _depth - 1);
            drawTree(x2, y2, angle + 20, _depth - 1);
        }
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
        line.material = Resources.Load("bark") as Material;
        //draw the line: original script is 2D,  Z=0
        line.SetPosition(0, new Vector3(x1, y1, 0));
        line.SetPosition(1, new Vector3(x2, y2, 0));
    }
}
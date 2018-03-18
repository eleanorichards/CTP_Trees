﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBranches : MonoBehaviour
{
    private float deg_to_rad = Mathf.PI / 180.0f;
    public int depth = 9; // gets slow over 15
    private float scale = 0.2f;
    private GameData _GD;

    ///Height
    //Surrounded/solo
    //moisture
    //soil quality
    ///Density
    //height
    //surrounded/solo
    ///Rotation
    //Sun
    //wind
    ///Randomness
    //soil quality/density

    // Use this for initialization
    private void Start()
    {
        _GD = GameObject.Find("CONTROLLER").GetComponent<GameData>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    //Surrounded, Sun strength
    public void DrawSplineBranches(BezierCurve spline, BranchData _BD)
    {
        if (!_GD)
            _GD = GameObject.Find("CONTROLLER").GetComponent<GameData>();
        float height = 0;
        if (_GD.surrounded)
        {
            height = 20;
        }
        else
        {
            height = 30;
        }

        switch (_BD.Hierachy)
        {
            case 0:

                break;

            case 1:
                height = height / 2;
                break;

            case 2:
                height = height / 3;

                break;

            case 3:
                height = height / 4;

                break;

            default:
                break;
        }

        //SUNSTRENGTH percentage
        height *= _GD._sunStrength;
        int nodeNum = (int)height - ((int)height % 4);
        spline.DrawSpline(transform.position, nodeNum, _BD);
    }

    public Vector3 InitBranchDefaults(Vector3 _originalRot, BranchData _BD)
    {
        Vector3 newRot = _originalRot;
        switch (_GD._treeType)
        {
            case TreeType.DICHOTOMOUS:
                newRot.y += 180.0f;
                newRot.x = -55.0f;
                break;

            case TreeType.SYMPODIAL:
                newRot.y += 137.5f;
                newRot.x = -55.0f;
                break;

            case TreeType.MONOPODIAL:
                newRot.y += 90.0f;
                newRot.x = -55.0f;
                break;

            case TreeType.WHORLED:
                newRot.y += 90.0f;
                newRot.x = -55.0f;
                break;

            case TreeType.WEEPING:
                switch (_BD.Hierachy)
                {
                    case 0:

                        break;

                    case 1:
                        newRot.y += 90.0f;
                        newRot.x = -55.0f;
                        break;

                    case 2:
                        newRot.y += 90.0f;
                        newRot.x = -125.0f;
                        break;

                    case 3:
                        newRot.y += 90.0f;
                        newRot.x = -125.0f;
                        break;
                }
                break;

            default:
                break;
        }
        if (_BD.Hierachy < 1)
            newRot.x = 0.0f;
        return newRot;
    }

    public Vector3 SunDirectionRot(Vector3 _originalRot, BranchData _BD)
    {
        Vector3 newRot = _originalRot;
        //SECOND PASS FOR ENVIRON EFFECTS
        switch (_GD._windHeading)
        {
            case WindHeading.NONE:
                break;

            case WindHeading.NORTH:

                if ((newRot.y % 360.0f) > 180 && (newRot.y % 360.0f) < 360)
                {
                    newRot.x = -3.0f * _GD._sunStrength;
                }
                if (_BD.Hierachy < 1) //Trunk
                    newRot.x = 2.0f * _GD._sunStrength;

                break;

            case WindHeading.EAST:

                if ((newRot.y % 360.0f) > 270 || (newRot.y % 360.0f) < 90)
                {
                    newRot.x = -3.0f * _GD._sunStrength;
                }

                if (_BD.Hierachy < 1)//Trunk
                    newRot.x = 1.0f * _GD._sunStrength;

                break;

            case WindHeading.SOUTH:
                if ((newRot.y % 360.0f) < 180 && (newRot.y % 360.0f) > 0)
                {
                    newRot.x = -3.0f * _GD._sunStrength;
                }

                if (_BD.Hierachy < 1)//Trunk
                    newRot.x = 1.0f * _GD._sunStrength;

                break;

            case WindHeading.WEST:
                if ((newRot.y % 360.0f) > 270 || (newRot.y % 360.0f) < 90)
                {
                    newRot.x = -3.0f * _GD._sunStrength;
                }
                if (_BD.Hierachy < 1)//Trunk
                    newRot.x = 2.0f * _GD._sunStrength;

                break;

            default:
                break;
        }
        return newRot;
    }

    public Vector3 WindHeadingRot(Vector3 _originalRot, BranchData _BD)
    {
        Vector3 newRot = _originalRot;
        //SECOND PASS FOR ENVIRON EFFECTS
        switch (_GD._windHeading)
        {
            case WindHeading.NONE:
                break;

            case WindHeading.NORTH:

                if ((newRot.y % 360.0f) > 180 && (newRot.y % 360.0f) < 360)
                {
                    newRot.x = -3.0f * _GD._windSpeed;
                }
                if (_BD.Hierachy < 1) //Trunk
                    newRot.x = 2.0f * _GD._windSpeed;

                break;

            case WindHeading.EAST:

                if ((newRot.y % 360.0f) > 270 || (newRot.y % 360.0f) < 90)
                {
                    newRot.x = -3.0f * _GD._windSpeed;
                }

                if (_BD.Hierachy < 1)//Trunk
                    newRot.x = 1.0f * _GD._windSpeed;

                break;

            case WindHeading.SOUTH:
                if ((newRot.y % 360.0f) < 180 && (newRot.y % 360.0f) > 0)
                {
                    newRot.x = -3.0f * _GD._windSpeed;
                }

                if (_BD.Hierachy < 1)//Trunk
                    newRot.x = 1.0f * _GD._windSpeed;

                break;

            case WindHeading.WEST:
                if ((newRot.y % 360.0f) > 270 || (newRot.y % 360.0f) < 90)
                {
                    newRot.x = -3.0f * _GD._windSpeed;
                }
                if (_BD.Hierachy < 1)//Trunk
                    newRot.x = 2.0f * _GD._windSpeed;

                break;

            default:
                break;
        }
        return newRot;
    }

    public void AddFractals(BranchData _BD, BezierCurve _parent)
    {
        GameObject fractal = Resources.Load("FractalObj") as GameObject;
        Vector3 rotPos = _parent.GetPoint(1) - _parent.GetPoint(0.98f);
        //float rotationZ = (float)(Mathf.Atan2(rotPos.y, rotPos.x) / (2 * Mathf.PI));
        //float rotationX = (float)(Mathf.Atan2(rotPos.x, rotPos.z) / (2 * Mathf.PI));
        //float rotationY = (float)(Mathf.Atan2(rotPos.y, rotPos.z) / (2 * Mathf.PI));

        //Instantiate(fractal, _parent.GetPoint(1), Quaternion.identity, _parent.gameObject.transform);
        //fractal.transform.Rotate(new Vector3(rotationX, rotationY, rotationZ));
    }
}

//private void drawTree(float x1, float y1, float angle, int _depth)
//{
//    if (_depth != 0)
//    {
//        float x2 = x1 + (Mathf.Cos(angle * deg_to_rad) * _depth * scale);
//        float y2 = y1 + (Mathf.Sin(angle * deg_to_rad) * _depth * scale);
//        drawLine(x1, y1, x2, y2, _depth);
//        drawTree(x2, y2, angle - 20, _depth - 1);
//        drawTree(x2, y2, angle + 20, _depth - 1);
//    }
//}
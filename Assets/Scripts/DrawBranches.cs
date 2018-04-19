using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBranches : MonoBehaviour
{
    public int depth = 9; // gets slow over 15
    public GameData _GD;

    public float maxWindAngle = 30.0f;
    public float maxSunAngle = 50.0f;

    public bool editor = true;

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
        if (!editor)
            _GD = GameObject.Find("CONTROLLER").GetComponent<GameData>();
        else
            _GD = GetComponentInParent<GameData>();
    }

    //Surrounded, Sun strength
    public void DrawSplineBranches(BezierCurve spline, BranchData _BD)
    {
        if (!_GD)
            _GD = GameObject.Find("CONTROLLER").GetComponent<GameData>();
        float height = GetTreeHeight();

        height = GetBranchHeight(_BD, height);
        // height *= _GD._sunStrength;
        int nodeNum = (int)height - ((int)height % 4); //make a multiple of 4
        spline.DrawSpline(transform.position, nodeNum, _BD, _GD);
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
                newRot.y += 37.0f;
                newRot.x = -55.0f;
                break;

            default:
                break;
        }

        if (_BD.Hierachy < 1)
            newRot.x = 0.0f;

        return newRot;
    }

    public float GetTreeHeight()
    {
        float height = 50.0f;
        float a = height * _GD._avgPrecip; //+- 10%
        float b = height * _GD._avgTemp; //+=20%
        float c = height * (_GD.density / 10.0f) + 1;
        height += a + b + c;

        return height;
    }

    public void SetTreeTangliness()
    {
        float x = 0;

        //Precipitation
        if (_GD._avgPrecip > 0.05f)
            for (float i = _GD._avgTemp; i > 0; i -= 0.01f)
            {
                x += 0.01f;
                _GD._tangliness += x;
            }
        else if (_GD._avgPrecip < -0.05f)
            for (float i = _GD._avgPrecip; i < 0f; i += 0.01f)
            {
                x += 0.01f;
                _GD._tangliness += x;
            }

        //Temperature
        if (_GD._avgTemp > 0.1f)
            for (float i = _GD._avgTemp; i > 0; i -= 0.01f)
                x += 0.01f;
        else if (_GD._avgTemp < -0.1f)
            for (float i = _GD._avgTemp; i < 0f; i += 0.01f)
                x += 0.01f;

        //Wind
        if (_GD._windSpeed > 0.7f)
            for (float i = _GD._windSpeed; i > 0f; i -= 0.01f)
                x += 0.01f;

        _GD._tangliness += x;
    }

    public float GetBranchHeight(BranchData _BD, float height)
    {
        switch (_BD.Hierachy)
        {
            case 0:
                break;

            case 1:
                height *= 0.7f;
                break;

            case 2:
                height *= 0.3f;
                break;

            case 3:
                height *= 0.1f;
                break;

            default:
                break;
        }
        return height;
    }

    public Vector3 SunHeadingRot(Vector3 _originalRot, BranchData _BD)
    {
        Vector3 newRot = Vector3.zero;
        //SECOND PASS FOR ENVIRON EFFECTS
        switch (_GD._lightHeading)
        {
            case LightHeading.NONE:
                break;

            case LightHeading.NORTH:

                if (_BD.Hierachy == 1)
                {
                    if (ReturnBranchFacingDir(_originalRot, "N") == "N")
                    {
                        newRot.x = maxSunAngle * _GD._sunStrength;
                    }
                }
                else if (_BD.Hierachy < 1) //Trunk
                    newRot.x = maxSunAngle * _GD._sunStrength;

                break;

            case LightHeading.EAST:

                if (_BD.Hierachy == 1)
                {
                    if (ReturnBranchFacingDir(_originalRot, "E") == "E")
                    {
                        newRot.x = maxSunAngle * _GD._sunStrength;
                    }
                }
                else if (_BD.Hierachy < 1) //Trunk
                    newRot.x = maxSunAngle * _GD._sunStrength;

                break;

            case LightHeading.SOUTH:

                if (_BD.Hierachy == 1)
                {
                    if (ReturnBranchFacingDir(_originalRot, "S") == "S")
                    {
                        newRot.x = maxSunAngle * _GD._sunStrength;
                    }
                }
                else if (_BD.Hierachy < 1) //Trunk
                    newRot.x = maxSunAngle * _GD._sunStrength;

                break;

            case LightHeading.WEST:

                if (_BD.Hierachy == 1)
                {
                    if (ReturnBranchFacingDir(_originalRot, "W") == "W")
                    {
                        newRot.x = maxSunAngle * _GD._sunStrength;
                    }
                }
                else if (_BD.Hierachy < 1) //Trunk
                    newRot.x = maxSunAngle * _GD._sunStrength;

                break;

            default:
                break;
        }
        return new Vector3(newRot.x, 0, 0); //changes angle that branches can sprout out (towards sun)
    }

    public Vector3 WindHeadingRot(Vector3 _originalRot, BranchData _BD) //newRot = (max) -100 degrees * windspeed (0-1)
    {
        Vector3 newRot = Vector3.zero;
        //SECOND PASS FOR ENVIRON EFFECTS
        switch (_GD._windHeading)
        {
            case WindHeading.NONE:
                break;

            case WindHeading.NORTH:

                if (_BD.Hierachy == 1)
                {
                    if (ReturnBranchFacingDir(_originalRot, "N") == "N")
                    {
                        newRot.x = maxWindAngle * _GD._windSpeed;
                    }
                }
                else if (_BD.Hierachy < 1) //Trunk
                    newRot.x = maxWindAngle * (_GD._windSpeed / 2);
                break;

            case WindHeading.EAST:

                if (_BD.Hierachy == 1)
                {
                    if (ReturnBranchFacingDir(_originalRot, "E") == "E")
                    {
                        newRot.x = maxWindAngle * _GD._windSpeed;
                    }
                }
                else if (_BD.Hierachy < 1) //Trunk
                    newRot.x = maxWindAngle * (_GD._windSpeed / 2);

                break;

            case WindHeading.SOUTH:
                if (_BD.Hierachy == 1)
                {
                    if (ReturnBranchFacingDir(_originalRot, "S") == "S")
                    {
                        newRot.x = maxWindAngle * _GD._windSpeed;
                    }
                }
                else if (_BD.Hierachy < 1) //Trunk
                    newRot.x = maxWindAngle * (_GD._windSpeed / 2);

                break;

            case WindHeading.WEST:
                if (_BD.Hierachy == 1)
                {
                    if (ReturnBranchFacingDir(_originalRot, "W") == "W")
                    {
                        newRot.x = maxWindAngle * _GD._windSpeed;
                    }
                }
                else if (_BD.Hierachy < 1) //Trunk
                    newRot.x = maxWindAngle * (_GD._windSpeed / 2);

                break;

            default:
                break;
        }
        return new Vector3(newRot.x, 0, 0);
    }

    public GameObject AddFractals(GameObject _parent, BezierCurve _parentCurve)
    {
        GameObject fractal = Resources.Load("FractalObj") as GameObject;
        GameObject thisFractal = Instantiate(fractal, _parentCurve.GetPoint(1), Quaternion.identity);
        thisFractal.transform.SetParent(_parent.transform);
        //thisFractal.transform.SetPositionAndRotation(_parentCurve.GetPoint(1), _parent.transform.rotation);
        //thisFractal.transform.Rotate(_parent.transform.rotation.eulerAngles);
        return thisFractal;
    }

    public string ReturnBranchFacingDir(Vector3 rotation, string checkZone)
    {
        while (rotation.y < 0)
            rotation.y += 360;
        if (((rotation.y % 360.0f) > 180 && (rotation.y % 360.0f) < 360) && checkZone == "N")
        {
            return "N";
        }
        if ((((rotation.y % 360.0f) > 270 && (rotation.y % 360.0f) < 360)
            || ((rotation.y % 360.0f) < 90 && (rotation.y % 360.0f) > 0)) && checkZone == "E")
        {
            return "E";
        }
        if (((rotation.y % 360.0f) < 180 && (rotation.y % 360.0f) > 0) && checkZone == "S")
        {
            return "S";
        }
        if ((((rotation.y % 360.0f) < 270 && (rotation.y % 360.0f) > 90)) && checkZone == "W")
        {
            return "W";
        }
        return "";
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
//Vector3 rotPos = _parent.transform.rotation.eulerAngles;
////float rotationZ = (float)(Mathf.Atan2(rotPos.y, rotPos.x) / (2 * Mathf.PI));
////float rotationX = (float)(Mathf.Atan2(rotPos.x, rotPos.z) / (2 * Mathf.PI));
////float rotationY = (float)(Mathf.Atan2(rotPos.y, rotPos.z) / (2 * Mathf.PI));
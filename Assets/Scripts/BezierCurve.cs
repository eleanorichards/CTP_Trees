﻿using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public Vector3[] nodes;
    //public GameObject segments;
    //public GameObject branchPlacer;

    public int hierachyIndex;
    public int memberIndex;

    public int parentHierachy;
    public int parentIndex;
    public int globalIndex;

    private Vector3 initialStep;
    private GameData _GD;

    //Initialisation
    private void Start()
    {
        _GD = GameObject.Find("CONTROLLER").GetComponent<GameData>();
    }

    public void SetInitialStatus(int _hierachyIndex, int _memberIndex)
    {
        hierachyIndex = _hierachyIndex;
        memberIndex = _memberIndex;
        parentHierachy = hierachyIndex - 1;
    }

    public void SetAllNodes(Vector3 node0Pos)
    {
        nodes[0] = node0Pos;
        // initialStep = (node1Pos - node0Pos);
        switch (hierachyIndex)
        {
            case 0:
                initialStep = new Vector3(0, 2, 0);
                PlaceTrunk();
                break;

            case 1:
                initialStep = new Vector3(0, 0, 1);
                PlaceTierOne();
                break;

            case 2:
                initialStep = new Vector3(1, 0, 0);
                PlaceTierTwo();
                break;

            default:
                break;
        }
    }

    public void PlaceTrunk()
    {
        nodes[0] = Vector3.zero;
        int y = 2;
        for (int i = 1; i < nodes.Length; i++)
        {
            nodes[i] = new Vector3(Random.Range(-_GD._tangliness, _GD._tangliness), y, Random.Range(-_GD._tangliness, _GD._tangliness));
            y += 2;
        }
    }

    //Tier one for the first set of branches along the tree
    public void PlaceTierOne()
    {
        switch (_GD._windHeading)
        {
            case WindHeading.NONE:
                for (int i = 1; i < nodes.Length; i++)
                {
                    //nodes[i] = (nodes[i - 1] + initialStep) + RandomVector(-_GD._tangliness, _GD._tangliness);
                    nodes[i] = (nodes[i - 1] + initialStep) + RandomVector(-_GD._tangliness, _GD._tangliness);
                }

                break;

            case WindHeading.NORTH:
                for (int i = 1; i < nodes.Length; i++)
                {
                    if (nodes[1].x > 0)
                    {
                        nodes[i] = new Vector3((nodes[i - 1].x + initialStep.x), ((nodes[i - 1].y + initialStep.y) + 1), (nodes[i - 1].z + initialStep.z)) + RandomVector(-_GD._tangliness, _GD._tangliness);
                    }
                    else
                    {
                        nodes[i] = (nodes[i - 1] + initialStep) + RandomVector(-_GD._tangliness, _GD._tangliness);
                    }
                }
                break;

            case WindHeading.EAST:
                for (int i = 1; i < nodes.Length; i++)
                {
                    if (nodes[1].z < 0)
                    {
                        nodes[i] = new Vector3((nodes[i - 1].x + initialStep.x), (nodes[i - 1].y + initialStep.y + 1), (nodes[i - 1].z + initialStep.z)) + RandomVector(-_GD._tangliness, _GD._tangliness);
                    }
                    else
                    {
                        nodes[i] = (nodes[i - 1] + initialStep) + RandomVector(-_GD._tangliness, _GD._tangliness);
                    }
                }
                break;

            case WindHeading.SOUTH:
                for (int i = 1; i < nodes.Length; i++)
                {
                    if (nodes[1].x < 0)
                    {
                        nodes[i] = new Vector3((nodes[i - 1].x + initialStep.x), (nodes[i - 1].y + initialStep.y + 1), (nodes[i - 1].z + initialStep.z)) + RandomVector(-_GD._tangliness, _GD._tangliness);
                    }
                    else
                    {
                        nodes[i] = (nodes[i - 1] + initialStep) + RandomVector(-_GD._tangliness, _GD._tangliness);
                    }
                }
                break;

            case WindHeading.WEST:
                for (int i = 1; i < nodes.Length; i++)
                {
                    if (nodes[1].z > 0)
                    {
                        nodes[i] = new Vector3((nodes[i - 1].x + initialStep.x), (nodes[i - 1].y + initialStep.y + 1), (nodes[i - 1].z + initialStep.z)) + RandomVector(-_GD._tangliness, _GD._tangliness);
                    }
                    else
                    {
                        nodes[i] = (nodes[i - 1] + initialStep) + RandomVector(-_GD._tangliness, _GD._tangliness);
                    }
                }
                break;

            default:
                break;
        }
    }

    public void PlaceTierTwo()
    {
        for (int i = 1; i < nodes.Length; i++)
        {
            nodes[i] = (nodes[i - 1] + initialStep) + (RandomVector(-_GD._tangliness, _GD._tangliness));
        }
    }

    public void SetNodeSize(int nodeNum)
    {
        System.Array.Resize(ref nodes, nodeNum);
    }

    public Vector3 RandomVector(float min, float max)
    {
        return new Vector3(Random.Range(min, max), Random.Range(min, max) + 0.2f, Random.Range(min, max));
    }

    public void SetParentIndex(int _parentIndex)
    {
        //parentHierachy = _parentHierachy;
        parentIndex = _parentIndex;
    }

    public void SetGlobalIndex(int _globalIndex)
    {
        globalIndex = _globalIndex;
    }

    public void AddCurve()
    {
        Vector3 node = nodes[nodes.Length - 1];
        System.Array.Resize(ref nodes, nodes.Length + 3);
        node.y += 1f;
        nodes[nodes.Length - 3] = node;
        node.y += 1f;
        nodes[nodes.Length - 2] = node;
        node.y += 1f;
        nodes[nodes.Length - 1] = node;
    }

    public Vector3 GetPoint(float t)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = nodes.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }

        return transform.TransformPoint(Bezier.GetPoint(nodes[i], nodes[i + 1], nodes[i + 2], nodes[i + 3], t));
    }

    public void Reset()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = new Vector3(0f, i, 0f);
        }
    }

    /// <summary>
    /// returns (nodes.length - 1) / 3
    /// </summary>
    public int CurveCount
    {
        get
        {
            return (nodes.Length - 1) / 3;
        }
    }

    //show directional lines (for following splines)
    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }

    //returns magnitude of direction
    public Vector3 GetVelocity(float t)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = nodes.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }
        return transform.TransformPoint(Bezier.GetFirstDerivative(
            nodes[i], nodes[i + 1], nodes[i + 2], nodes[i + 3], t)) - transform.position;
    }

    public Vector3 GetNormal2D(float t)
    {
        Vector3 tng = GetDirection(t);
        return new Vector3(-tng.y, tng.x, 0f);
    }

    public Vector3 GetNormal3D(float t, Vector3 up)
    {
        Vector3 tng = GetDirection(t);
        Vector3 binormal = Vector3.Cross(up, tng).normalized;
        return Vector3.Cross(tng, binormal);
    }

    public Quaternion GetOrientation2D(float t)
    {
        Vector3 tng = GetDirection(t);
        Vector3 nrm = GetNormal2D(t);
        return Quaternion.LookRotation(tng, nrm);
    }

    public Quaternion GetOrientation3D(float t, Vector3 up)
    {
        Vector3 tng = GetDirection(t);
        Vector3 nrm = GetNormal3D(t, up);
        return Quaternion.LookRotation(tng, nrm);
    }

    public void ExtrudeShape()
    {
        //segments.GetComponent<segmentPlacer>().PlaceShapes();
        //Mesh mesh = splineGeo.GetMesh();
        //var shape = splineGeo.GetExtrudeShape();
        //var path = splineGeo.GetPath();

        //splineGeo.Extrude(mesh, shape, path);
    }
}
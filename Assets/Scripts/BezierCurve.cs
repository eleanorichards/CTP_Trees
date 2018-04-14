using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public Vector3[] nodes;

    private Vector3 initialStep;
    private GameData _GD;
    private int x = 0;
    /// <summary>
    /// First Func Called
    /// Initialise here, BranchData already set
    /// </summary>
    /// <param name="node0Pos"></param>
    /// <param name="segmentCount"></param>
    public void DrawSpline(Vector3 node0Pos, int segmentCount, BranchData _BD)
    {
        segmentCount /= 4;

        if (segmentCount < 4)
            segmentCount = 4;
        SetNodeSize(segmentCount);

        //i == 1/groupID
        if (!_GD)
            _GD = GameObject.Find("CONTROLLER").GetComponent<GameData>();
        x = 0;

        for (int i = 1; i < segmentCount; i++)
        {
            x += 4;
            nodes[i] = new Vector3(Random.Range(-_GD._tangliness, _GD._tangliness), x, Random.Range(-_GD._tangliness, _GD._tangliness));
        }
    }

    public void SetAllNodes(Vector3 node0Pos, Vector3 node1Pos)
    {
        nodes[0] = node0Pos;
        nodes[1] = node1Pos;

        initialStep = (node1Pos - node0Pos);
    }

    public void SetNodeSize(int nodeNum)
    {
        System.Array.Resize(ref nodes, nodeNum);
    }

    public Vector3 RandomVector(float min, float max)
    {
        return new Vector3(Random.Range(min, max), Random.Range(min, max) + 0.2f, Random.Range(min, max));
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
}
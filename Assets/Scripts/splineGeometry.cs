using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class splineGeometry : MonoBehaviour {
    public MeshFilter mf;
    public Mesh mesh;
    public BezierCurve curve;

    // Use this for initialization
    void Start () {
        //vertex locations
        mf = GetComponent<MeshFilter>();
        mesh = mf.mesh;
    }

    ///
    /// EXTRUDE
    /// 
    public void Extrude(Mesh mesh, ExtrudeShape shape, OrientedPoint[] path)
    {
        int vertsInShape = shape.vert2Ds.Length;
        int segments = path.Length - 1;
        int edgeLoops = path.Length;
        int vertCount = vertsInShape * edgeLoops;
        int triCount = shape.lines.Length * segments;
        int triIndexCount = triCount * 3;

        var triangleIndices = new int[triIndexCount];
        var vertices = new Vector3[vertCount];
        var normals = new Vector3[vertCount];
        var uvs = new Vector2[vertCount];

        float totalLength = 0;
        float distanceCovered = 0;
        for (int i = 0; i < path.Length - 1; i++)
        {
            var d = Vector3.Distance(path[i].position, path[i + 1].position);
            totalLength += d;
        }

        for (int i = 0; i < path.Length; i++)
        {
            int offset = i * vertsInShape;
            if (i > 0)
            {
                var d = Vector3.Distance(path[i].position, path[i - 1].position);
                distanceCovered += d;
            }
            float v = distanceCovered / totalLength;

            for (int j = 0; j < vertsInShape; j++)
            {
                int id = offset + j;
                vertices[id] = path[i].LocalToWorld(shape.vert2Ds[j].point);
                normals[id] = path[i].LocalToWorldDirection(shape.vert2Ds[j].normal);
                uvs[id] = new Vector2(shape.vert2Ds[j].uCoord, v);
            }
        }
        int ti = 0;
        for (int i = 0; i < segments; i++)
        {
            int offset = i * vertsInShape;
            for (int l = 0; l < shape.lines.Length; l += 2)
            {
                int a = offset + shape.lines[l] + vertsInShape;
                int b = offset + shape.lines[l];
                int c = offset + shape.lines[l + 1];
                int d = offset + shape.lines[l + 1] + vertsInShape;
                triangleIndices[ti] = c; ti++;
                triangleIndices[ti] = b; ti++;
                triangleIndices[ti] = a; ti++;
                triangleIndices[ti] = a; ti++;
                triangleIndices[ti] = d; ti++;
                triangleIndices[ti] = c; ti++;
            }
        }


        mesh.Clear();
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangleIndices;
    }


    /// 
    /// STRUCTS
    /// 
    public struct Vertex
    {
        public Vector3 point;
        public Vector3 normal;
        public float uCoord;


        public Vertex(Vector3 point, Vector3 normal, float uCoord)
        {
            this.point = point;
            this.normal = normal;
            this.uCoord = uCoord;
        }
    }

    public struct ExtrudeShape
    {
        public Vertex[] vert2Ds;
        public int[] lines;

        public ExtrudeShape(Vertex[] vert2Ds, int[] lines)
        {
            this.vert2Ds = vert2Ds;
            this.lines = lines;
        }
    }

    public struct OrientedPoint
    {
        public Vector3 position;
        public Quaternion rotation;

        public OrientedPoint(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }

        public Vector3 LocalToWorld(Vector3 point)
        {
            return position + rotation * point;
        }

        public Vector3 WorldToLocal(Vector3 point)
        {
            return Quaternion.Inverse(rotation) * (point - position);
        }

        public Vector3 LocalToWorldDirection(Vector3 dir)
        {
            return rotation * dir;
        }

    }
    ///
    ///GETTERS
    ///
    public Mesh GetMesh()
    {
        if (mf.mesh == null)
        {
            mf.mesh = new Mesh();
        }
        return mf.mesh;
    }

    public ExtrudeShape GetExtrudeShape()
    {
        var vert2Ds = new Vertex[] {
                new Vertex(
                    new Vector3(0, 0, 0),
                    new Vector3(0, 1, 0),
                    0),
                new Vertex(
                    new Vector3(2, 0, 0),
                    new Vector3(0, 1, 0),
                    0.5f),
                new Vertex(
                    new Vector3(2, 0, 0),
                    new Vector3(0, 1, 0),
                    0.5f),
                new Vertex(
                    new Vector3(4, 0, 0),
                    new Vector3(0, 1, 0),
                    1)
            };

        var lines = new int[] {
                0, 1,
                1, 2,
                2, 3
            };

        return new ExtrudeShape(vert2Ds, lines);

    }

    public OrientedPoint[] GetPath()
    {

        var path = new List<OrientedPoint>();

        for (float t = 0; t <= 1; t += 0.1f)
        {
            var point = curve.GetPoint(t);
            var rotation = curve.GetOrientation3D(t, Vector3.up);
            path.Add(new OrientedPoint(point, rotation));
        }

        return path.ToArray();
    }

}



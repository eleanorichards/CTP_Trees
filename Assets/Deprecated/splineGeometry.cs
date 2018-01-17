using System.Collections.Generic;
using UnityEngine;

public class splineGeometry : MonoBehaviour
{
    public MeshFilter mf;
    public Mesh mesh;
    public BezierCurve curve;
    public int cap_resolution = 5;
    public int radius = 1;
    public Vertex[] vert2Ds;

    // Use this for initialization
    private void Start()
    {
        //vertex locations
        mf = GetComponent<MeshFilter>();
        mesh = mf.sharedMesh;
        //vert2Ds = new Vertex[mesh.vertexCount];
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
        //creating the faces - could be moved over with cylinder code?
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
        if (mf.sharedMesh == null)
        {
            mf.sharedMesh = new Mesh();
        }
        return mf.sharedMesh;
    }

    //public ExtrudeShape GetExtrudeShape()
    //{
    //    int height = curve.nodes.Length;
    //    //columns and rows
    //    int column_num = cap_resolution + 1;
    //    int row_num = height + 1;

    //    //Vertices
    //    int vertice_num = column_num * row_num;

    //    //Normals
    //    int normals_num = vertice_num;

    //    //uvs are always equal to no of vertices
    //    int uv_num = vertice_num;

    //    //side faces/tris without caps
    //    int side_faces_num = cap_resolution * height * 2;

    //    //cap faces
    //    int cap_faces_num = cap_resolution - 2;

    //    //initialise all arrays
    //    Vector3[] vertices = new Vector3[vertice_num];
    //    Vector3[] normals = new Vector3[normals_num];
    //    Vector2[] uvs = new Vector2[uv_num];

    //    //angle step for each column for side tris
    //    float step = Mathf.PI * 2 / cap_resolution;

    //    /*
    //       first for loop computes all the side faces of the cylinder
    //       second loop computes tris for top and bottom caps
    //    */
    //    for (int i = 0; i < row_num; i++)
    //    {
    //        for (int j = 0; j < column_num; j++)
    //        {
    //            float angle = j * step;

    //            //folding from the first and last vertex
    //            if (j == column_num - 1) angle = 0;

    //            //compute vertices, uvs and normals for each row and column offsets
    //            //Change this for curved/warped branches
    //            vertices[i * column_num + j] = new Vector3(radius * Mathf.Cos(angle), i * height, radius * Mathf.Sin(angle)); //build a cylinder with an upwards orientation
    //            uvs[i * column_num + j] = new Vector2(j * 1 / radius, 0);
    //            normals[i * column_num + j] = new Vector3(0, 0, -1.0f);

    //        }
    //    }

    //    //VERTEX DATA:
    //    //NORMAL
    //    //POINT
    //    //UV
    //    //var vert2Ds = new Vertex[] {
    //    //        new Vertex(
    //    //            new Vector3(0, 0, 0),
    //    //            new Vector3(0, 1, 0),
    //    //            0),
    //    //        new Vertex(
    //    //            new Vector3(1, 0, 0),
    //    //            new Vector3(0, 1, 0),
    //    //            0.5f),
    //    //        new Vertex(
    //    //            new Vector3(-1, 0,0),
    //    //            new Vector3(0, 1, 0),
    //    //            0.5f),
    //    //        new Vertex(
    //    //            new Vector3(0, 0, 0),
    //    //            new Vector3(0, 1, 0),
    //    //            1)
    //    //    };

    //    //var lines = new int[] {
    //    //        0, 1,
    //    //        1, 2,
    //    //        2, 3
    //    //    };

    //    /*

    //     The problem here is only once instance on vert2D is created

    //     */
    //    //mesh.vertexCount

    //    // var vert2Ds = new Vertex[];

    //        for (int i = 0; i < vertices.Length; i++)
    //        {
    //            vert2Ds[i] =
    //                new Vertex(
    //                    (vertices[i]),
    //                    (normals[i]),
    //                        uvs[i].x);

    //        }

    //    //var vert2Ds = new Vertex[] {
    //    //        new Vertex(
    //    //            new Vector3(-1, 0, 0),
    //    //            new Vector3(0, 1, 0),
    //    //            0),
    //    //        new Vertex(
    //    //            new Vector3(-0.5f, 0.5f, 0.0f),
    //    //            new Vector3(0, 1, 0),
    //    //            0.5f),
    //    //        new Vertex(
    //    //            new Vector3(0.0f, 1, 0),
    //    //            new Vector3(0, 1, 0),
    //    //            0.5f),
    //    //        new Vertex(
    //    //            new Vector3(0.5f, 0.5f, 0.0f),
    //    //            new Vector3(0, 1, 0),
    //    //            1),
    //    //         new Vertex(
    //    //            new Vector3(1.0f, 0, 0),
    //    //            new Vector3(0, 1, 0),
    //    //            1),
    //    //        new Vertex(
    //    //            new Vector3(0.5f, -0.5f, 0.0f),
    //    //            new Vector3(0, 1, 0),
    //    //            1),
    //    //        new Vertex(
    //    //            new Vector3(0.0f, -1.0f, 0),
    //    //            new Vector3(0, 1, 0),
    //    //            0.5f),
    //    //        new Vertex(
    //    //            new Vector3(-0.5f, -0.5f, 0f),
    //    //            new Vector3(0, 1, 0),
    //    //            0.5f),
    //    //        new Vertex(
    //    //            new Vector3(-1, 0, 0),
    //    //            new Vector3(0, 1, 0),
    //    //            0)
    //    //    };
    //    var lines = new int[] {
    //            0, 1,
    //            1, 2,
    //            2, 3,
    //            3, 4,
    //            4, 5,
    //            5, 6,
    //            6, 7,
    //            7, 8

    //        };

    //    return new ExtrudeShape(vert2Ds, lines);

    //}
    public ExtrudeShape GetExtrudeShape()
    {
        //VERTEX DATA:
        //NORMAL
        //POINT
        //UV
        var vert2Ds = new Vertex[] {
                new Vertex(
                    new Vector3(0, 0, 0),
                    new Vector3(0, 1, 0),
                    0),
                new Vertex(
                    new Vector3(0, 0, 1),
                    new Vector3(0, 1, 0),
                    0.5f),
                new Vertex(
                    new Vector3(0, 0, 0),
                    new Vector3(0, 1, 0),
                    0.0f),
                new Vertex(
                    new Vector3(0, 0, -1),
                    new Vector3(0, 1, 0),
                    1)
            };

        var lines = new int[] {
                0, 1,
                1, 2,
                2,3,
            };
        //var vert2Ds = new Vertex[] {
        //        new Vertex(
        //            new Vector3(-1, 0, 0),
        //            new Vector3(0, 1, 0),
        //            0),
        //        new Vertex(
        //            new Vector3(-0.5f, 0.5f, 0.0f),
        //            new Vector3(0, 1, 0),
        //            0.5f),
        //        new Vertex(
        //            new Vector3(0.0f, 1, 0),
        //            new Vector3(0, 1, 0),
        //            0.5f),
        //        new Vertex(
        //            new Vector3(0.5f, 0.5f, 0.0f),
        //            new Vector3(0, 1, 0),
        //            1),
        //         new Vertex(
        //            new Vector3(1.0f, 0, 0),
        //            new Vector3(0, 1, 0),
        //            1),
        //        new Vertex(
        //            new Vector3(0.5f, -0.5f, 0.0f),
        //            new Vector3(0, 1, 0),
        //            1),
        //        new Vertex(
        //            new Vector3(0.0f, -1.0f, 0),
        //            new Vector3(0, 1, 0),
        //            0.5f),
        //        new Vertex(
        //            new Vector3(-0.5f, -0.5f, 0f),
        //            new Vector3(0, 1, 0),
        //            0.5f),
        //        new Vertex(
        //            new Vector3(-1, 0, 0),
        //            new Vector3(0, 1, 0),
        //            0)
        //    };
        //var lines = new int[] {
        //        0, 1,
        //        1, 2,
        //        2, 3,
        //        3, 4,
        //        4, 5,
        //        5, 6,
        //        6, 7,
        //        7, 8

        //    };

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
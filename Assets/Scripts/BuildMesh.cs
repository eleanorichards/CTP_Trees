using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=IYMQ2ErFz0s&t=6s


public class BuildMesh : MonoBehaviour {

    public int cap_resolution = 3;
    public int radius = 1;
    public int height = 3;

    public Vector3[] vertices;
    private Vector2[] uvs;
    private Vector3[] normals;
    int[] faces;
    int[] triangles;

    public Vector3 halfAxis = Vector3.up;

    // Use this for initialization
    void Start () {
        RecalculateMesh();
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void RecalculateMesh()
    {
        print("starting...");
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = mf.mesh;

        ComputeCyclinder();

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = faces;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
    }

    void ComputeCyclinder()
    {
        //columns and rows
        int column_num = cap_resolution + 1;
        int row_num = height + 1;

        //Vertices
        int vertice_num = column_num * row_num;
     
        //Normals
        int normals_num = vertice_num;

        //uvs are always equal to no of vertices
        int uv_num = vertice_num;

        //side faces/tris without caps
        int side_faces_num = cap_resolution * height * 2;

        //cap faces
        int cap_faces_num = cap_resolution - 2;

        //initialise all arrays
        vertices = new Vector3[vertice_num];
        normals = new Vector3[normals_num];
        uvs = new Vector2[uv_num];
        faces = new int[(side_faces_num + cap_faces_num * 2) * 3];

        //angle step for each column for side tris
        float step = Mathf.PI * 2 / cap_resolution;

        /*
           first for loop computes all the side faces of the cylinder
           second loop computes tris for top and bottom caps
        */
        for (int i = 0; i < row_num; i++)
        {
            for (int j = 0; j < column_num; j++)
            {
                float angle = j * step;

                //folding from the first and last vertex
                if (j == column_num - 1) angle = 0;

                //compute vertices, uvs and normals for each row and column offsets
                vertices[i * column_num + j] = new Vector3(radius * Mathf.Cos(angle), i * height, radius * Mathf.Sin(angle)); //build a cylinder with an upwards orientation
                uvs[i * column_num + j] = new Vector2(j * 1 / radius, i * 1 / halfAxis.y);
                normals[i * column_num + j] = new Vector3(0, 0, -1.0f);

                    /*
                    To create faces, we ignore the first row and the last column
                    for every other vertex we create two triangle faces at the same time in one loop
                    */
                if (i != 0 && j < column_num - 1)
                {
                    //offset the initial space for storing tris for bottom cap
                    int index = cap_faces_num * 3 + (i - 1) * cap_resolution * 6 + j * 6;

                    //create the first face
                    faces[index + 0] = i * column_num + j;
                    faces[index + 1] = i * column_num + j + 1;
                    faces[index + 2] = (i - 1) * column_num + j;

                    //create the second face
                    faces[index + 3] = (i - 1) * column_num + j;
                    faces[index + 4] = i * column_num + j + 1;
                    faces[index + 5] = (i - 1) * column_num + j + 1;
                }
            }
        }

        /*drawing top and bottom caps
        we need the firstIndex, midIndex and lastIndex as vertices for cap tris and store it in the faces array*/

        int firstIndex = 0;
        int midIndex = 0;
        int lastIndex = 0;
        int topCapOffset = vertice_num - column_num;

        for (int i = 0; i < cap_faces_num; i++)
        {
            //we get the bottom index to populate faces for bottom cap
            int bottomIndex = i * 3;

            //top cap tris will be stored in the empty end location of faces array
            int topIndex = (cap_faces_num + side_faces_num) * 3 + i * 3;

            //get the three index for each vertex to make a cap tri
            if (i == 0)
            {
                firstIndex = 1;
                midIndex = 0;
                lastIndex = column_num - 2;
            }
            else
            {
                midIndex = lastIndex;
                lastIndex = lastIndex - 1;
            }

            //populate triangle vertices for bottom cap
            faces[bottomIndex + 0] = lastIndex;
            faces[bottomIndex + 1] = midIndex;
            faces[bottomIndex + 2] = firstIndex;

            //populate triangle vertices for top cap
            faces[topIndex + 0] = topCapOffset + firstIndex;
            faces[topIndex + 1] = topCapOffset + midIndex;
            faces[topIndex + 2] = topCapOffset + lastIndex;
        }

    }
}

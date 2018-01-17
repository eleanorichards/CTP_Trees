using UnityEngine;

public class CreateCylinder : MonoBehaviour
{
    public Vector3 halfAxis = Vector3.up;
    public float radius = 3f;
    public int capResolution = 3;
    public int height = 2;
    private const int MAX_CAP_RES = 3;
    private const int MAX_RADIUS = 1;

    private Vector3[] vertices;
    private Vector2[] uvs;
    private Vector3[] normals;
    private int[] faces;

    private void Start()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = mf.mesh;
        ComputeCylinder();
        mesh.Clear();
        mesh.vertices = vertices;
        //mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        print("done");
    }

    private void ComputeCylinder()
    {
        if (capResolution < MAX_CAP_RES) capResolution = MAX_CAP_RES;
        if (radius < MAX_RADIUS) radius = MAX_RADIUS;

        //define total columns and rows
        int noOfColumns = capResolution + 1;
        int noOfRows = height + 1;

        //total number of vertices that make up the cylinder
        int noOfVertices = noOfColumns * noOfRows;

        //no of normals for each vertex
        int noOfNormals = noOfVertices;

        //uvs are always equal to no of vertices in a mesh
        int noOfUvs = noOfVertices;

        //side faces (tris) without the top and bottom caps
        int noOfSideFaces = capResolution * height * 2;

        //cap faces (2 caps bottom and top)
        int noOfCapFaces = capResolution - 2;

        //initialize all the arrays
        vertices = new Vector3[noOfVertices];
        normals = new Vector3[noOfNormals];
        uvs = new Vector2[noOfUvs];
        faces = new int[(noOfSideFaces + noOfCapFaces * 2) * 3];

        //angle step for each column for side tris
        float step = Mathf.PI * 2 / capResolution;

        /*
            first for loop computes all the side faces of the cylinder
            second loop computes tris for top and bottom caps
            */
        for (int i = 0; i < noOfRows; i++)
        {
            for (int j = 0; j < noOfColumns; j++)
            {
                float angle = j * step;

                //folding from the first and last vertex
                if (j == noOfColumns - 1) angle = 0;

                //compute vertices, uvs and normals for each row and column offsets
                vertices[i * noOfColumns + j] = new Vector3(radius * Mathf.Cos(angle), i * height, radius * Mathf.Sin(angle)); //build a cylinder with an upwards orientation
                uvs[i * noOfColumns + j] = new Vector2(j * 1 / radius, i * 1 / halfAxis.y);
                normals[i * noOfColumns + j] = new Vector3(0, 0, -1.0f);

                /*
                    To create faces, we ignore the first row and the last column
                    for every other vertex we create two triangle faces at the same time in one loop
                    */
                if (i != 0 && j < noOfColumns - 1)
                {
                    //offset the initial space for storing tris for bottom cap
                    int index = noOfCapFaces * 3 + (i - 1) * capResolution * 6 + j * 6;

                    //create the first face
                    faces[index + 0] = i * noOfColumns + j;
                    faces[index + 1] = i * noOfColumns + j + 1;
                    faces[index + 2] = (i - 1) * noOfColumns + j;

                    //create the second face
                    faces[index + 3] = (i - 1) * noOfColumns + j;
                    faces[index + 4] = i * noOfColumns + j + 1;
                    faces[index + 5] = (i - 1) * noOfColumns + j + 1;
                }
            }
        }

        /*drawing top and bottom caps
        we need the firstIndex, midIndex and lastIndex as vertices for cap tris and store it in the faces array*/

        int firstIndex = 0;
        int midIndex = 0;
        int lastIndex = 0;
        int topCapOffset = noOfVertices - noOfColumns;

        for (int i = 0; i < noOfCapFaces; i++)
        {
            //we get the bottom index to populate faces for bottom cap
            int bottomIndex = i * 3;

            //top cap tris will be stored in the empty end location of faces array
            int topIndex = (noOfCapFaces + noOfSideFaces) * 3 + i * 3;

            //get the three index for each vertex to make a cap tri
            if (i == 0)
            {
                firstIndex = 1;
                midIndex = 0;
                lastIndex = noOfColumns - 2;
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
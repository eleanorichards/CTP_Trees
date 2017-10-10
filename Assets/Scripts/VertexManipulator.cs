using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexManipulator : MonoBehaviour {
    public float stretch_X;
    public float stretch_Y;
    public float stretch_Z;


    public int vertex_min;
    public int vertex_max;

    private float OG_x;
    private float OG_y;
    private float OG_z;

    Vector3[] vertices;
    Mesh mesh = null;

    // Use this for initialization
    void Start () {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        for (int i = vertex_min; i < vertex_max; i++)
        {
            OG_x = vertices[i].x;
            OG_y = vertices[i].y;
            OG_z = vertices[i].z;
        }
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = vertex_min; i < vertex_max; i++)
        {            
            
            vertices[i].x = stretch_X;

            vertices[i].y = stretch_Y;
           
            vertices[i].z = stretch_Z;
            i++;
            
        }
        
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }
}

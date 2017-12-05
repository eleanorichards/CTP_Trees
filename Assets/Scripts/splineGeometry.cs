using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class splineGeometry : MonoBehaviour {
   
	// Use this for initialization
	void Start () {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = mf.mesh;
        //vertex locations
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(1,0,1),
            new Vector3(-1,0,1),
            new Vector3(1,0,-1),
            new Vector3(-1,0,-1)
        };
        Vector3[] normals = new Vector3[]
        {
            new Vector3(0,1,0),
            new Vector3(0,1,0),
            new Vector3(0,1,0),
            new Vector3(0,1,0)
        };
        //for texturing
        Vector2[] uvs = new Vector2[]
        {
            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),
            new Vector2(1,0)
        };
        //polygons
        int[] triangles = new int[]
        {
            0,2,3,
            3,1,0
        };

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.normals = normals;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

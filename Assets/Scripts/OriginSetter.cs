using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginSetter : MonoBehaviour {

    private List<GameObject> branch_origins = new List<GameObject>(100);

    public GameObject generator;

    public float x_pos;
    public float y_pos;
    public float z_pos;

    public float x_rot;
    public float y_rot;
    public float z_rot;

    public float num_of_branches = 10;

	// Use this for initialization
	void Start () {
          SetBranchPositions();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetBranchPositions()
    {
        branch_origins.Clear();
        for (int i = 0; i < num_of_branches; i++)
        {
            float angle = i * Mathf.PI * 2 / num_of_branches;
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 5;
            Instantiate(generator, pos, Quaternion.identity);
        }
    }
}

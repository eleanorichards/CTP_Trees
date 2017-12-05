using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginSetter : MonoBehaviour {

    private List<GameObject> branch_origins = new List<GameObject>(100);

    public GameObject generator;

    public float y_rotation;
    public float y_incrementer = 0.0f;
    public float num_of_branches = 10;
    public float z_spinOffset;
    public float offset = 0;
    private float y_temp = 0;
    // Use this for initialization
    void Start () {
          SetBranchPositions();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetBranchPositions()
    {
       
        //rotation.x = 90 degrees by default
        branch_origins.Clear();
        for (int i = 0; i < num_of_branches; i++)
        {
            float angle = i * Mathf.PI * 2 / num_of_branches;
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle) * z_spinOffset);
            y_temp += y_incrementer;
            pos.y = y_temp;        
            Instantiate(generator, pos, Quaternion.Euler(new Vector3(90,0, (angle*y_rotation)+offset)));
        }
    }
}

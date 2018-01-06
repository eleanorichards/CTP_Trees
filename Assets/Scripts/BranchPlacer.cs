using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchPlacer : MonoBehaviour {
  
    public int branch_num;
    public BezierCurve branch;
    public BezierCurve Trunk;
    Vector3 origin;

    void Start()
    {
         //branch = (BezierCurve)Resources.Load("Prefabs/Bezier", typeof(BezierCurve));
         //TrunkCurve = Trunk.GetComponent<BezierCurve>();
    }

	public void AddBranch()
    {
        float stepSize = 1f / (branch_num);
        for (int i = 0; i < branch_num; i++)
        {
            if(i == 0)
            {
                origin = Vector3.zero;
                Instantiate(branch, origin, Quaternion.Euler(new Vector3(0, 0, 0)));
                branch.tag = "Trunk";
            }
            float angle = i * Mathf.PI * 2 / branch_num;
            origin = Trunk.GetPoint(i);
            Instantiate(branch, origin, Quaternion.Euler(new Vector3(90, 0, (angle))));
            branch.transform.position = new Vector3(i, 0, 0);
            branch.transform.rotation = Trunk.GetOrientation3D((i * branch_num), Vector3.up);
        }
    }
    
}

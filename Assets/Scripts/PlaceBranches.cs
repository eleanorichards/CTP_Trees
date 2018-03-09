using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBranches : MonoBehaviour
{
    private FractalGen fractalGen;
    private List<GameObject> BranchTransforms = new List<GameObject>();

    [Tooltip("Should be multiple of predecessor")]
    public int[] tierCount;

    private int branchNum;
    private int depth; //Reverse direction of hierachy

    // Use this for initialization
    private void Start()
    {
        for (int i = 0; i < tierCount.Length; i++)
        {
            branchNum += tierCount[i];
        }

        for (int i = 0; i < branchNum; i++)
        {
            GameObject branch = new GameObject("branch" + i);
            BranchTransforms.Add(branch);
            LineRenderer line = BranchTransforms[i].AddComponent<LineRenderer>();
            BranchData _BD = BranchTransforms[i].AddComponent<BranchData>();
            InitBranchValues(i);
            InitBranchPos(i);
            InitLineRenderer(line);
        }
    }

    private void InitLineRenderer(LineRenderer line)
    {
        line.GetComponent<Renderer>().enabled = true;
        line.useWorldSpace = false;
        // line.startWidth = _depth * 0.08f;
        //line.endWidth = _depth * 0.06f;
        line.startColor = Color.black;
        line.endColor = Color.green;
        line.material = Resources.Load("bark") as Material;
    }

    private void InitBranchValues(int globalID)
    {
        BranchData _BD = BranchTransforms[globalID].GetComponent<BranchData>();
        for (int i = 0; i < tierCount.Length; i++)
        {
        }

        if (globalID < tierCount[0])
        {
            _BD.Hierachy = 0;
            _BD.GlobalID = globalID; //Set global ID
            _BD.GroupID = globalID;
        }
        else if (globalID < tierCount[1] + tierCount[0])
        {
            _BD.Hierachy = 1;
            _BD.GlobalID = globalID; //Set global ID
            _BD.GroupID = globalID - (tierCount[0]); //Set Group ID
        }
        else if (globalID < tierCount[2] + tierCount[1] + tierCount[0])
        {
            _BD.Hierachy = 2;
            _BD.GlobalID = globalID; //Set global ID
            _BD.GroupID = globalID - (tierCount[1] + 1); //Set Group ID
        }
        else if (globalID < tierCount[3] + tierCount[2] + tierCount[1] + tierCount[0])
        {
            _BD.Hierachy = 3;
            _BD.GlobalID = globalID; //Set global ID
            _BD.GroupID = globalID - (tierCount[2] + 1); //Set Group ID
        }
    }

    private void InitBranchPos(int globalID)
    {
        BranchData _BD = BranchTransforms[globalID].GetComponent<BranchData>();
        ReturnBranchParent(_BD); //BRANCHPARENT

        BranchTransforms[globalID].transform.position = ReturnBranchParent(_BD).transform.position;
    }

    private GameObject ReturnBranchParent(BranchData _BD)
    {
        if (_BD.Hierachy > 0)
            _BD.ParentGroupID = _BD.GroupID / (tierCount[_BD.Hierachy] / tierCount[_BD.Hierachy - 1]);

        foreach (GameObject otherBranch in BranchTransforms)
        {
            BranchData _OtherBD = otherBranch.GetComponent<BranchData>();
            if (_OtherBD.Hierachy == _BD.Hierachy - 1 &&    //if Hierachy is one up ^
                _OtherBD.GroupID == _BD.ParentGroupID)  //and others group ID == parent groupID
            {
                return _OtherBD.gameObject;
            }
        }
        print("null parent");
        return gameObject;
    }

    private void InitBranchSpline(int hierachy)
    {
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapTile : MonoBehaviour
{
    public int treeNum; //_GD.density
    public float tileSize;
    private GameData tileData;
    private PlaceBranches drawTree;
    private List<GameObject> treeSpawn = new List<GameObject>();
    private GameObject tree;

    public int seed = 0;

    private int xIndex = 0;
    private int yIndex = 0;

    // Use this for initialization
    private void Start()
    {
        tree = Resources.Load("Tree") as GameObject;
        drawTree = tree.GetComponent<PlaceBranches>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void BuildTrees()
    {
        tileData = GetComponent<GameData>();
        for (int i = 0; i < tileData.density; i++)
        {
            GameObject tempTree = Instantiate(tree) as GameObject;
            treeSpawn.Add(tempTree);
            tempTree.transform.SetParent(this.transform);
            tempTree.GetComponent<DrawBranches>()._GD = tileData;
            tempTree.GetComponent<PlaceBranches>()._GD = tileData;
            tempTree.GetComponent<PlaceBranches>().BuildTree();
            tempTree.transform.localPosition = RaycastPointInTile();
        }
    }

    public Vector3 RaycastPointInTile()
    {
        System.Random rndSeed = new System.Random(seed);
        float xPos = rndSeed.Next((int)-(tileSize / 2), (int)(tileSize / 2));
        float zPos = rndSeed.Next((int)-(tileSize / 2), (int)(tileSize / 2));
        seed++;
        // this.transform.position = cam.ScreenToWorldPoint((Input.mousePosition));

        Vector3 origin = new Vector3(xPos, 500.0f, zPos);
        RaycastHit hit;

        Debug.DrawRay(origin, -transform.up, Color.red);
        // RaycastHit hit;

        if (Physics.Raycast(origin, -Vector3.up, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Terrain")))
        {
            //print(hit.point);
            return hit.point;
        }
        return Vector3.zero;
    }

    public void SetMapIndex(int x, int y)
    {
        xIndex = x;
        yIndex = y;
    }

    public int GetMapX()
    {
        return xIndex;
    }

    public int GetMapY()
    {
        return yIndex;
    }

    public void SetGameData(GameData _GD)
    {
        tileData = _GD;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapTile : MonoBehaviour
{
    public int treeNum; //_GD.density
    public float tileSize;
    private GameData tileData;
    private PlaceBranches drawTree;
    private DrawBranches drawBranch;
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
        drawBranch = tree.GetComponent<DrawBranches>();

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
            tileData._treeType = (TreeType)(int)drawBranch.GetRandomNumInRange(0, 3);
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

        if (Physics.Raycast(origin, -Vector3.up, out hit, 2000.0f, 1 << LayerMask.NameToLayer("Terrain")))
        {
            //print(hit.point);
            Debug.DrawRay(hit.point, transform.up, Color.magenta);

            return hit.point;
        }
        print("not found");
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
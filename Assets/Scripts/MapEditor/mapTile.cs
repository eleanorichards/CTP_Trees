using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapTile : MonoBehaviour
{
    public float tileSize;
    private GameData tileData;
    private PlaceBranches drawTree;
    private DrawBranches drawBranch;
    private List<GameObject> treeSpawn = new List<GameObject>();
    private GameObject tree;

    [HideInInspector]
    public int seed = 0;

    private int xIndex = 0;
    private int yIndex = 0;
    public GameObject plane;
    private Renderer planeRend;

    public Material[] tileSprites; //0 = clear
                                   //1 = hovered
                                   //2 = set

    // Use this for initialization
    private void Start()
    {
        tree = Resources.Load("Tree") as GameObject;
        drawTree = tree.GetComponent<PlaceBranches>();
        drawBranch = tree.GetComponent<DrawBranches>();
        planeRend = plane.GetComponent<Renderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        // SetToDefault();
    }

    public void BuildTrees()
    {
        tileData = GetComponent<GameData>();
        for (int i = 0; i < tileData.density; i++)
        {
            tileData._treeType = (TreeType)(int)drawBranch.GetRandomNumInRange(0, 3);
            print(tileData._treeType);
            GameObject tempTree = Instantiate(tree) as GameObject;
            treeSpawn.Add(tempTree);
            tempTree.transform.SetParent(this.transform);
            tempTree.GetComponent<DrawBranches>()._GD = tileData;
            tempTree.GetComponent<PlaceBranches>()._GD = tileData;
            tempTree.GetComponent<PlaceBranches>().BuildTree();
            tempTree.transform.position = RaycastPointInTile();
        }
    }

    public Vector3 RaycastPointInTile()
    {
        System.Random rndSeed = new System.Random(seed);
        float xPos = rndSeed.Next((int)-(tileSize / 2), (int)(tileSize / 2));
        float zPos = rndSeed.Next((int)-(tileSize / 2), (int)(tileSize / 2));
        seed++;

        Vector3 origin = new Vector3(xPos, 500.0f, zPos) + transform.position; //random loc within tile + tile's transform
        RaycastHit hit;

        Debug.DrawRay(origin, -transform.up, Color.red);
        // RaycastHit hit;

        if (Physics.Raycast(origin, -Vector3.up, out hit, 2000.0f, 1 << LayerMask.NameToLayer("Terrain")))
        {
            Debug.DrawLine(hit.point, hit.point + Vector3.up);
            return hit.point;
        }
        //map either not marked as terrain, or scale values incorrect
        print("not found");
        return Vector3.zero;
    }

    public void SetToDefault()
    {
        if (planeRend.material != tileSprites[0])
        {
            StartCoroutine(ResetMat(0.5f));

            planeRend.material = tileSprites[0];
        }
    }

    private IEnumerator ResetMat(float secs)
    {
        yield return new WaitForSeconds(secs);
    }

    public void SetToHover()
    {
        if (planeRend.material != tileSprites[1] /*&& planeRend.material != tileSprites[1]*/)
            planeRend.material = tileSprites[1];
    }

    public void SetToClicked()
    {
        if (planeRend.material != tileSprites[2])
            planeRend.material = tileSprites[2];
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
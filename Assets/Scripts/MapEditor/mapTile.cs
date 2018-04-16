using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapTile : MonoBehaviour
{
    public int treeNum; //_GD.density
    public int tileSize;
    public GameData tileData;
    private PlaceBranches drawBranch;
    private List<GameObject> treeSpawn = new List<GameObject>();
    public int seed = 0;

    // Use this for initialization
    private void Start()
    {
        GameObject tree = Resources.Load("Tree") as GameObject;
        tree.transform.SetParent(transform);
        drawBranch = tree.GetComponent<PlaceBranches>();
    }

    // Update is called once per frame
    private void Update()
    {
        System.Random rndSeed = new System.Random(seed);
        float xPos = rndSeed.Next((int)transform.position.x - (tileSize / 2), (int)transform.position.x + (tileSize / 2));
        float zPos = rndSeed.Next((int)transform.position.z - (tileSize / 2), (int)transform.position.z + (tileSize / 2));
        drawBranch.BuildTree();
    }
}
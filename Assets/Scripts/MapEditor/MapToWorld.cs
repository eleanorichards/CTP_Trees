using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapToWorld : MonoBehaviour
{
    [Header("Tiling dimensions")]
    public int height;

    public int width;

    [Header("Terrain links")]
    [Tooltip("drag in Unity terrain obj (tagged terrain)")]
    public Terrain terrain;

    [Tooltip("drag in Unity gameObject (tagged terrain)")]
    public GameObject terrain2;

    private GameObject tilePrefab;
    private int seed = 0;
    private float terrHeight;
    private float terrWidth;
    private mapTile tileData;
    private List<GameObject> mapTiles = new List<GameObject>();

    // Use this for initialization
    private void Start()
    {
        if (terrain)
        {
            terrWidth = terrain.terrainData.heightmapWidth;
            terrHeight = terrain.terrainData.heightmapWidth;
        }
        else if (terrain2)
        {
            terrWidth = terrain2.GetComponent<Renderer>().bounds.size.x;
            terrHeight = terrain2.GetComponent<Renderer>().bounds.size.z;
        }

        tilePrefab = Resources.Load("GridZone") as GameObject;
        transform.position = new Vector3(0, 0, 0);

        InitTileSet();
    }

    public void InitTileSet()
    {
        mapTiles.Clear();
        int tileNo = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject tile = Instantiate(tilePrefab);

                tileData = tile.GetComponent<mapTile>();

                mapTiles.Add(tile);
                tileData.SetMapIndex(x, y);
                tileData.tileSize = (terrWidth / width);
                seed++;
                tileData.seed = seed;
                tile.transform.SetParent(this.transform); //Set this map fill obj as parent
            }
        }
        for (float x = (-terrWidth / 2) + (terrWidth / width) / 2; x <= (terrWidth / 2); x += (terrWidth / (width)))
        {
            for (float y = (-terrHeight / 2) + (terrHeight / height) / 2; y <= (terrHeight / 2); y += (terrHeight / (height)))
            {
                mapTiles[tileNo].transform.position = new Vector3(x, 0, y) + transform.position; //setLocations + mapGeneratorPos

                tileNo++;
            }
        }
    }

    public void DrawTrees()
    {
        foreach (GameObject tile in mapTiles)
        {
            tile.GetComponent<mapTile>().BuildTrees();
        }
    }
}
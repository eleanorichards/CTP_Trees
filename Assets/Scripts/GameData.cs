using UnityEngine;

public enum GameState
{
    PLAY,
    PAUSE,
    MENU
}

public enum TreeHeading
{
    NONE,
    NORTH,
    EAST,
    SOUTH,
    WEST
}

public enum TreeType
{
    DISSECANT,
    FIBBONACI,
    QUAD
}

public enum BranchHier
{
    FIRST,
    SECOND,
    THIRD,
    FOURTH
}

public enum windDirection
{
    NONE,
    NORTH,
    EAST,
    SOUTH,
    WEST
}


public class GameData : MonoBehaviour
{
    public TreeType _treeType;
    public BranchHier _branchType;

    // Use this for initialization
    private void Start()
    {
        _treeType = TreeType.DISSECANT;
        _branchType = BranchHier.FIRST;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}
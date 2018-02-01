using UnityEngine;

public enum BranchHier
{
    FIRST,
    SECOND,
    THIRD,
    FOURTH
}

public enum GameState
{
    PLAY,
    PAUSE,
    MENU
}

public enum TreeType
{
    DICHOTOMOUS, //branching in to two equal parts
    SYMPODIAL,  //branching in to many lateral (can be uneven) parts
    MONOPODIAL, //branching from a single trunk
    WHORLED     //spiral/fibbonacci? from previous branch
}

public enum WindHeading
{
    NONE,
    NORTH,
    EAST,
    SOUTH,
    WEST
}

public class GameData : MonoBehaviour
{
    public BranchHier _branchType;
    public int _treeNum = 1;
    public TreeType _treeType;
    public WindHeading _windHeading;
    public float _windSpeed = 0.0f;
    public float _sunStrength = 1.0f;
    public float _tangliness = 0.0f;

    // Use this for initialization
    private void Start()
    {
        _windHeading = WindHeading.NONE;
        _treeType = TreeType.DICHOTOMOUS;
        _branchType = BranchHier.FIRST;
    }
}
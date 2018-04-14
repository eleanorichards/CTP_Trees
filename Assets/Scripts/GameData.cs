using UnityEngine;

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
    WHORLED,     //spiral/fibbonacci? from previous branch
    WEEPING
}

public enum WindHeading
{
    NONE,
    NORTH,
    EAST,
    SOUTH,
    WEST
}

public enum LightHeading
{
    NONE,
    NORTH,
    EAST,
    SOUTH,
    WEST
}

public enum TempZones
{
    NONE,
    TEMPERATE, //constant rain, stable temp
    TROPICAL    //erratic highish temp, rain & drought
}

public class GameData : MonoBehaviour
{
    public int _treeNum = 1;
    public TreeType _treeType;
    public WindHeading _windHeading;
    public LightHeading _lightHeading;
    public TempZones _tempZones;

    public float _windSpeed = 0.0f;
    public float _sunStrength = 1.0f;
    public float _avgTemp = 0.0f;
    public float _avgPrecip = 0.0f;
    public float _tangliness = 0.1f;
    public float density = 0.0f;
    public bool leaves = false;

    // Use this for initialization
    private void Start()
    {
        _windHeading = WindHeading.NONE;
        _treeType = TreeType.SYMPODIAL;
        _tempZones = TempZones.NONE;
    }
}
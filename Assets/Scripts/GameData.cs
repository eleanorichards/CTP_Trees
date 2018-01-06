using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState
{
    PLAY,
    PAUSE,
    MENU
}
//GameState _GS = GameState.PLAY;

public enum ViewState
{
    LINERENDER,
    SEGMENTPLACER,
    NODESONLY
}

public enum TreeType
{
    DISSECANT,
    FIBBONACI,
    DROOPY
}

public enum BranchHier
{
    FIRST,
    SECOND,
    THIRD,
    FOURTH
}

public class GameData : MonoBehaviour
{
    public ViewState _viewState;
    public TreeType _treeType;
    public BranchHier _branchType;

    // Use this for initialization
    void Start ()
    {
        _viewState = ViewState.LINERENDER;
        _treeType = TreeType.DISSECANT;
        _branchType = BranchHier.FIRST;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

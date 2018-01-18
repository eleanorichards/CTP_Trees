using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Dropdown windHeadingDD;
    public Dropdown treeTypeDD;

    public Slider windSpeedSlider;
    public Slider treeNumSlider;
    public Slider tanglinessSlider;

    private GameData _GD;

    private WindHeading windHeading;
    private TreeType treeType;

    // Use this for initialization
    private void Start()
    {
        _GD = GetComponent<GameData>();

        windHeading = _GD._windHeading;
        treeType = _GD._treeType;

        PopulateWindList();
        PopulateTreeList();
    }

    //WIND HEADING DD
    public void WindIndexChanged(int index)
    {
        _GD._windHeading = (WindHeading)index;
    }

    private void PopulateWindList()
    {
        string[] headingNames = WindHeading.GetNames(typeof(WindHeading));
        List<string> windNames = new List<string>(headingNames);
        windHeadingDD.AddOptions(windNames);
    }

    //WIND HEADING DD
    public void TreeTypeIndexChanged(int index)
    {
        _GD._treeType = (TreeType)index;
    }

    private void PopulateTreeList()
    {
        string[] typeNames = TreeType.GetNames(typeof(TreeType));
        List<string> treeNames = new List<string>(typeNames);
        treeTypeDD.AddOptions(treeNames);
    }

    //SLIDERS

    //WINDSPEED
    public void WindSliderChanged()
    {
        _GD._windSpeed = windSpeedSlider.value;
    }

    //TREENUM
    public void TreeNumSliderChanged()
    {
        _GD._treeNum = (int)treeNumSlider.value;
    }

    //TANGLINESS
    public void TanglinessSliderChanged()
    {
        _GD._tangliness = tanglinessSlider.value;
    }
}
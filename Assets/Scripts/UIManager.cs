using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Dropdown windHeadingDD;
    public Dropdown treeTypeDD;
    public Dropdown lightHeadingDD;
    public Dropdown tempDD;

    public Slider windSpeedSlider;
    public Slider sunStrengthSlider;
    public Slider tanglinessSlider;
    public Slider tempSlider;
    public Slider precipSlider;
    public Slider densitySlider;

    public Toggle leavesToggle;

    private GameData _GD;

    private WindHeading windHeading;
    private LightHeading lightHeading;
    private TreeType treeType;
    private TempZones tempZone;

    // Use this for initialization
    private void Start()
    {
        _GD = GetComponent<GameData>();

        windHeading = _GD._windHeading;
        lightHeading = _GD._lightHeading;
        treeType = _GD._treeType;
        tempZone = _GD._tempZones;

        PopulateWindList();
        PopulateLightList();
        PopulateTreeList();
        PopulateTempList();
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

    //LIGHT HEADING DD
    public void LightIndexChanged(int index)
    {
        _GD._lightHeading = (LightHeading)index;
    }

    private void PopulateLightList()
    {
        string[] headingNames = LightHeading.GetNames(typeof(LightHeading));
        List<string> lightNames = new List<string>(headingNames);
        lightHeadingDD.AddOptions(lightNames);
    }

    //TEMP HEADING DD
    public void TempIndexChanged(int index)
    {
        _GD._tempZones = (TempZones)index;
    }

    private void PopulateTempList()
    {
        string[] headingNames = TempZones.GetNames(typeof(TempZones));
        List<string> tempNames = new List<string>(headingNames);
        tempDD.AddOptions(tempNames);
    }


    //SLIDERS

    //PRECIP
    public void DensitySliderChanged()
    {
        _GD.density = densitySlider.value;
    }

    //WINDSPEED
    public void WindSliderChanged()
    {
        _GD._windSpeed = windSpeedSlider.value;
    }

    //PRECIP
    public void PrecipSliderChanged()
    {
        _GD._avgPrecip = precipSlider.value;
    }

    //TEMP
    public void TempSliderChanged()
    {
        _GD._avgTemp = tempSlider.value;
    }

    //SUN STRENGTH
    public void SunStrengthSliderChanged()
    {
        _GD._sunStrength = sunStrengthSlider.value;
    }

    //TANGLINESS
    public void TanglinessSliderChanged()
    {
        _GD._tangliness = tanglinessSlider.value;
    }

    //LEAVES
    public void HasLeaves(bool _leaves)
    {
        _GD.leaves = _leaves;
        //return _leaves;
    }

  
}
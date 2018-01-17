using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    
    public Dropdown headingDropdown;
    public Dropdown windDropdown;

    private TreeHeading treeHeading;
    
    // Use this for initialization
    void Start ()
    {
        treeHeading = TreeHeading.NONE;
        PopulateList();
    }

   
    public void DropdownIndexChanged(int index)
    {
        TreeHeading treeHeading = (TreeHeading)index;
        Debug.Log(treeHeading);      
    }

    void PopulateList()
    {
        string[] headingNames = TreeHeading.GetNames(typeof(TreeHeading));
        List<string> names = new List<string>(headingNames);
        headingDropdown.AddOptions(names);
    }
}

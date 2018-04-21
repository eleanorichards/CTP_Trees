using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    private Camera cam;
    private MapToWorld map;
    private GameData _GD;

    // Use this for initialization
    private void Start()
    {
        map = GameObject.Find("Plane").GetComponent<MapToWorld>();
        _GD = GetComponent<GameData>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    //MOUSE CONTROL

    private void FixedUpdate()
    {
        this.transform.position = cam.ScreenToWorldPoint((Input.mousePosition));

        RaycastHit hit;

        Debug.DrawRay(transform.position, -transform.up, Color.red);
        // RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Tile")))
        {
            transform.position = hit.collider.transform.position;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                hit.collider.transform.GetComponentInChildren<Light>().color = Color.red;
                CopyComponent(_GD, hit.transform.gameObject);
                // hit.transform.GetComponent<mapTile>().SetGameData(_GD);
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            map.DrawTrees();
        }
    }

    private Component CopyComponent(Component original, GameObject destination)
    {
        System.Type type = original.GetType();
        Component copy = destination.GetComponent(type);
        // Copied fields can be restricted with BindingFlags
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy;
    }
}
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
        _GD = GetComponent<GameData>();
        map = GameObject.Find("MapGen").GetComponent<MapToWorld>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    //MOUSE CONTROL
    private void FixedUpdate()
    {
        transform.position = cam.ScreenToWorldPoint((Input.mousePosition));

        RaycastHit hit;

        Debug.DrawRay(transform.position, -transform.up, Color.red);
        // RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Tile")))
        {
            transform.position = hit.collider.transform.position;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                CopyComponent(_GD, hit.transform.gameObject);
                hit.transform.GetComponent<mapTile>().SetToClicked();
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), -transform.up, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("UI")))
            {
                print("UI HIT");
            }
        }
        if (Input.GetButtonDown("Jump"))
        {
            print("constructing...");
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
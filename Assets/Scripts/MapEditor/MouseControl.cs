using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    private Camera cam;
    private MapToWorld map;

    // Use this for initialization
    private void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    //MOUSE CONTROL

    private void FixedUpdate()
    {
        //new Vector3(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y, 0);
        //Vector3 origin = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, Camera.main.nearClipPlane);
        Vector3 origin = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));

        Ray ray = cam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.farClipPlane));

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground"));
        Debug.DrawRay(origin, ray.direction, Color.red);
        // RaycastHit hit;

        if (hit)
        {
            if (transform.localPosition.x < map.width && transform.localPosition.y < map.height && transform.localPosition.x >= 0 && transform.localPosition.y >= 0)
            {
                transform.position = hit.collider.transform.position;
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    Debug.Log(hit.collider.transform.localPosition);
                    map.SetMapTile((int)hit.collider.transform.localPosition.x, (int)hit.collider.transform.localPosition.y);
                }
            }
        }
    }
}
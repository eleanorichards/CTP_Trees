using UnityEngine;

public class Movement : MonoBehaviour
{
    public float lookSpeedH = 5f;
    public float lookSpeedV = 5f;
    public float zoomSpeed = 5f;
    public float dragSpeed = 10f;

    public float rotate_speed = 10.0f;
    public float move_speed = 10.0f;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * move_speed;
        float vertical = Input.GetAxis("Vertical") * Time.deltaTime * move_speed;

        MouseRotation();
    }

    private void MouseRotation()
    {
        float yaw = 0f;
        float pitch = 0f;

        //Look around with Right Mouse
        if (Input.GetMouseButton(1))
        {
            pitch -= lookSpeedV * Input.GetAxis("Mouse Y");
            yaw += lookSpeedH * Input.GetAxis("Mouse X");

            transform.eulerAngles += new Vector3(0, yaw, pitch);
        }

        //drag camera around with Middle Mouse
        if (Input.GetMouseButton(2))
        {
            //transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed, -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * dragSpeed, 0);
            transform.position += new Vector3(0, -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * dragSpeed, -Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed);
        }

        //Zoom in and out with Mouse Wheel
        transform.Translate(-Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, 0, 0, Space.Self);
    }
}
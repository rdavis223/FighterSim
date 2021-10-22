using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightController : MonoBehaviour
{
    public float forwardSpeed = 25f, lookSpeed = 1f, rollSpeed = 0.2f;
    public float forwardAcceleration =25f, lookAcceleration = 7.5f, rollAcceleration= 3.5f;
    public float activeForwardSpeed, active;
    public float lookRate = 90f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float forward = Input.GetAxisRaw("Vertical") * forwardSpeed;
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;
        float roll = Input.GetAxis("Horizontal") * rollSpeed * Time.deltaTime;
        this.transform.position += this.transform.forward * forward * Time.deltaTime;
        transform.Rotate(mouseY, mouseX, roll, Space.Self);     
    }
}

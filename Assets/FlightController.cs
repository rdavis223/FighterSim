using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FlightController : MonoBehaviour
{
    public float maxForwardSpeed = 25f, lookSpeed = 1f, rollSpeed = 0.2f;
    private float forwardSpeed;
    public float throttleSpeed = 1.3f;
    public float forwardAcceleration =25f, lookAcceleration = 7.5f, rollAcceleration= 3.5f;
    public float lookRate = 90f;
    public Image throttleUI;
    CharacterController rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalStateMgr.canMove())
        {
            forwardSpeed += Input.GetAxisRaw("Vertical") * throttleSpeed;
            forwardSpeed = Mathf.Clamp(forwardSpeed, 0f, maxForwardSpeed);
            throttleUI.fillAmount = forwardSpeed / maxForwardSpeed;
            float forward = forwardSpeed;
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;
            float roll = Input.GetAxis("Horizontal") * rollSpeed * Time.deltaTime;
            rigidbody.Move(this.transform.forward * Mathf.Clamp(forward * Time.deltaTime, 0f, 400f));
            //this.transform.position += this.transform.forward * Mathf.Clamp(forward * Time.deltaTime, 0f, 400f);
            transform.Rotate(mouseY, mouseX, roll, Space.Self);
        }
    }
}

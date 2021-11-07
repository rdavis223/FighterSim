using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroidMovement : MonoBehaviour
{
    private float rotateX;
    private float rotateY;
    private float rotateZ;

    private float rotateSpeed;
    private void Start()
    {
        rotateX = Random.Range(0f, 1f);
        rotateY = Random.Range(0f, 1f);
        rotateZ = Random.Range(0f, 1f);
        rotateSpeed = Random.Range(10f, 40f);

    }

    private void Update()
    {
        this.transform.Rotate(rotateX * Time.deltaTime * rotateSpeed, rotateY * Time.deltaTime * rotateSpeed, rotateZ * Time.deltaTime * rotateSpeed);
    }
}

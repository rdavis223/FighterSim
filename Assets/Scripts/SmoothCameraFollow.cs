using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    GameObject playerTarget;
    public float smoothRate = 0.0125f;

    private void Start()
    {
        playerTarget = GameObject.Find("Target");
    }
    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 smoothedPos = Vector3.Lerp(this.transform.position, playerTarget.transform.position, smoothRate * Time.deltaTime);
        Quaternion smoothedRot = Quaternion.Lerp(this.transform.rotation, playerTarget.transform.rotation, smoothRate * Time.deltaTime);
        this.transform.position = smoothedPos;
        this.transform.rotation = smoothedRot;
    }
}

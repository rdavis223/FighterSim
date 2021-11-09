using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAICommon : MonoBehaviour
{
    protected float speed = 0;
    public float turnSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    protected bool isAtPoint(Vector3 pos, Vector3 destination)
    {
        return Vector3.Distance(pos, destination) <= 1f;
    }

    protected void flyInDirection(Vector3 point)
    {
        this.transform.position += point.normalized * speed * Time.deltaTime;

    }
    protected void flyForward()
    {
        this.transform.position += this.transform.forward * speed * Time.deltaTime;
    }
    protected void flyTowardsPoint(Vector3 point)
    {
        flyInDirection(point - this.transform.position);
    }

    protected void setSpeed(float s)
    {
        speed = s;
    }

    protected void turnTowardsVector(Quaternion rot)
    {
        if (this.transform.rotation != rot)
        {
            this.transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
        }
    }

    public float getSpeed()
    {
        return speed;
    }
}

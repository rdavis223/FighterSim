using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAICommon : MonoBehaviour
{
    protected float speed = 0;
    public float turnSpeed = 1f;

    protected bool destSet;
    public float range;
    protected Quaternion dir;
    protected Vector3 dest;
    protected bool dodging = false;
    public LayerMask wall;
    protected bool clearingBoundry = false;

    protected GameObject attachedEnemy;



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

    public virtual void initiateDetach()
    {

    }

    protected void Patrol()
    {
        if (!destSet)
        {
            dest = new Vector3(Random.Range(0, range), Random.Range(0, range), Random.Range(0, range));
            dest = vectorClamp(dest);
            destSet = true;
            dir = Quaternion.LookRotation(dest - this.transform.position);

        }
        if (isAtPoint(this.transform.position, dest))
        {
            destSet = false;
        }
        else
        {
            turnTowardsVector(dir);
            flyInDirection(dest - this.transform.position);
        }
    }

    protected Vector3 vectorClamp(Vector3 pos)
    {
        pos.x = Mathf.Clamp(pos.x, 0f, 1500f);
        pos.y = Mathf.Clamp(pos.y, 0f, 1500f);
        pos.z = Mathf.Clamp(pos.x, 0f, 1500f);
        return pos;

    }

    protected void dodgeObject()
    {
        turnTowardsVector(dir);
        flyTowardsPoint(dest);
        if (isAtPoint(this.transform.position, dest))
        {
            clearingBoundry = false;
            dodging = nextDodge();
        }
    }

    protected void setDodgeObject(Vector3 currentDir, float angle, float range, int depth = 3)
    {
        Vector3 newDirection = Quaternion.Euler(angle, 0f, 0f) * currentDir;
        dest = this.transform.position + (newDirection.normalized * range);
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, newDirection, out hit, (dest - this.transform.position).magnitude))
        {
            if (hit.transform.gameObject.tag == "Station" || hit.transform.gameObject.tag == "Enemy" || hit.transform.gameObject.tag == "Ast")
            {
                if (depth > 0)
                {
                    setDodgeObject(newDirection, angle, range, depth - 1);
                    return;
                }
            }

            if (hit.transform.gameObject.tag == "Wall")
            {
                dest = hit.point;
            }
        }
        dir = Quaternion.LookRotation(newDirection);
        dodging = true;
    }

    public virtual void detectObject(Collider collision)
    {

    }

    protected virtual bool nextDodge()
    {
        return false;
    }

    protected virtual void boundryHardCheck()
    {
        if (!clearingBoundry)
        {
            if (this.transform.position.x > range + 10f || this.transform.position.y > range + 10f || this.transform.position.z > range + 10f || this.transform.position.x < -10f || this.transform.position.y < -10f || this.transform.position.z < -10f)
            {
                Vector3 newDirection = new Vector3(750f, 750f, 750f) - this.transform.position;
                dest = this.transform.position + (newDirection.normalized * 40f);
                dir = Quaternion.LookRotation(newDirection);
                dodging = true;
            }
        }
    }

    public GameObject getAttachedEnemy()
    {
        return attachedEnemy;
    }

}

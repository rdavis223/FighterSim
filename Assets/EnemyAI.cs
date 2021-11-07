using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float range;
    private bool destSet;
    private Vector3 dest;
    public float attackSpeed;
    public float flySpeed;
    private Quaternion dir;
    public float turnSpeed = 1f;

    private float speed;

    public GameObject player;
    public float playerAttackRange = 600f;
    private bool dodging = false;
    private EnemyShooting shootControl;
    public bool testing = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        shootControl = GetComponent<EnemyShooting>();
    }

    // Update is called once per frame
    void Update()
    {
        if (testing)
        {
            executeTestScript();
            return;
        }
        if (dodging)
        {
            setSpeed(flySpeed);
            dodgeObject();
        }
        else if (distanceToPlayer() < playerAttackRange)
        {
            destSet = false;
            setSpeed(attackSpeed);
            attackPlayer();
        }
        else
        {
            setSpeed(flySpeed);
            Patrol();
        }
        
    }

    void executeTestScript()
    {
        if (dodging)
        {
            dodgeObject();
        } else
        {
            flyForward();
        }

    }
    void Patrol()
    {
        if (!destSet)
        {
            dest = new Vector3(Random.Range(0, range), Random.Range(0, range), Random.Range(0, range));
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

    void attackPlayer()
    {
        flyTowardsPlayer();
    }

    void flyTowardsPlayer()
    {
        Quaternion playerRot = Quaternion.LookRotation(player.transform.position - this.transform.position);
        if (playerRot == this.transform.rotation)
        {
            shootControl.shootPrimary();
        }
        turnTowardsVector(playerRot);
        if (distanceToPlayer() > 30f)
        {
            flyForward();
        }
        else
        {
            int[] vals = new int[]  { -1, 1 };
            int randIndex = Random.Range(0, 1);

            setDodgeObject((player.transform.position - this.transform.position).normalized, Random.Range(55f, 80f) * vals[randIndex], Random.Range(150f, 400f));
        }
    }

    void dodgeObject()
    {
        turnTowardsVector(dir);
        flyTowardsPoint(dest);
        if (isAtPoint(this.transform.position, dest))
        {
            dodging = false;
        }
    }

    void setDodgeObject(Vector3 currentDir, float angle, float range)
    {
        Vector3 newDirection = Quaternion.Euler(angle, 0f, 0f) * currentDir;
        dest = this.transform.position + (newDirection.normalized * range);
        dir = Quaternion.LookRotation(newDirection);
        dodging = true;
    }
    void turnTowardsVector(Quaternion rot)
    {
        if (this.transform.rotation != rot)
        {
            this.transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
        }
    }

    void flyInDirection(Vector3 point)
    {
        this.transform.position += point.normalized * speed * Time.deltaTime;

    }
    void flyForward()
    {
        this.transform.position += this.transform.forward * speed * Time.deltaTime;

    }
    void flyTowardsPoint(Vector3 point)
    {
        flyInDirection(point - this.transform.position);
    }
 float distanceToPlayer()
    {
        return Vector3.Distance(this.transform.position, player.transform.position);
    }
   

    bool isAtPoint(Vector3 pos, Vector3 destination)
    {
        return Vector3.Distance(this.transform.position, dest) <= 1f;
    }

    public void detectObject(Collider collision)
    {
        if (collision.gameObject.tag == "Station")
        {
            setDodgeObject(this.transform.forward, 90, 50f);
            //handle other enemy collision
        }
    }

    void setSpeed(float s)
    {
        speed = s;
    }


}

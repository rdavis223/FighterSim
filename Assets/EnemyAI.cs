using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float range;
    private bool destSet;
    private Vector3 dest;
    public float speed;
    private Quaternion dir;
    public float turnSpeed = 1f;

    public GameObject player;
    public float playerAttackRange = 600f;
    public bool dodging = false;
    private EnemyShooting shootControl;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        shootControl = GetComponent<EnemyShooting>();
    }

    // Update is called once per frame
    void Update()
    {
        if (distanceToPlayer() < playerAttackRange)
        {
            destSet = false;
            attackPlayer();
        }
        else
        {
            Patrol();
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
        if (dodging)
        {
            dodgePlayer();
        }
        else
        {
            flyTowardsPlayer();
        }
    }

    void flyTowardsPlayer()
    {
        Vector3 playerDir = (player.transform.position - this.transform.position).normalized;
        Quaternion playerRot = Quaternion.LookRotation(player.transform.position - this.transform.position);
        if (playerRot == this.transform.rotation)
        {
            shootControl.shootPrimary();
        }
        turnTowardsVector(playerRot);
        if (distanceToPlayer() > 30f)
        {
            flyInDirection(playerDir);
        }
        else
        {
            Vector3 newDirection = Quaternion.Euler(Random.Range(30f, 50f), 0f, 0f) * playerDir;
            Debug.Log(this.transform.position);
            dest = this.transform.position + (newDirection.normalized * Random.Range(150f, 400f));
            Debug.Log(dest);
            dir = Quaternion.LookRotation(newDirection);
            dodging = true;
        }
    }

    void dodgePlayer()
    {
        turnTowardsVector(dir);
        flyTowardsPoint(dest);
        if (isAtPoint(this.transform.position, dest))
        {
            dodging = false;
        }
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


}

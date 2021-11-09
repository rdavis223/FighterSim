using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIMain : EnemyAICommon
{
    public float range;
    private bool destSet;
    private Vector3 dest;
    public float attackSpeed;
    public float flySpeed;
    private Quaternion dir;

    public LayerMask wall;

    public GameObject player;
    public float playerAttackRange = 600f;
    private bool dodging = false;
    private EnemyShooting shootControl;
    public bool testing = false;

    private bool hasHealer = false;
    private GameObject healer = null;
    private bool healerIsAttached = false;

    public GameObject[] attachPos;

    EnemyHealthMgr healthMgr;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        shootControl = GetComponent<EnemyShooting>();
        healthMgr = GetComponent<EnemyHealthMgr>();
        GameObject attachPosParent = this.transform.Find("AttachPositions").gameObject;
        attachPos = new GameObject[4];
        for(int i = 0; i < attachPosParent.transform.childCount; i++)
        {
            attachPos[i] = attachPosParent.transform.GetChild(i).gameObject;
        }
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
        setSpeed(10f);
        flyForward();
    }
    void Patrol()
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

    void attackPlayer()
    {
        if (player.transform.position.x > 1500f || player.transform.position.y > 1500f || player.transform.position.z > 1500f)
        {
            Patrol();
        }
        else
        {
            flyTowardsPlayer();
        }
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
            int randIndex = Random.Range(0, 2);
            float dodgeAngle = Random.Range(70f, 120f) * vals[randIndex];
            setDodgeObject((player.transform.position - this.transform.position).normalized, dodgeAngle, Random.Range(150f, 400f));
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
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, newDirection, out hit, (dest-this.transform.position).magnitude, wall))
        {
            Debug.Log("Did the thing");
            dest = hit.point;
        }
        dir = Quaternion.LookRotation(newDirection);
        dodging = true;
    }


    float distanceToPlayer()
    {
        return Vector3.Distance(this.transform.position, player.transform.position);
    }

    public void detectObject(Collider collision)
    {
        if (collision.gameObject.tag == "Station")
        {
            setDodgeObject(this.transform.forward, 90, 50f);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            setDodgeObject((collision.gameObject.transform.position - this.transform.position).normalized, 180f, Random.Range(10f, 40f));

        }
        if (collision.gameObject.tag == "Ast")
        {
            setDodgeObject((collision.gameObject.transform.position - this.transform.position).normalized, 70f, 12f);
        }
    }

    Vector3 vectorClamp(Vector3 pos)
    {
        pos.x = Mathf.Clamp(pos.x, 0f, 1500f);
        pos.y = Mathf.Clamp(pos.y, 0f, 1500f);
        pos.z = Mathf.Clamp(pos.x, 0f, 1500f);
        return pos;

    }

    public bool isAvailableForRepair()
    {
        return (!dodging && !hasHealer && healthMgr.getCurrentHealthPercent() < 100f);
    }

    public GameObject[] getRepairAttachPoint()
    {
        return attachPos;
    }

    public bool setHealer(GameObject healer)
    {
        if (!hasHealer)
        {
            hasHealer = true;
            this.healer = healer;
            return true;
        } else
        {
            return false;
        }
    }

    public void attachHealer()
    {
        healerIsAttached = true;
    }





}

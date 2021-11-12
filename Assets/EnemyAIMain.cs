using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIMain : EnemyAICommon
{
    
    public float attackSpeed;
    public float flySpeed;
    public float runSpeed;
    public GameObject player;
    public float playerAttackRange = 600f;
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
        if (!GlobalStateMgr.canMove())
        {
            return;
        }
        if (testing)
        {
            executeTestScript();
            return;
        }
        if (dodging)
        {
            setSpeed(runSpeed);
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
        if (dodging)
        {
            dodgeObject();
            return;
        }
        flyForward();
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
        if (distanceToPlayer() > 60f)
        {
            flyForward();
        }
        else
        {
            int[] vals = new int[]  { -1, 1 };
            int randIndex = Random.Range(0, 2);
            float dodgeAngle = Random.Range(75f, 120f) * vals[randIndex];
            setDodgeObject((player.transform.position - this.transform.position).normalized, dodgeAngle, Random.Range(150f, 400f));
        }
    }

    float distanceToPlayer()
    {
        return Vector3.Distance(this.transform.position, player.transform.position);
    }

    public override void detectObject(Collider collision)
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


    public bool isAvailableForRepair()
    {
        return (!dodging && !hasHealer && healthMgr.getCurrentHealthPercent() < 1f);
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

    public override void initiateDetach()
    {
        if (healer != null)
        {
            healer.GetComponent<EnemyAIHealer>().detach();
        }
        detach();
    }

    public void detach()
    {
        hasHealer = false;
        healer = null;
        healerIsAttached = false;
    }
    public void setDodgeSpeedDynamically()
    {
        if (distanceToPlayer() < 60f)
        {
            setSpeed(runSpeed);
        } else
        {
            setSpeed(flySpeed);
        }
    }







}

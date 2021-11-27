using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIHealer : EnemyAICommon
{
    bool enemySet = false;
    GameObject[] enemyAttachPos;
    bool findEnemyRunning = false;
    bool isAttached = false;
    GameObject attachPos;
    EnemyHealthMgr otherHealthMgr;
    EnemyAIMain otherEnemyAI;
    LineRenderer line;
    public float healerSpeed = 100f;
    private AudioSource healSound;
    // Start is called before the first frame update
    void Start()
    {
        healSound = GetComponent<AudioSource>();
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        setSpeed(healerSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GlobalStateMgr.canMove())
        {
            return;
        }
        if (dodging)
        {
            dodgeObject();
            return;
        }
        if (!findEnemyRunning && !enemySet)
        {
            StartCoroutine(findEnemyToHeal());
        } else if (findEnemyRunning && !enemySet)
        {
            Patrol();
        }
        else if (enemySet && isAttached)
        {
            continueHealing();
        }
        else
        {
            if (enemyAttachPos != null)
            {
                GameObject g = findClosestAttachPos();
                Vector3 closestAttachPos = g.transform.position;
                if (enemySet && !isAttached && !isAtPoint(this.transform.position, closestAttachPos))
                {
                    moveToEnemy(closestAttachPos);

                }
                else if (enemySet && !isAttached && isAtPoint(this.transform.position, closestAttachPos))
                {
                    startHealing(g);
                }
            }
        }

        if (GlobalStateMgr.isDead())
        {
            StopAllCoroutines();
        }

    }

    IEnumerator findEnemyToHeal()
    {
        findEnemyRunning = true;
        foreach (GameObject enemy in GlobalStateMgr.mainEnemyList)
        {
            if (enemy != null)
            {
                EnemyAIMain enemyAi = enemy.GetComponent<EnemyAIMain>();
                if (enemyAi.isAvailableForRepair())
                {
                    bool result = enemyAi.setHealer(this.gameObject);
                    if (result)
                    {
                        enemySet = true;
                        otherEnemyAI = enemyAi;
                        attachedEnemy = enemy;
                        enemyAttachPos = enemyAi.getRepairAttachPoint();
                        findEnemyRunning = false;
                        yield break;
                    }
                }
            }
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(findEnemyToHeal());
    }

    void moveToEnemy(Vector3 closestAttachPos)
    {
        Quaternion playerRot = Quaternion.LookRotation(closestAttachPos - this.transform.position);
        turnTowardsVector(playerRot);
        flyForward();
    }

    GameObject findClosestAttachPos()
    {
        GameObject bestPos = null;
        float minDist = -1;
        foreach (GameObject g in enemyAttachPos)
        {
            Vector3 pos = g.transform.position;
            if (minDist == -1)
            {
                bestPos = g;
                minDist = Vector3.Distance(this.transform.position, pos);
            } else
            {
                float newDist = Vector3.Distance(this.transform.position, pos);
                if (newDist < minDist)
                {
                    minDist = newDist;
                    bestPos = g;
                }
            }
        }
        return bestPos;
    }

    void startHealing(GameObject closest)
    {
        healSound.Play();
        isAttached = true;
        attachPos = closest;
        otherEnemyAI = attachedEnemy.GetComponent<EnemyAIMain>();
        otherEnemyAI.attachHealer();
        otherHealthMgr = attachedEnemy.GetComponent<EnemyHealthMgr>();
        line.enabled = true;
    }

    void continueHealing()
    {
        this.transform.rotation = attachedEnemy.transform.rotation;
        line.SetPosition(0, this.transform.position);
        line.SetPosition(1, attachedEnemy.transform.position);
        setSpeed(otherEnemyAI.getSpeed());
        flyTowardsPoint(attachPos.transform.position);
        otherHealthMgr.heal(Time.deltaTime * 3.5f);
        if (otherHealthMgr.getCurrentHealthPercent() >= 1f)
        {
            initiateDetach();
        }
    }

    public override void initiateDetach()
    {
        if (otherEnemyAI != null) {
            otherEnemyAI.detach();
        }
        
        this.detach();
    }
    public void detach()
    {
        healSound.Stop();
        enemySet = false;
        if (line != null)
        {
            line.enabled = false;
        }
        attachedEnemy = null;
        enemyAttachPos = null;
        findEnemyRunning = false;
        isAttached = false;
        attachPos = null;
        otherHealthMgr = null;
        otherEnemyAI = null;
        destSet = false;

    }

    public override void detectObject(Collider collision)
    {
        if (collision.gameObject.tag == "Station")
        {
            Debug.Log("Collision");
            setDodgeObject(this.transform.forward, 90, 50f);
            initiateDetach();
        }
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("EnemyCollide");
            if (collision.gameObject != attachedEnemy)
            {
                setDodgeObject((collision.gameObject.transform.position - this.transform.position).normalized, 180f, Random.Range(10f, 40f));
                initiateDetach();
            }
        }
        if (collision.gameObject.tag == "Ast")
        {
            setDodgeObject((collision.gameObject.transform.position - this.transform.position).normalized, 70f, 12f);
            initiateDetach();
        }
        if (collision.gameObject.tag == "player")
        {
            setDodgeObject((collision.gameObject.transform.position - this.transform.position).normalized, Random.Range(70f, 90f), 12f);

        }
    }
}

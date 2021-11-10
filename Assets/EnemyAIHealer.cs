using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIHealer : EnemyAICommon
{
    bool enemySet = false;
    GameObject currentEnemy = null;
    GameObject[] enemyAttachPos;
    bool findEnemyRunning = false;
    bool isAttached = false;
    GameObject attachPos;
    EnemyHealthMgr otherHealthMgr;
    EnemyAIMain otherEnemyAI;
    LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
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

    }

    IEnumerator findEnemyToHeal()
    {
        findEnemyRunning = true;
        foreach (GameObject enemy in GlobalStateMgr.mainEnemyList)
        {
            EnemyAIMain enemyAi = enemy.GetComponent<EnemyAIMain>();
            if (enemyAi.isAvailableForRepair())
            {
                bool result = enemyAi.setHealer(this.gameObject);
                if (result)
                {
                    enemySet = true;
                    otherEnemyAI = enemyAi;
                    currentEnemy = enemy;
                    enemyAttachPos = enemyAi.getRepairAttachPoint();
                    findEnemyRunning = false;
                    yield break;
                }
            }
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(findEnemyToHeal());
    }

    void moveToEnemy(Vector3 closestAttachPos)
    {
        setSpeed(20f);
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
        isAttached = true;
        attachPos = closest;
        otherEnemyAI = currentEnemy.GetComponent<EnemyAIMain>();
        otherEnemyAI.attachHealer();
        otherHealthMgr = currentEnemy.GetComponent<EnemyHealthMgr>();
        line.enabled = true;
    }

    void continueHealing()
    {
        this.transform.rotation = currentEnemy.transform.rotation;
        line.SetPosition(0, this.transform.position);
        line.SetPosition(1, currentEnemy.transform.position);
        setSpeed(otherEnemyAI.getSpeed());
        flyForward();
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
        enemySet = false;
        line.enabled = false;
        currentEnemy = null;
        enemyAttachPos = null;
        findEnemyRunning = false;
        isAttached = false;
        attachPos = null;
        otherHealthMgr = null;
        otherEnemyAI = null;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalStateMgr
{
    public static bool dead = false;

    public static List<GameObject> mainEnemyList = new List<GameObject>();
    public static List<GameObject> healerEnemyList = new List<GameObject>();
    public static bool isDead()
    {
        return dead;
    }

    public static void setDead(bool d)
    {
        dead = d;
    }

    public static void timeControl(bool d)
    {
        if (d)
        {
            Time.timeScale = 0;
        } else
        {
            Time.timeScale = 1;
        }
    }

    public static void addEnemy(GameObject enemy)
    {
        if (enemy.GetComponent<EnemyAIMain>() != null)
        {
            mainEnemyList.Add(enemy);
        } else
        {
            healerEnemyList.Add(enemy);
        }
    }

    public static void removeEnemy(GameObject enemy)
    {
        if (enemy.GetComponent<EnemyAIMain>() != null)
        {
            mainEnemyList.Remove(enemy);
        }
        else
        {
            healerEnemyList.Remove(enemy);
        }
    }

    public static void initalize()
    {
        dead = false;
        mainEnemyList = new List<GameObject>();
        healerEnemyList = new List<GameObject>();
        timeControl(false);
    }
}

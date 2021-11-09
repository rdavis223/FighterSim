using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalStateMgr
{
    public static bool dead = false;

    public static List<GameObject> mainEnemyList = new List<GameObject>();
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
        mainEnemyList.Add(enemy);
    }

    public static void removeEnemy(GameObject enemy)
    {
        mainEnemyList.Remove(enemy);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalStateMgr
{
    public static bool dead = false;
    public static bool gamePaused = false;

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
        gamePaused = false;
        lockCursor();
    }

    public static void togglePause()
    {
        gamePaused = !gamePaused;
        if (gamePaused)
        {
            unlockCursor();
            timeControl(true);
        } else
        {
            lockCursor();
            timeControl(false);
        }
    }

    public static bool isPaused()
    {
        return gamePaused;
    }

    public static void unlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public static void lockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public static bool canMove()
    {
        return !gamePaused && !dead;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalStateMgr
{
    public static bool dead = false;

    public static bool isDead()
    {
        return dead;
    }

    public static void setDead(bool d)
    {
        dead = d;
    }
}

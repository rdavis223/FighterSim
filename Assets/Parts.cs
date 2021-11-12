using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts : MonoBehaviour
{
    private bool used;
    private void Start()
    {
        used = false;
    }

    public void setUsed()
    {
        used = true;
    }
    public bool isUsed()
    {
        return used;
    }
}

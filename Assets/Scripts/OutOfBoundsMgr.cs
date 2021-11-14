using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OutOfBoundsMgr : MonoBehaviour
{
    public float[] xBounds;
    public float[] yBounds;
    public float[] zBounds;
    private GameObject player;
    private bool outside = false;
    public GameObject returnToBattleField;
    public TMP_Text counter;
    public float outsideTime;
    private float outsideTimer;
    private void Start()
    {
        player = GameObject.Find("Player");
        outside = false;
    }


    private void Update()
    {
        if (outside)
        {
            outsideTimer -= Time.deltaTime;
            if (outsideTimer <= 0f)
            {
                returnToBattleField.SetActive(false);
                player.GetComponent<PlayerHealthMgr>().playerDie();
            }
            counter.text = Mathf.Ceil(outsideTimer).ToString();
        }
    }


    public void crossedBoundry()
    {
        if (xBounds[0] < player.transform.position.x && xBounds[1] > player.transform.position.x)
        {
            if (yBounds[0] < player.transform.position.y && yBounds[1] > player.transform.position.y)
            {
                if (zBounds[0] < player.transform.position.y && yBounds[1] > player.transform.position.z)
                {
                    outside = false;
                    updateState();
                    return;
                }
            }
        }
        outside = true;
        updateState();
    }

    private void updateState()
    {
        if (outside)
        {
            outsideTimer = outsideTime;
            returnToBattleField.SetActive(true);
            counter.text = Mathf.Ceil(outsideTimer).ToString();
        } else
        {
            returnToBattleField.SetActive(false);
        }
    }
}

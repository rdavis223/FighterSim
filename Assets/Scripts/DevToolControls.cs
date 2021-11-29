using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DevToolControls : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerHealthMgr playerHealth;
    public TMP_Text output;
    void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealthMgr>();
        output.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
            if (Input.GetKeyDown(KeyCode.G))
            {
                playerHealth.GOD_MODE = !playerHealth.GOD_MODE;
                StartCoroutine(displayOutput("GOD_MODE: " + playerHealth.GOD_MODE.ToString()));
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                destroyAllShips();
            }
        }
    }

    IEnumerator displayOutput(string t)
    {
        output.enabled = true;
        output.text = t;
        yield return new WaitForSeconds(3f);
        output.enabled = false;
    }

    private void destroyAllShips()
    {
        int enemiesDestroyed = 0;
        List<GameObject> eList = new List<GameObject>(GlobalStateMgr.mainEnemyList);
        List<GameObject> hList = new List<GameObject>(GlobalStateMgr.healerEnemyList);

        foreach (GameObject enemy in eList)
        {
            enemy.GetComponent<EnemyHealthMgr>().kill();
            enemiesDestroyed++;
        }
        foreach (GameObject enemy in hList)
        {
            enemy.GetComponent<EnemyHealthMgr>().kill();
            enemiesDestroyed++;
        }

        StartCoroutine(displayOutput("KILLED ALL ENEMIES: " + enemiesDestroyed.ToString()));

    }
}

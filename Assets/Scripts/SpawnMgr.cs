using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnMgr : MonoBehaviour
{
    public SpawnConfig config;
    public float range;

    public float currentEnemyShipHealth;
    public float currentEnemyShipShields;
    public float currentEnemyShipDamage;
    public float currentEnemyShipFireChance;

    public float currentHealerShipRechargeRate;
    public float currentHealerShipHealth;

    public float currentEnemyShipCount;
    public float currentHealerShipCount;

    private bool firstRound;

    private int fadeDirection;
    public float fadeSpeed;
    private bool isTransition;
    private Color32 startColor = new Color32(0, 0, 0, 255);
    private Color32 endColor = new Color32(0, 0, 0, 60);
    private Color32 currentColor;

    public TMP_Text roundText;

    private void Start()
    {
        GlobalStateMgr.initalize();
        firstRound = true;
        currentEnemyShipDamage = config.initialEnemyShipDamage;
        currentEnemyShipFireChance = config.initialEnemyShipFireChance;
        currentEnemyShipHealth = config.initialEnemyShipHealth;
        currentEnemyShipShields = config.initialEnemyShipShields;

        currentHealerShipRechargeRate = config.initalHealerShipRechargeRate;
        currentHealerShipHealth = config.initalHealerShipHealth;

        currentEnemyShipCount = (int) config.initialEnemyShipCount;
        currentHealerShipCount = (int) config.initialHealerShipCount;
        spawnEnemyShips();
        spawnHealerShips();
        firstRound = false;
    }
    IEnumerator nextRoundTransition()
    {
        GlobalStateMgr.nextRound();
        roundText.text = GlobalStateMgr.currentRound.ToString();
        fadeDirection = -1;
        currentColor = startColor;
        isTransition = true;
        yield return new WaitForSeconds(5f);
        nextRound();

    }
    void nextRound()
    {
        roundText.color = startColor;
        isTransition = false;


        int currentRound = GlobalStateMgr.currentRound;
        if (currentRound > config.enemyShipStartLevel)
        {
            currentEnemyShipCount = currentEnemyShipCount * config.enemyShipCountIncreaseRate;
            currentEnemyShipDamage = currentEnemyShipDamage * config.enemyShipDamageIncreaseRate;
            currentEnemyShipFireChance = currentEnemyShipFireChance * config.enemyFireChangeIncreaseRate;
            currentEnemyShipHealth = currentEnemyShipHealth * config.enemyShipHealthIncreaseRate;
            currentEnemyShipShields = currentEnemyShipShields * config.enemyShipShieldsIncreaseRate;
        }

        if (currentRound > config.healerShipStartLevel)
        {
            currentHealerShipCount = currentHealerShipCount * config.healerShipCountIncreaseRate;

            currentHealerShipRechargeRate = currentHealerShipRechargeRate * config.healerShipRechargeIncreaseRate;
            currentHealerShipHealth = currentHealerShipHealth * config.healerShipHealthIncreaseRate;
        }

        spawnEnemyShips();
        spawnHealerShips();

    }

    private void Update()
    {
        if (GlobalStateMgr.isRoundOver())
        {
            StartCoroutine(nextRoundTransition());
        }

        if (isTransition)
        {
            if (fadeDirection == -1)
            {
                currentColor = Color32.Lerp(currentColor, endColor, fadeSpeed * Time.deltaTime);
                roundText.color = currentColor;
            }
            else
            {
                currentColor = Color32.Lerp(currentColor, startColor, fadeSpeed * Time.deltaTime);
                roundText.color = currentColor;
            }
            if (roundText.color.a < 0.24f)
            {
                fadeDirection = 1;
            }

            if (roundText.color.a > 0.8f)
            {
                fadeDirection = -1;
            }
        }
    }



    void spawnEnemyShips()
    {
        if ((!firstRound && GlobalStateMgr.currentRound >= config.enemyShipStartLevel) || (firstRound && 1 >= config.enemyShipStartLevel))
        {
            int intCount = Mathf.FloorToInt(currentEnemyShipCount);
            for (int i = 0; i < intCount; i++)
            {
                GameObject ship = Instantiate(config.enemyShipPrefab);
                ship.GetComponent<EnemyAIMain>().initializeSpawnValues(currentEnemyShipHealth, currentEnemyShipDamage, currentEnemyShipShields, currentEnemyShipFireChance);
                ship.transform.position = generateSpawnPos();
            }
        }
    }

    void spawnHealerShips()
    {
        if ((!firstRound && GlobalStateMgr.currentRound >= config.healerShipStartLevel) ||  (firstRound && 1 >= config.healerShipStartLevel))
        {
            int intCount = Mathf.FloorToInt(currentHealerShipCount);
            for (int i = 0; i < currentHealerShipCount; i++)
            {
                GameObject ship = Instantiate(config.healerShipPrefab);
                ship.GetComponent<EnemyAIHealer>();
                ship.transform.position = generateSpawnPos();
            }
        }
    }

    Vector3 generateSpawnPos()
    {
        Vector3 pos = new Vector3(Random.Range(200f, range - 200f), Random.Range(200f, range - 200f), Random.Range(200f, range - 200f));
        Collider[] hitColliders = Physics.OverlapSphere(pos, 15f);
        if (hitColliders.Length == 0)
        {
            return pos;
        } else
        {
            return generateSpawnPos();
        }
    }

}

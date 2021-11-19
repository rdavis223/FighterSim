using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public int currentEnemyShipCount;
    public int currentHealerShipCount;

    private bool firstRound;

    private void Start()
    {
        firstRound = true;
        currentEnemyShipDamage = config.initialEnemyShipDamage;
        currentEnemyShipFireChance = config.initialEnemyShipFireChance;
        currentEnemyShipHealth = config.initialEnemyShipHealth;
        currentEnemyShipShields = config.initialEnemyShipShields;

        currentHealerShipRechargeRate = config.initalHealerShipRechargeRate;
        currentHealerShipHealth = config.initalHealerShipHealth;

        currentEnemyShipCount = config.initialEnemyShipCount;
        currentHealerShipCount = config.initialHealerShipCount;
        spawnEnemyShips();
        spawnHealerShips();
        firstRound = false;
    }

    void nextRound()
    {
        GlobalStateMgr.nextRound();
        int currentRound = GlobalStateMgr.currentRound;
        if (currentRound > config.enemyShipStartLevel)
        {
            currentEnemyShipCount = Mathf.FloorToInt(currentEnemyShipCount * config.enemyShipCountIncreaseRate);
            currentEnemyShipDamage = currentEnemyShipDamage * config.enemyShipDamageIncreaseRate;
            currentEnemyShipFireChance = currentEnemyShipFireChance * config.enemyFireChangeIncreaseRate;
            currentEnemyShipHealth = currentEnemyShipHealth * config.enemyShipHealthIncreaseRate;
            currentEnemyShipShields = currentEnemyShipShields * config.enemyShipShieldsIncreaseRate;
        }

        if (currentRound > config.healerShipStartLevel)
        {
            currentHealerShipCount = Mathf.FloorToInt(currentHealerShipCount * config.healerShipCountIncreaseRate);

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
            nextRound();
        }
    }



    void spawnEnemyShips()
    {
        if (GlobalStateMgr.currentRound >= config.enemyShipStartLevel || (firstRound && 1 >= config.enemyShipStartLevel))
        {
            for (int i = 0; i < currentEnemyShipCount; i++)
            {
                GameObject ship = Instantiate(config.enemyShipPrefab);
                ship.GetComponent<EnemyAIMain>().initializeSpawnValues(currentEnemyShipHealth, currentEnemyShipDamage, currentEnemyShipShields, currentEnemyShipFireChance);
                ship.transform.position = generateSpawnPos();
            }
        }
    }

    void spawnHealerShips()
    {
        if (GlobalStateMgr.currentRound >= config.healerShipStartLevel ||  (firstRound && 1 >= config.healerShipStartLevel))
        {
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
        Collider[] hitColliders = Physics.OverlapSphere(pos, 5f);
        if (hitColliders.Length != 0)
        {
            return pos;
        } else
        {
            return generateSpawnPos();
        }
    }

}

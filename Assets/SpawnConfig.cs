using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnConfig", order = 1)]
public class SpawnConfig : ScriptableObject
{
    public int enemyShipStartLevel;
    public int healerShipStartLevel;

    public int initialEnemyShipCount;
    public int initialHealerShipCount;

    public float initialEnemyShipHealth;
    public float initialEnemyShipShields;
    public float initialEnemyShipDamage;
    public float initialEnemyShipFireChance;

    public float initalHealerShipRechargeRate;
    public float initalHealerShipHealth;

    public float enemyShipCountIncreaseRate;
    public float enemyShipHealthIncreaseRate;
    public float enemyShipShieldsIncreaseRate;
    public float enemyShipDamageIncreaseRate;
    public float enemyFireChangeIncreaseRate;

    public float healerShipCountIncreaseRate;
    public float healerShipRechargeIncreaseRate;
    public float healerShipHealthIncreaseRate;

    public GameObject enemyShipPrefab;
    public GameObject healerShipPrefab;


}

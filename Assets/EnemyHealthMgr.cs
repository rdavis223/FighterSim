using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthMgr : MonoBehaviour
{
    public Slider healthBar;
    public float startingHealth;
    private float currentHealth;
    private List<GameObject> targetedMissiles;
    public EnemyShield shield;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        targetedMissiles = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void hurt(float damage)
    {
        shield.damageTaken();
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            destroyExistingMissiles();
            Destroy(this.gameObject);
        }
        updateHealthBar();
    }

    public void trackMissile(GameObject g)
    {
        targetedMissiles.Add(g);
    }

    void updateHealthBar()
    {
        healthBar.value = (currentHealth / startingHealth);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerProjectile")
        {
            this.hurt(other.gameObject.GetComponent<PlayerProjectile>().Damage);
            Destroy(other.gameObject);
        }
    }

    void destroyExistingMissiles()
    {
        foreach(GameObject missile in targetedMissiles)
        {
            Destroy(missile);
        }
    }
}

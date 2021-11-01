using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthMgr : MonoBehaviour
{
    public Image healthBar;
    public float startingHealth;
    private float currentHealth;
    public PlayerShield shield;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
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
            Debug.Log("DEAD");
        }
        updateHealthBar();
    }

    void updateHealthBar()
    {
        healthBar.fillAmount = (currentHealth / startingHealth);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyProjectile")
        {
            this.hurt(other.gameObject.GetComponent<EnemyProjectile>().Damage);
            Destroy(other.gameObject);
        }
    }
}

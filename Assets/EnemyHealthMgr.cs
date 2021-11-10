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
    public GameObject explosionPrefab;
    public GameObject partsPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GlobalStateMgr.addEnemy(this.gameObject);
        //TESTING : currentHealth = startingHealth;
        currentHealth = 20f;
        targetedMissiles = new List<GameObject>();
        updateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentHealth);
    }

    public void kill()
    {
        hurt(startingHealth);
    }

    public void hurt(float damage)
    {
        shield.damageTaken();
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            destroyExistingMissiles();
            GlobalStateMgr.removeEnemy(this.gameObject);
            this.GetComponent<EnemyAICommon>().initiateDetach();
            explode();
            dropParts();
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

        if (other.gameObject.tag == "Enemy")
        {
            kill();
        }
        if (other.gameObject.tag == "Ast")
        {
            if (shield.isShieldActive())
            {
                shield.hurtShield(49f);
            }
            else
            {
                this.hurt(49f);
            }
            other.gameObject.GetComponent<AstroidMovement>().explode();
        }
    }

    void destroyExistingMissiles()
    {
          foreach(GameObject missile in targetedMissiles)
        {
            Destroy(missile);
        }
    }
    void explode()
    {
        GameObject exp = Instantiate(explosionPrefab);
        exp.transform.position = this.transform.position;
    }
    void dropParts()
    {
        GameObject exp = Instantiate(partsPrefab);
        exp.transform.position = this.transform.position;
    }

    public float getCurrentHealthPercent()
    {
        return currentHealth / startingHealth;
    }
}

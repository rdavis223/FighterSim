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
    private MeshRenderer playerBody;
    public GameObject explosionPrefab;
    public bool GOD_MODE;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        playerBody = this.GetComponent<MeshRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = startingHealth;
    }

    public void hurt(float damage)
    {
        shield.damageTaken();
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            Debug.Log("DEAD");
            currentHealth = 0f;
            playerDie();
        }
        updateHealthBar();
    }

    void updateHealthBar()
    {
        healthBar.fillAmount = (currentHealth / startingHealth);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyProjectile" && !shield.isShieldActive())
        {
            Debug.Log("shot");
            this.hurt(other.gameObject.GetComponent<EnemyProjectile>().Damage);
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Enemy")
        {
            if (!shield.isShieldActive())
            {
                EnemyShipCrash(other.gameObject);
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("CharacterControllerHit");
        if (hit.gameObject.tag == "Enemy")
        {
            EnemyShipCrash(hit.gameObject);
        }
    }

    void EnemyShipCrash(GameObject other)
    {
        this.hurt(startingHealth);
        EnemyHealthMgr mgr = other.GetComponent<EnemyHealthMgr>();
        if (mgr != null)
        {
            mgr.kill();
        }

    }

    public void repair(float repairAmount)
    {
        currentHealth += repairAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, startingHealth);
        updateHealthBar();
    }

    public void playerDie()
    {
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = this.transform.position;
        playerBody.enabled = false;
        GlobalStateMgr.setDead(true);
    }
}

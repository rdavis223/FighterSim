using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthMgr : MonoBehaviour
{
    public Image healthBar1;
    public Image healthBar2;
    public float startingHealth;
    private float currentHealth;
    public PlayerShield shield;
    private MeshRenderer playerBody;
    public GameObject explosionPrefab;
    public bool GOD_MODE;
    public GameObject gameOverCanvas;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        playerBody = this.GetComponent<MeshRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    public void hurt(float damage)
    {
        shield.damageTaken();
        currentHealth -= damage;
        if (GOD_MODE)
        {
            currentHealth = startingHealth;
            updateHealthBar();
        }
        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            playerDie();
        }
        updateHealthBar();

    }

    void updateHealthBar()
    {
        healthBar1.fillAmount = (currentHealth / startingHealth);
        healthBar2.fillAmount = (currentHealth / startingHealth);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyProjectile" && !shield.isShieldActive())
        {
            this.hurt(other.gameObject.GetComponent<EnemyProjectile>().Damage);
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Ast")
        {
            if (!shield.isShieldActive())
            {
                if (other.gameObject.tag == "Enemy")
                {
                    EnemyShipCrash(other.gameObject);
                }
                else
                {
                    AsteroidCrash(other.gameObject);
                }
            }
            else
            {
                shield.shieldCollision(other.gameObject);
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Enemy" || hit.gameObject.tag == "Ast")
        {
            if (!shield.isShieldActive())
            {
                if (hit.gameObject.tag == "Enemy")
                {
                    EnemyShipCrash(hit.gameObject);
                } else
                {
                    AsteroidCrash(hit.gameObject);
                }
            }
            else
            {
                shield.shieldCollision(hit.gameObject);
            }
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

    void AsteroidCrash(GameObject other)
    {
        this.hurt(startingHealth / 2);
        AstroidMovement mgr = other.GetComponent<AstroidMovement>();
        if (mgr != null)
        {
            mgr.explode();
        }
        else
        {
            mgr = other.transform.parent.gameObject.GetComponent<AstroidMovement>();
            if (mgr != null)
            {
                mgr.explode();
            }
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
        shield.disableGraphic();
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = this.transform.position;
        playerBody.enabled = false;
        GlobalStateMgr.unlockCursor();
        StartCoroutine("showGameOverScreen");
        GlobalStateMgr.setDead(true);

    }

    IEnumerator showGameOverScreen()
    {
        yield return new WaitForSeconds(.5f);
        gameOverCanvas.SetActive(true);
        GlobalStateMgr.timeControl(true);

    }
}

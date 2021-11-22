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
    public bool hasShield;
    private bool dieRan = false;
    private bool partsDropped = false;
    private bool firstRun;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        targetedMissiles = new List<GameObject>();
        updateHealthBar();
        firstRun = true;
    }

    public void setInitialHealthValue(float health)
    {
        startingHealth = health;
        currentHealth = startingHealth;
        updateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        if (firstRun)
        {
            GlobalStateMgr.addEnemy(this.gameObject);
            firstRun = false;
        }
    }

    public void kill()
    {
        hurt(startingHealth);
    }

    public void hurt(float damage)
    {
        if (hasShield)
        {
            shield.damageTaken();
        }
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            if (!dieRan)
            {
                dieRan = true;
                destroyExistingMissiles();
                GlobalStateMgr.removeEnemy(this.gameObject);
                this.GetComponent<EnemyAICommon>().initiateDetach();
                explode();
                dropParts();
                Destroy(this.gameObject);
            }
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
            if (!shield.isShieldActive())
            {
                this.hurt(other.gameObject.GetComponent<PlayerProjectile>().Damage);
            }
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Enemy")
        {
            GameObject e = this.GetComponent<EnemyAICommon>().getAttachedEnemy();
            if (e == null || other.gameObject != e)
            {
                kill();
            }
        }
        if (other.gameObject.tag == "Ast")
        {
            if (hasShield && shield.isShieldActive())
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
    void dropParts() { 
        float r = Random.Range(0f, 100f);
        if (r <= 60f)
        {
            GameObject exp = Instantiate(partsPrefab);
            exp.transform.position = this.transform.position;
        }
        
    }

    public float getCurrentHealthPercent()
    {
        return currentHealth / startingHealth;
    }

    public void heal(float amount)
    {
        currentHealth += amount;
        updateHealthBar();
    }
}

  using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyShield : MonoBehaviour
{
    public float sinceDamageTime;
    private float sinceDamageTimer;

    public float shieldEnergyMax;
    private float shieldEnergy;

    private MeshRenderer visible;
    private Collider shield;
    public GameObject sliderObject;
    public Slider ui;

    private EnemyHealthMgr healthmgr;

    public float shieldRechargeRate;
    // Start is called before the first frame update
    void Start()
    {
        shieldEnergy = shieldEnergyMax;
        visible = this.GetComponent<MeshRenderer>();
        shield = this.GetComponent<MeshCollider>();
        shield.enabled = true;
        healthmgr = this.transform.parent.GetComponentInParent<EnemyHealthMgr>();
    }

    public void setInitialShieldEnergy(float energy)
    {
        shieldEnergyMax = energy;
        shieldEnergy = shieldEnergyMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (sinceDamageTimer <= 0f)
        {
            visible.enabled = false;
            shieldEnergy += Time.deltaTime * shieldRechargeRate;
            shieldEnergy = Mathf.Clamp(shieldEnergy, -500f, shieldEnergyMax);
        }

        if (shieldEnergy <= 0f)
        {
            visible.enabled = false;
            shield.enabled = false;
            sliderObject.SetActive(false);
        } else
        {
            shield.enabled = true;
            ui.enabled = true;
            sliderObject.SetActive(true);
            ui.value = shieldEnergy / shieldEnergyMax;
        }
        sinceDamageTimer -= Time.deltaTime;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerProjectile")
        {
            Destroy(other.gameObject);
            hurtShield(other.gameObject.GetComponent<PlayerProjectile>().Damage);
        }
    }

    public void damageTaken()
    {
        sinceDamageTimer = sinceDamageTime;
    }

    public void hurtShield(float damage)
    {
        visible.enabled = true;
        sinceDamageTimer = sinceDamageTime;
        shieldEnergy -= damage;
        if (shieldEnergy < 0f)
        {
            Debug.Log("Transfer");
            if (healthmgr != null)
            {
                healthmgr.hurt(shieldEnergy * -1);
                shieldEnergy = 0f;
            }
        }
    }

    public bool isShieldActive()
    {
        return shield.enabled;
    }

}

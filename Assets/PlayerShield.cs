using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShield : MonoBehaviour
{
    public float sinceDamageTime;
    private float sinceDamageTimer;

    public float shieldEnergyMax;
    public float shieldEnergy;

    private MeshRenderer visible;
    private Collider shield;
    public Image ui;

    public float shieldRechargeRate;

    public bool disableShield;
    // Start is called before the first frame update
    void Start()
    {
        shieldEnergy = shieldEnergyMax;
        visible = this.GetComponent<MeshRenderer>();
        shield = this.GetComponent<MeshCollider>();
        shield.enabled = true;

        if (disableShield)
        {
            shield.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (disableShield)
        {
            Debug.Log("returned");
            return;
        }
        if (sinceDamageTimer <= 0f)
        {
            visible.enabled = false;
            shieldEnergy += Time.deltaTime * shieldRechargeRate;
            shieldEnergy = Mathf.Clamp(shieldEnergy, 0f, shieldEnergyMax);
        }

        if (shieldEnergy <= 0f)
        {
            visible.enabled = false;
            shield.enabled = false;
        }
        else
        {
            shield.enabled = true;
        }
        ui.fillAmount = shieldEnergy / shieldEnergyMax;
        sinceDamageTimer -= Time.deltaTime;
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (disableShield)
        {
            Debug.Log("returned");
            return;
        }
        if (other.gameObject.tag == "EnemyProjectile")
        {
            visible.enabled = true;
            Destroy(other.gameObject);
            sinceDamageTimer = sinceDamageTime;
            shieldEnergy -= other.gameObject.GetComponent<EnemyProjectile>().Damage;
        }
    }

    public void damageTaken()
    {
        sinceDamageTimer = sinceDamageTime;
    }

    public void shieldCollision(GameObject other)
    {
        if (disableShield)
        {
            return;
        }
        if (other.gameObject.tag == "Enemy" && shield.enabled && shieldEnergy > 1f)
        {
            visible.enabled = true;
            Destroy(other.gameObject);
            sinceDamageTimer = sinceDamageTime;
            shieldEnergy = 1f;
            EnemyHealthMgr mgr = other.gameObject.GetComponent<EnemyHealthMgr>();
            if (mgr != null)
            {
                mgr.kill();
            } 
        } else if (other.gameObject.tag == "Enemy" && shield.enabled)
        {
            sinceDamageTimer = sinceDamageTime;
            shieldEnergy = 0f;

        }
    }

    public bool isShieldActive()
    {
        return shield.enabled;
    }

}

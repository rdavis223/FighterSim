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
        if (disableShield)
        {
            return;
        }
        if (other.gameObject.tag == "EnemyProjectile")
        {
            visible.enabled = true;
            Destroy(other.gameObject);
            takeShieldDamage(other.gameObject.GetComponent<EnemyProjectile>().Damage);
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
            sinceDamageTimer = sinceDamageTime;
            shieldEnergy = 1f;
            EnemyHealthMgr mgr = other.gameObject.GetComponent<EnemyHealthMgr>();
            if (mgr != null)
            {
                mgr.kill();
            }
        }
        else if (other.gameObject.tag == "Enemy" && shield.enabled)
        {
            sinceDamageTimer = sinceDamageTime;
            shieldEnergy = 0f;

        }
        else if (other.gameObject.tag == "Ast" && shield.enabled)
        {
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
            takeShieldDamage(49f);
        }
    }

    public void takeShieldDamage(float damage)
    {
        visible.enabled = true;
        sinceDamageTimer = sinceDamageTime;
        shieldEnergy -= damage;
    }

    public bool isShieldActive()
    {
        return shield.enabled;
    }

}

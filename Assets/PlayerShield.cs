using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShield : MonoBehaviour
{
    public float sinceDamageTime;
    private float sinceDamageTimer;

    public float shieldEnergyMax;
    private float shieldEnergy;

    private MeshRenderer visible;
    private Collider shield;
    public Image ui;

    public float shieldRechargeRate;
    // Start is called before the first frame update
    void Start()
    {
        shieldEnergy = shieldEnergyMax;
        visible = this.GetComponent<MeshRenderer>();
        shield = this.GetComponent<Collider>();
        shield.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
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

}

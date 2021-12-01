using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRepairMgr : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxRepair;
    private float repairLevel;
    public float repairPerPart;
    private PlayerHealthMgr health;
    public Image ui;
    private bool partsLock;
    public GameObject[] regThrusters;
    public GameObject[] healThrusters;

    private void Start()
    {
        partsLock = false;
        repairLevel = 0f;
        health = this.GetComponent<PlayerHealthMgr>();
        updateRepairBar();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (health.getLostHealth() > 0f)
            {
                if (health.getLostHealth() >= repairLevel)
                {
                    health.repair(repairLevel);
                    repairLevel = 0f;
                }
                else
                {
                    repairLevel -= health.getLostHealth();
                    health.repair(health.getLostHealth());
                }
                StartCoroutine(healThrusterEffect());
                updateRepairBar();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Parts")
        {
            repairLevel += repairPerPart;
            repairLevel = Mathf.Clamp(repairLevel, 0f, maxRepair);
            Destroy(other.gameObject);
            updateRepairBar();
        }
    }

    void updateRepairBar()
    {
        ui.fillAmount = repairLevel / maxRepair;
    }

    IEnumerator healThrusterEffect()
    {
        foreach (GameObject thruster in healThrusters)
        {
            thruster.SetActive(true);
        }
        foreach (GameObject thruster in regThrusters)
        {
            thruster.SetActive(false);
        }

        yield return new WaitForSeconds(3f);

        foreach (GameObject thruster in regThrusters)
        {
            thruster.SetActive(true);
        }

        foreach (GameObject thruster in healThrusters)
        {
            thruster.SetActive(false);
        }

    }

}

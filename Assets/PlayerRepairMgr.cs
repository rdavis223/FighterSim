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


    private void Start()
    {
        repairLevel = 0f;
        health = this.GetComponent<PlayerHealthMgr>();
        updateRepairBar();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            health.repair(repairLevel);
            repairLevel = 0f;
            updateRepairBar();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");
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
}

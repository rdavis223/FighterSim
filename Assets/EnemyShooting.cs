using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{

    public GameObject[] shootPos;
    public GameObject bulletPrefab;
    public float[] fireTimes;
    private float activeFireTime;
    private float fireTimer;
    public float weaponHeatMax;
    private float weaponHeat;
    public float chanceOfShoot;
    // Start is called before the first frame update
    void Start()
    {
        activeFireTime = fireTimes[0];
        fireTimer = 0;
        weaponHeat = 0;

    }

    void Update()
    {
        fireTimer += Time.deltaTime;
        weaponHeat -= Time.deltaTime * 2f;
    }
    public void shootPrimary()
    {
        if (!GlobalStateMgr.canMove())
        {
            return;
        }
        float shootRand = Random.Range(0f, 100f);
        if (shootRand > chanceOfShoot)
        {
            return;
        }
        if (fireTimer < activeFireTime || weaponHeat == weaponHeatMax)
        {
            return;
        }

        for (int i = 0; i < shootPos.Length; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = shootPos[i].transform.position;
            bullet.transform.forward = shootPos[i].transform.position;
            float end_x = this.transform.position.x + 50 * this.transform.forward.x;
            float end_y = this.transform.position.y + 50 * this.transform.forward.y;
            float end_z = this.transform.position.z + 50 * this.transform.forward.z;
            Vector3 lookPosition = new Vector3(end_x, end_y, end_z);
            bullet.GetComponent<BulletMovement>().setStraight(this.transform.forward, lookPosition);
            bullet.transform.LookAt(lookPosition);
            fireTimer = 0;
            weaponHeat++;
            float weaponHeatPercent = ((weaponHeat / weaponHeatMax) * 100);
            if (weaponHeatPercent < 70)
            {
                activeFireTime = fireTimes[0];

            }
            else if (weaponHeatPercent > 70 && weaponHeatPercent < 90)
            {
                activeFireTime = fireTimes[1];
            }
            else
            {
                activeFireTime = fireTimes[2];
            }
        }
    }
}

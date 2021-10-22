using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject[] shootPos;
    public GameObject bulletPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            for (int i =0; i < shootPos.Length; i++)
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
            }
        }
    }
}

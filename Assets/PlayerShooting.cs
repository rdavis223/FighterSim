using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject[] shootPos;
    public GameObject bulletPrefab;
    public Transform aimUI;
    public GameObject[] missilePos;
    public GameObject missilePrefab;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
                float end_x = this.transform.position.x + 100 * this.transform.forward.x;
                float end_y = this.transform.position.y + 100 * this.transform.forward.y;
                float end_z = this.transform.position.z + 100 * this.transform.forward.z;
                Vector3 lookPosition = new Vector3(end_x, end_y, end_z);
                bullet.GetComponent<BulletMovement>().setStraight(this.transform.forward, lookPosition);
                bullet.transform.LookAt(lookPosition);
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Ray ray = Camera.main.ScreenPointToRay(aimUI.position);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit)){
                GameObject enemy = hit.transform.gameObject;
                for (int i = 0; i < missilePos.Length; i++)
                {
                    GameObject bullet = Instantiate(missilePrefab);
                    bullet.transform.position = missilePos[i].transform.position;
                    bullet.transform.forward = missilePos[i].transform.forward;
                    bullet.GetComponent<Missile>().setTarget(enemy);
                    bullet.transform.LookAt(enemy.transform.position);
                }
            }

        }

    }
}

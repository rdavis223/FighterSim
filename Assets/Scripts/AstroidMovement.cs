using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroidMovement : MonoBehaviour
{
    private float rotateX;
    private float rotateY;
    private float rotateZ;

    private float rotateSpeed;

    private Vector3 dir;

    private float moveSpeed;
    public GameObject explosionPrefab;

    public bool disableMovement = false;
    private void Start()
    {
        rotateX = Random.Range(0f, 1f);
        rotateY = Random.Range(0f, 1f);
        rotateZ = Random.Range(0f, 1f);
        rotateSpeed = Random.Range(10f, 40f);
        dir = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), 0f) * this.transform.forward;
        moveSpeed = Random.Range(4f, 10f);

    }

    private void Update()
    {
        if (!disableMovement)
        {
            this.transform.Rotate(rotateX * Time.deltaTime * rotateSpeed, rotateY * Time.deltaTime * rotateSpeed, rotateZ * Time.deltaTime * rotateSpeed);
            this.transform.position += dir * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Wall"))
        {
            this.transform.LookAt(new Vector3(750f, 750f, 750f));
            dir = Quaternion.Euler(Random.Range(-45f, 45f), Random.Range(-45f, 45f), 0f) * this.transform.forward;
        }

        if (other.gameObject.tag == "PlayerProjectile")
        {
            Missile m = other.gameObject.GetComponent<Missile>();
            if (m != null)
            {
                m.removeTarget();
            }
            Destroy(other.gameObject);
            explode();
         
        } else if (other.gameObject.tag == "EnemyProjectile")
        {
            Destroy(other.gameObject);
            explode();
        }
    }

    public void bounceOffEnvironment()
    {
        dir = Quaternion.Euler(180f + Random.Range(-30f, 30f), 0f, 0f) * dir;
    }

    public void explode()
    {
        GameObject exp = Instantiate(explosionPrefab);
        exp.transform.position = this.transform.position;
        Destroy(this.gameObject);
    }

}

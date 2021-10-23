using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float range;
    private bool destSet;
    private Vector3 dest;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        if (!destSet)
        {
            dest = new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));
            destSet = true;
        }
        if (Vector3.Distance(this.transform.position, dest) <= 1f)
        {
            destSet = false;
        }
        else
        {
            this.transform.LookAt(dest);
            this.transform.position += this.transform.forward * speed * Time.deltaTime;
        }
    }

    public void handleCollision(Collider other)
    {
        if (other.gameObject.name.Contains("Bullet"))
        {
            Destroy(this.gameObject);
        }
    }
}

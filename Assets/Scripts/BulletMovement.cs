using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{

    public float speed;
    public float timeToLive = 10f;
    private Vector3 straightPos;
    private Vector3 straight;

    // Update is called once per frame
    private void Start()
    {

    }

    void Update()
    {
        if (Vector3.Distance(this.transform.position, straightPos) < 2f)
        {
            this.transform.forward = straight;
        }

        transform.position += transform.forward * speed * Time.deltaTime;

        if (timeToLive <= 0f)
        {
            Destroy(this.gameObject);
        }

        timeToLive -= Time.deltaTime;
    }

    public void setStraight(Vector3 str, Vector3 strPos)
    {
        straightPos = strPos;
        straight = str;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Station")
        {
            Destroy(this.gameObject);
        }
    }
}


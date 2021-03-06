using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollision : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyShield")
        {
            this.transform.parent.GetComponent<PlayerShield>().shieldCollision(other.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsCollision : MonoBehaviour
{
    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("collsion");
        if (collision.gameObject.name == "Player")
        {
            this.transform.parent.GetComponentInParent<OutOfBoundsMgr>().crossedBoundry();
        }
    }
}

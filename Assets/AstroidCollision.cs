using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroidCollision : MonoBehaviour
{
    private AstroidMovement astroid;
    void Start()
    {
        astroid = this.transform.parent.gameObject.GetComponent<AstroidMovement>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Station")
        {
            astroid.bounceOffEnvironment();
        }
    }
}

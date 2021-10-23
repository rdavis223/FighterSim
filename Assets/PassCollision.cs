using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     
    void OnTriggerEnter(Collider other)
    {

        if (this.transform.parent.transform.parent.GetComponent<EnemyAI>() != null)
        {
            this.transform.parent.transform.parent.GetComponent<EnemyAI>().handleCollision(other);
        } 
    }
}

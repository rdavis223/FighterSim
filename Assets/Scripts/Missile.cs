using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private GameObject target;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            this.transform.LookAt(target.transform.position);
            this.transform.position += this.transform.forward * speed * Time.deltaTime;
        }
    }
    public void setTarget(GameObject t)
    {
        target = t;
        t.GetComponent<EnemyHealthMgr>().trackMissile(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem exp = this.GetComponent<ParticleSystem>();
        float totalDuration = exp.main.duration;
        Destroy(this.gameObject, totalDuration);
    }

}

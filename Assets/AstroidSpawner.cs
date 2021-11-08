using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroidSpawner : MonoBehaviour
{

    public GameObject astroidRed;
    public GameObject asteroidBlue;
    public int asteroidsToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        int numRed = Random.Range(0, asteroidsToSpawn);
        int i = 0;
        while (i < asteroidsToSpawn)
        {
            Vector3 pos = new Vector3(Random.Range(0f, 1500f), Random.Range(0f, 1500f), Random.Range(0f, 1500f));
            Collider[] hitColliders = Physics.OverlapSphere(pos, 5f);
            if (hitColliders.Length == 0)
            {
                GameObject asteroid;
                if (i < numRed)
                {
                    asteroid = Instantiate(astroidRed);
                    
                } else
                {
                    asteroid = Instantiate(asteroidBlue);
                }
                asteroid.transform.position = pos;
                i++;
            }
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsMovement : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.Find("Player");   
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(player.transform.position);
    }
}

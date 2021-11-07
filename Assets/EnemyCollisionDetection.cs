using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update
    public EnemyAI aiControl;

    private void OnTriggerEnter(Collider collision)
    {
        aiControl.detectObject(collision);
    }
}
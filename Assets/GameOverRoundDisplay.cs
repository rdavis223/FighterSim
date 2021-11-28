using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverRoundDisplay : MonoBehaviour
{
    
    void Awake()
    {
        this.GetComponent<TMP_Text>().text = "You Survived " + GlobalStateMgr.currentRound.ToString() + " Rounds!";
    } 

    // Update is called once per frame
    void Update()
    {
        
    }
}

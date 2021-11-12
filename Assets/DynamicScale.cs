using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicScale : MonoBehaviour
{

    private RectTransform image;
    private RectTransform canvas;
    // Start is called before the first frame update
    void Start()
    {
        image = this.GetComponent<RectTransform>();
        canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

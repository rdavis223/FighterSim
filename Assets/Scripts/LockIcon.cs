using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockIcon : MonoBehaviour
{

    public Canvas UICanvas;
    private RectTransform rect;
    private RectTransform otherRect;
    // Start is called before the first frame update
    void Awake()
    {
        //rect = this.GetComponent<RectTransform>();
        //otherRect = UICanvas.GetComponent<RectTransform>();
        //rect.localScale = rect.localScale.x * otherRect.localScale;
    }
    // Update is called once per frame
    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimFollow : MonoBehaviour
{
    public GameObject player;
    public RectTransform aim;
    private Vector2 uiOffset;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float end_x = player.transform.position.x + 50 * player.transform.forward.x;
        float end_y = player.transform.position.y + 50 * player.transform.forward.y;
        float end_z = player.transform.position.z + 50 * player.transform.forward.z;
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, new Vector3(end_x,end_y, end_z));
        aim.anchoredPosition = screenPoint - this.GetComponent<RectTransform>().sizeDelta / 2f;
    }
}

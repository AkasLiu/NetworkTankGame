using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMove : MonoBehaviour {

    public EasyTouch touch;

    public void Start()
    {
        touch = GameObject.Find("Joystick").GetComponent<EasyTouch>();
    }

    // Update is called once per frame
    void Update()
    {
        //获取horizontal 和 vertical 的值，其值位遥感的localPosition
        float hor = touch.Horizontal;
        float ver = touch.Vertical;

        Vector3 direction = new Vector3(hor, 0, ver);

        if (direction != Vector3.zero)
        {
            transform.Translate(transform.forward * ver / 10 * Time.deltaTime, Space.World);
            transform.transform.Rotate(0, hor / 2 * Time.deltaTime, 0, Space.Self);
        }
    }
}

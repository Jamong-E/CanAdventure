using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelControl : MonoBehaviour
{
    public GameObject Wheel;
    public GameObject Player;
    protected static float powerDefault = 20;
    float delta = 0;
    public int fever = 0;
    protected static int feverMax = 50;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButton(0) && fever > 0) { delta += Time.deltaTime; }
        if (delta > 0.5) { fever = (int)(fever * 0.5f) - 1; delta = 0; }
        if (Input.GetMouseButton(0))
        {
            float anglePrev = Wheel.transform.eulerAngles.z;
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Wheel.transform.position;
            float angle = Mathf.Atan(pos.y / pos.x) * 180 / Mathf.PI;
            int key = (pos.x < 0) ? 180 : 0;
            Wheel.transform.eulerAngles = new Vector3(0, 0, angle + key);

            float angleNew = Wheel.transform.eulerAngles.z;
            float power = powerDefault + (int)(fever / 10) * 10;
            if (0 < anglePrev && anglePrev < 90 && 90 < angleNew && angleNew < 180) { Player.GetComponent<CanControl>().Roll(-1 * fever*fever/10, -1 * power); fever++; }
            if (90 < anglePrev && anglePrev < 180 && 0 < angleNew && angleNew < 90) { Player.GetComponent<CanControl>().Roll(fever*fever/10, power); fever++; }
            if (fever > 50) { fever = 50; }
        }
    }
}

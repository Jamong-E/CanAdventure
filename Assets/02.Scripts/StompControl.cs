using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompControl : MonoBehaviour
{
    public GameObject Player;
    protected bool Moving = false;
    float delta = 0;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Moving)
        {
            delta += Time.deltaTime;
            if (transform.position.y > 15) { transform.position = new Vector2(20, 15); }
            if (2 < delta && delta < 5) { rb.AddForce(new Vector3(0, -100000, 0)); }
            else if (5 < delta && delta < 10) { rb.AddForce(new Vector3(0, 20000, 0)); }
            else if (delta > 10) { delta = 0; }

            
        }
        else
        {
            float diff = Player.transform.position.x - this.transform.position.x;
            if (-5 < diff && diff < 5) { Moving = true; }
        }
    }

}

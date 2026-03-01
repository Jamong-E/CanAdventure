using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanControl : MonoBehaviour
{
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -5) {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
            transform.position = new Vector2(0, 3);
        }
    }
    public void Roll(float push, float spin)
    {
        rb.AddForce(new Vector3(push, 0, 0));
        rb.AddTorque(-1 * spin);
    }
}

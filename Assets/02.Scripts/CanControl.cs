using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanControl : MonoBehaviour
{
    public GameObject Cam;
    public GameObject ParticleGuy;
    Rigidbody2D rb;
    ParticleSystem ps;
    bool alive = true;
    float delta = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ps = ParticleGuy.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        ParticleGuy.transform.position = this.transform.position;
        if (!alive) { delta += Time.deltaTime; }
        if (!alive && delta > 1)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
            alive = true;
            delta = 0;
            transform.position = new Vector2(0, 3);
        }

        if (alive && transform.position.y < -5) {
            ps.Play();
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
            alive = false;
        }
        else { Cam.transform.position = new Vector3(this.transform.position.x, 0, -10); }

    }
    public void Roll(float push, float spin)
    {
        if (alive) {
            rb.AddForce(new Vector3(push, 0, 0));
            rb.AddTorque(-1 * spin);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Stomp")
        {
            ps.Play();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Stomp")
        {
            alive = false;
        }
    }
}

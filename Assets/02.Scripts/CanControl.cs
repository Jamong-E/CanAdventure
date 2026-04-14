using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Outlook
{
    Roll,
    Pressed     // TODO : make the Can pressed AFTER the stomp lands!!!!! (idea : use instance && StompControl)
}

public class CanControl : MonoBehaviour
{
    public GameObject Cam;
    public GameObject ParticleGuy;
    public GameObject CanDefault;
    public GameObject CanPressed;
    CircleCollider2D CircleCD;
    BoxCollider2D BoxCD;
    Rigidbody2D rb;
    ParticleSystem ps;
    bool alive = true;
    float deathCam = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ps = ParticleGuy.GetComponent<ParticleSystem>();
        CircleCD = GetComponent<CircleCollider2D>();
        BoxCD = GetComponent<BoxCollider2D>();
        CircleCD.enabled = true;
        BoxCD.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive) { deathCam -= Time.deltaTime; }
        if (!alive && deathCam <= 0)
        {
            CanDefault.SetActive(true);
            CanPressed.SetActive(false);
            CircleCD.enabled = true;
            BoxCD.enabled = false;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
            alive = true;
            deathCam = 0;
            transform.position = new Vector2(0, 3);
        }

        if (alive && transform.position.y < -5) {
            ps.Play();
            rb.angularVelocity = 0;
            Death(1);
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
            Debug.Log(collision.gameObject.name);
            ps.Play();
            transform.rotation = Quaternion.identity;
            rb.angularVelocity = 0;
            CanDefault.SetActive(false);
            CanPressed.SetActive(true);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Stomp")
        {
            Death(2);
            CircleCD.enabled = false;
            BoxCD.enabled = true;
        }
    }
    public void Death(float camLength)
    {
        alive = false;
        deathCam = camLength;
    }
}

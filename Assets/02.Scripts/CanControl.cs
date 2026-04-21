using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(BoxCollider2D))]
public class CanControl : MonoBehaviour
{
    enum Outlook
    {
        CRoll,
        RRoll,
        CtoR,
        RtoC,
        CPressed     // TODO : make the Can pressed AFTER the stomp lands!!!!! (idea : use instance && StompControl)
    }
    public GameObject Cam;
    public GameObject ParticleGuy;
    public GameObject CanDefault;
    public GameObject CanPressed;
    public GameObject Rotater;
    public CircleCollider2D CircleCD;
    public BoxCollider2D CPressedCD;
    public BoxCollider2D RectCD;
    Rigidbody2D rb;
    ParticleSystem ps;
    bool alive = true;
    float deathCam = 0;
    private Outlook outlook = Outlook.CRoll;
    float rotateTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ps = ParticleGuy.GetComponent<ParticleSystem>();
        CircleCD.enabled = true;
        CPressedCD.enabled = false;
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
            CPressedCD.enabled = false;
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
        
        if (outlook == Outlook.RRoll && Input.GetKeyDown(KeyCode.Alpha1))
        {
            outlook = Outlook.RtoC;
            rotateTime = 1;
        }
        
        if (outlook == Outlook.CRoll && Input.GetKeyDown(KeyCode.Alpha2))
        {
            outlook = Outlook.CtoR;
            rotateTime = 1;
        }

        // Rect → Circle
        if (outlook == Outlook.RtoC)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
            float delta = Time.deltaTime;
            if (Rotater.transform.eulerAngles.x > 0) { Rotater.transform.Rotate(new Vector3(-360*delta, 0, 0)); }
            if (Rotater.transform.eulerAngles.x < 15)
            {
                Rotater.transform.eulerAngles = new Vector3(0, 0, 0);
                rotateTime -= delta;
                if (rotateTime < 0) { rotateTime = 0; outlook = Outlook.CRoll; }
            }
        }

        // Circle → Rect
        if (outlook == Outlook.CtoR)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
            float delta = Time.deltaTime;
            if (Rotater.transform.eulerAngles.x < 90) { Rotater.transform.Rotate(new Vector3(360*delta, 0, 0)); }
            if (Rotater.transform.eulerAngles.x > 75)
            {
                Rotater.transform.eulerAngles = new Vector3(90, 0, 0);
                rotateTime -= delta;
                if (rotateTime < 0) { rotateTime = 0; outlook = Outlook.RRoll; }
            }
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
            CPressedCD.enabled = true;
        }
    }
    public void Death(float camLength)
    {
        alive = false;
        deathCam = camLength;
    }
}

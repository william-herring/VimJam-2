using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool isGrounded;
    public bool isJumping;
    public float jumpForce;
    public float speed = 5f;
    public float dashAcceleration;
    public float dashCooldownTime;
    public Animator anim;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float climbSpeed;
    public bool dash;
    public Rigidbody2D rb;
    private bool isRunning;
    private bool climbing;
    void Update()
    {
        //GroundCheck();

        //Jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            anim.Play("Jump");
            isJumping = true; rb.velocity = Vector2.up * jumpForce;
        }

        //General movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(horizontal * Time.deltaTime, 0);

        if (dash)
        {
            dashCooldown = dashCooldownTime;
        }

        if (!dash)
        {
            dashCooldown -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Debug.Log(Input.mousePosition);
            
            if (dashCooldown <= 0)
            {
                dash = true;
                anim.SetTrigger("Dash");

                if (Input.mousePosition.x - Screen.width / 2 > 0) 
                { 
                    transform.localScale = new Vector2(1, 1);

                    rb.AddForce(Vector2.right * dashAcceleration); 
                }

                if (Input.mousePosition.x - Screen.width / 2 < 0)
                {
                    transform.localScale = new Vector2(-1, 1);

                    rb.AddForce(Vector2.left * dashAcceleration); 
                }

                dash = false;
                dashCooldown = dashCooldownTime;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Input.mousePosition.x - Screen.width / 2 > 0) 
            { 
                transform.localScale = new Vector2(1, 1);
            }

            if (Input.mousePosition.x - Screen.width / 2 < 0)
            {
                transform.localScale = new Vector2(-1, 1);
            }
            anim.Play("Attack");
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            speed = 10f;
            dash = false;
        }

        if (isGrounded && horizontal > 0)
        {
            transform.localScale = new Vector2(1, 1);
        }

        if (horizontal < 0) 
        { 
            transform.localScale = new Vector2(-1, 1);
        }
        
        if (horizontal > 0) 
        { 
            transform.localScale = new Vector2(1, 1);
        }
        
        if (isGrounded && horizontal < 0) {
            transform.localScale = new Vector2(-1, 1);
        }

        if (isGrounded)
        {
            anim.SetFloat("Speed", Mathf.Abs(horizontal));
        }

        if (climbing && Input.GetKey(KeyCode.E))
        {
            rb.gravityScale = 0f;
            transform.Translate(Vector2.up * vertical * climbSpeed * Time.deltaTime);
        }

        if (climbing && Input.GetKeyUp(KeyCode.E))
        {
            climbing = false;
            rb.gravityScale = 3f;
        }
        
        transform.Translate(direction * speed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Tilemap"))
        {
            isJumping = false;
            isGrounded = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rope"))
        {
            climbing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        climbing = false;
        rb.gravityScale = 3f;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isJumping = true;
        isGrounded = false;
    }
}

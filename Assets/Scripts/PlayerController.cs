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
    public bool dash;
    public Rigidbody2D rb;
    private bool isRunning;
    void Update()
    {
        //GroundCheck();

        //Jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            isJumping = true; rb.velocity = Vector2.up * jumpForce;
        }
        
        //General movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        Vector2 direction = new Vector2(horizontal * Time.deltaTime, 0);

        if (dash)
        {
            dashCooldown = dashCooldownTime;
        }

        if (!dash)
        {
            dashCooldown -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log(Input.mousePosition);
            
            if (dashCooldown <= 0)
            {
                dash = true;

                if (Input.mousePosition.x - Screen.width/2 > 0) 
                { 
                    transform.localScale = new Vector2(1, 1);

                    rb.AddForce(Vector2.right * dashAcceleration); 
                }

                if (Input.mousePosition.x - Screen.width/2 < 0)
                {
                    transform.localScale = new Vector2(-1, 1);

                    rb.AddForce(Vector2.left * dashAcceleration); 
                }

                dash = false;
                dashCooldown = dashCooldownTime;
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
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

        transform.Translate(direction * speed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}

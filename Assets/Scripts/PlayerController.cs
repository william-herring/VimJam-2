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
    private static float health = 3f;
    public Animator anim;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float climbSpeed;
    [SerializeField] private LayerMask mask;
    public bool dash;
    public Rigidbody2D rb;
    private bool isRunning;
    private bool climbing;

    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite halfHeart;

    private void Start()
    {
        SetHearts(health);
    }
    void Update()
    {
        GroundCheck();

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
            anim.Play("Attack");
            if (Input.mousePosition.x - Screen.width / 2 > 0) 
            { 
                transform.localScale = new Vector2(1, 1);
                RaycastHit2D hit = Physics2D.CircleCast(transform.position, 3, Vector2.right * 2, LayerMask.NameToLayer("Enemy"));
                Debug.Log(hit.collider.name);

                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Enemies"))
                    {
                        hit.collider.GetComponent<Enemy>().DoDamage(1);
                    }
                }
            }

            if (Input.mousePosition.x - Screen.width / 2 < 0)
            {
                transform.localScale = new Vector2(-1, 1);
                
                RaycastHit2D hit = Physics2D.CircleCast(transform.position, 3, Vector2.right * 2, LayerMask.NameToLayer("Enemy"));
                Debug.Log(hit.collider.name);

                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Enemies"))
                    {
                        hit.collider.GetComponent<Enemy>().DoDamage(1);
                    }
                }
            }
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

    public void SetLives(float _lives)
    {
        health = _lives;
    }

    public void SetHearts(float lives)
    {
        if (lives % 1 == 5)
        {
            if (lives == 2.5)
            {
                hearts[0].GetComponent<Image>().sprite = fullHeart;
                hearts[1].GetComponent<Image>().sprite = fullHeart;
                hearts[2].GetComponent<Image>().sprite = halfHeart;
            }

            if (lives == 1.5)
            {
                hearts[0].GetComponent<Image>().sprite = fullHeart;
                hearts[1].GetComponent<Image>().sprite = halfHeart;
                hearts[2].SetActive(false);
            }

            if (lives == 0.5)
            {
                hearts[0].GetComponent<Image>().sprite = halfHeart;
                hearts[1].SetActive(false);
                hearts[2].SetActive(false);
            }

            return;
        }
        
        if (lives == 3)
        {
            hearts[0].GetComponent<Image>().sprite = fullHeart;
            hearts[1].GetComponent<Image>().sprite = fullHeart;
            hearts[2].GetComponent<Image>().sprite = fullHeart;
        }

        if (lives == 2)
        {
            hearts[0].GetComponent<Image>().sprite = fullHeart;
            hearts[1].GetComponent<Image>().sprite = fullHeart;
            hearts[2].SetActive(false);
        }

        if (lives == 1)
        {
            hearts[0].GetComponent<Image>().sprite = fullHeart;
            hearts[1].SetActive(false);
            hearts[2].SetActive(false);
        }
        
        if (lives == 0)
        {
            hearts[0].SetActive(false);
            hearts[1].SetActive(false);
            hearts[2].SetActive(false);
            
            GameObject.Find("Canvas").SetActive(false);
            deathScreen.SetActive(true);
            this.enabled = false;
        }
    }

    private void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2, mask);

        if (hit.collider != null && hit.collider.CompareTag("Tilemap"))
        {
            isJumping = false;
            isGrounded = true;
        } else if (hit.collider == null)
        {
            isJumping = true;
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Spikes")
        {
            health = 0f;
            SetHearts(health);
        }
        
        if (other.collider.tag == "Arrows")
        {
            health -= 0.5f;
            Destroy(other.gameObject);
            SetHearts(health);
        }
        
        if (other.collider.tag == "Enemies")
        {
            health -= 0.5f;
            SetHearts(health);
            other.gameObject.SetActive(true);
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
    
}

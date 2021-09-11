using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool isGrounded;
    public bool isJumping;
    public float jumpForce;
    public float speed = 5f;
    public float dashSpeed;
    public float dashAcceleration;
    public Rigidbody2D rb;
    void Update()
    {
        GroundCheck();

        //Jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            isJumping = true; rb.velocity = Vector2.up * jumpForce;
        }
        
        //General movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        Vector2 direction = new Vector2(horizontal * Time.deltaTime, 0); 
        transform.Translate(direction * speed);

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (speed < dashSpeed)
            {
                speed += dashAcceleration;
            } else if (speed >= dashSpeed)
            {
                speed = 10f;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            speed = 10f;
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
    }
    private void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.15f);

        if (hit.collider != null && hit.collider.CompareTag("Tilemap"))
        {
            isGrounded = true;
            isJumping = false;
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}

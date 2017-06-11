using UnityEngine;

public class PlatformerCharacter2D : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float jumpForce = 400f;
    [SerializeField] private LayerMask groundLayerMask;

    private Transform groundCheck;
    private const float groundedRadius = .2f;
    private bool isGrounded;
    private new Rigidbody2D rigidbody2D;
    private bool facingRight = true;

    private void Awake()
    {
        groundCheck = transform.Find("GroundCheck");
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundLayerMask);
        foreach (Collider2D collider2d in colliders)
        {
            if (collider2d.gameObject != gameObject)
            {
                isGrounded = true;
            }
        }
    }

    public void Move(float move, bool jump)
    {
        rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }


        if (!isGrounded || !jump) return;
        isGrounded = false;
        rigidbody2D.AddForce(new Vector2(0f, jumpForce));
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}

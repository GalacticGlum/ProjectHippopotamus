using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public bool IsGrounded { get; private set; }

    [SerializeField]
    private LayerMask groundLayerMask;

    private Transform groundCheck;
    private const float groundedRadius = .2f;
    private new Rigidbody2D rigidbody2D;
    private bool facingRight = true;
    private bool didPressJump;

    private void Awake()
    {
        groundCheck = transform.Find("GroundCheck");
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        IsGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundLayerMask);
        foreach (Collider2D collider2d in colliders)
        {
            if (collider2d.gameObject != gameObject)
            {
                IsGrounded = true;
            }
        }
    }

    public void Move(float move, bool jump, float speed, float jumpForce)
    {
        if (!IsGrounded || didPressJump)
        {
            if (move == 0) return;
            Vector2 direction = facingRight ? transform.right : -transform.right;
            rigidbody2D.velocity = new Vector2(Mathf.Sign(move), rigidbody2D.velocity.y) + direction * (speed / 2);

        }

        if(IsGrounded || !didPressJump)
        {
            didPressJump = false;
            rigidbody2D.velocity = new Vector2(move * speed, rigidbody2D.velocity.y);
        }

        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }

        if (!jump || !IsGrounded) return;

        IsGrounded = false;
        didPressJump = true;

        rigidbody2D.AddForce(new Vector2(move * speed, jumpForce));
        Player.Current.HasJumped();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}

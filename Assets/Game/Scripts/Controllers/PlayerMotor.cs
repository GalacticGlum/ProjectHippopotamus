using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMotor : MonoBehaviour
{
    public bool IsGrounded
    {
        get { return Physics2D.Raycast(groundCheck.position, -Vector2.up, distanceFromGround); }
    }

    [SerializeField]
    private LayerMask groundLayerMask;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float distanceFromGround = .2f;

    private new Rigidbody2D rigidbody2D;

    // Jump control
    private bool facingRight = true;
    private bool didPressJump;
    private int tickFromLastJump;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Move(float move, bool jump, bool isPowerJump, float speed, float jumpForce)
    {
        // Increment tick count
        tickFromLastJump++;

        if (!IsGrounded && didPressJump)
        {
            if (move == 0) return;
            Vector2 direction = facingRight ? transform.right : -transform.right;
            rigidbody2D.velocity = new Vector2(Mathf.Sign(move), rigidbody2D.velocity.y) + direction * (speed / 2);
        }
        else if ((IsGrounded || !IsGrounded && !didPressJump) && tickFromLastJump > 1)
        {
            rigidbody2D.velocity = new Vector2(move * speed, rigidbody2D.velocity.y);
            didPressJump = false;
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

        float force = 200;
        if (isPowerJump)
        {
            force = jumpForce;
            Player.Current.HasJumped(25);
        }

        Player.Current.HasJumped(5);
        rigidbody2D.AddForce(new Vector2(move * speed, force));
        tickFromLastJump = 0;
        didPressJump = true;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}

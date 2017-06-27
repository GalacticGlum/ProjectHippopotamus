using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

/* TODO:
 *      - Refactor movement code into the PlayerController.
 */
[RequireComponent(typeof(PlayerMotor))]
public class Player : MonoBehaviour
{
    public static Player Current { get; private set; }
    public CharacterAttributeContainer Attributes { get { return attributes; } }

    [SerializeField]
    private CharacterAttributeContainer attributes = new CharacterAttributeContainer();
    [SerializeField]
    private int jumpPowerCost = 25;
    [SerializeField]
    private float walkSpeed = 10f;
    [SerializeField]
    private float jumpMovementSpeed = 2f;
    [SerializeField]
    private float runSpeed = 20f;
    [SerializeField]
    private float jumpForce = 400f;
    [SerializeField]
    private float jumpDeteriorationPercent = 0.5f;

    private PlayerMotor motor;
    private bool jump;
    private float currentJumpForce;
    private float currentSpeed;

    private void Awake()
    {
        Current = this;

        motor = GetComponent<PlayerMotor>();
        attributes.Initialize();
        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        attributes.Update();
        if (!jump)
        {
            jump = Input.GetKeyDown(KeyCode.Space);
        }

        if (motor.IsGrounded)
        {
            currentJumpForce = jumpForce;
        }

        if (Attributes.Get("Energy").Value <= 0 || Attributes.Get("Energy").Value < jumpPowerCost)
        {
            jump = false;
        }
    }

    private void FixedUpdate()
    {
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        motor.Move(horizontal, jump, currentSpeed, currentJumpForce);

        jump = false;
    }

    public void HasJumped()
    {
        Attributes.Get("Energy").Modify(-jumpPowerCost);
    }
}


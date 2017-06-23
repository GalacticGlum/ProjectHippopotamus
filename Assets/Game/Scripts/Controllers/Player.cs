using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(PlayerMotor))]
public class Player : MonoBehaviour
{
    public float Power { get { return currentPower; } }
    public float MaximumPower{ get { return maximumPower; } }

    [SerializeField]
    private float maximumPower = 100f;
    [SerializeField]
    private float jumpPowerCost = 25f;
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
    private float currentPower;

    private void Awake()
    {
        currentPower = maximumPower;
        motor = GetComponent<PlayerMotor>();
    }

    private void Update()
    {
        if (!jump)
        {
            jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }
    }

    private void FixedUpdate()
    {
        if (motor.IsGrounded)
        {
            currentJumpForce = jumpForce;
            currentSpeed = walkSpeed;
        }

        if (currentPower <= 0)
        {
            jump = false;
        }

        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        motor.Move(horizontal, jump, currentSpeed, currentJumpForce);

        if (jump)
        {
            currentPower -= jumpPowerCost;
            //currentJumpForce *= jumpDeteriorationPercent;
        }

        jump = false;
    }
}


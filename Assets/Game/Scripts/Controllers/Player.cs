using System;
using UnityEngine;

public delegate void PlayerJumpedEventHandler(object sender, PlayerEventArgs args);
public class PlayerEventArgs : EventArgs
{
    public readonly Player Player;
    public PlayerEventArgs(Player player)
    {
        Player = player;
    }
}

/* TODO:
 *      - Refactor movement code into the PlayerController.
 *      - Jump audio played before actuall jump force.
 */
[RequireComponent(typeof(PlayerMotor))]
public class Player : MonoBehaviour
{
    public static Player Current { get; private set; }
    public CharacterAttributeContainer Attributes { get { return attributes; } }

    public event PlayerJumpedEventHandler Jumped;

    private void OnJumped()
    {
        if (Jumped == null) return;
        Jumped(this, new PlayerEventArgs(this));
    }

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
    private bool isPowerJump;
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
            isPowerJump = true;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            isPowerJump = false;
            jump = true;
        }

        if (motor.IsGrounded)
        {
            currentJumpForce = jumpForce;
        }

        if (Attributes.Get("Energy").Value <= 0 || Attributes.Get("Energy").Value < 10)
        {
            jump = false;
        }
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        motor.Move(horizontal, jump, isPowerJump, currentSpeed, currentJumpForce);

        jump = false;
        isPowerJump = false;
    }

    public void HasJumped(int cost)
    {
        OnJumped();
        Attributes.Get("Energy").Modify(-cost);
    }
}


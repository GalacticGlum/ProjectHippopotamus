using System;
using MoonSharp.Interpreter;
using UnityEngine;
using Random = UnityEngine.Random;

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
 *      - Jump audio played before actual jump force.
 */

[LuaExposeType]
[MoonSharpUserData]
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
    private bool canMove;

    private PlayerMotor motor;
    private bool jump;
    private bool isPowerJump;
    private float currentJumpForce;
    private float currentSpeed;

    private void Awake()
    {
        Current = this;

        canMove = true;
        motor = GetComponent<PlayerMotor>();
        attributes.Initialize();
        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        attributes.Update();

        if (!canMove) return;
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
        if(!canMove) return;

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

    public void Freeze(float duration = 0)
    {
        canMove = false;
        if (duration > 0)
        {
            Invoke("Unfreeze", duration);
        }
    }

    public void Unfreeze()
    {
        canMove = true;
    }

    public void Shock(float duration, float cameraShakeAmount = 0.5f, float cameraShakeDegradationFactor = 1)
    {
        Freeze(duration);
        CameraController.Instance.Freeze(duration);
        CameraShakeAgent.Create(duration, cameraShakeAmount, cameraShakeDegradationFactor);
    }

    public Tile GetTile()
    {
        return WorldController.Instance.GetTileFromWorldCoordinates(transform.position);
    }

    public Tile GetTileInChunk()
    {
        Tile tile = GetTile();
        if (tile == null) return null;

        Chunk chunk = tile.Chunk;
        return chunk.GetTileAt(Random.Range(0, Chunk.Size), Random.Range(0, Chunk.Size));
    }

    public Vector2i GetGridPosition()
    {
        return WorldController.Instance.WorldCoordiantesToGridSpace(transform.position);
    }
}


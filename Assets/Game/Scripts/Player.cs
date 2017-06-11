using UnityEngine;

public class Player
{
    public readonly GameObject CharacterGameObject;

    private bool isJumping;
    private const int jumpForce = 15;
    private const int speed = 5;

    public Player(Vector2i position)
    {
        CharacterGameObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/PlayerPrefab"), position.ToVector3(), Quaternion.identity);
    }

    public void Update()
    {

    }
}

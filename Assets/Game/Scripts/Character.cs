public delegate void CharacterCreatedEventHandler(object sender, CharacterEventArgs args);
public delegate void CharacterChangedEventHandler(object sender, CharacterEventArgs args);
public class CharacterEventArgs
{
    public readonly Character Character;
    public CharacterEventArgs(Character character)
    {
        Character = character;
    }
}

public class Character
{
    public Vector2i Position { get; private set; }
    public Chunk Chunk { get; private set; }

    public event CharacterChangedEventHandler CharacterChanged;
    public void OnCharacterChanged(CharacterEventArgs args)
    {
        if (CharacterChanged != null)
        {
            CharacterChanged(this, args);
        }
    }

    public Character(Vector2i position)
    {
        Position = position;
        Chunk = World.Current.GetChunkContaining(position.X, position.Y);
    }

    public void Update(float deltaTime)
    {
        OnCharacterChanged(new CharacterEventArgs(this));
    }
}


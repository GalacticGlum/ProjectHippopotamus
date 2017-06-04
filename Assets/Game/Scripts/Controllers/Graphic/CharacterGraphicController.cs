using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityUtilities.ObjectPool;

public class CharacterGraphicController
{
    private readonly Dictionary<Character, GameObject> characterGameObjectMap;
    private readonly GameObject characterParent;

    public CharacterGraphicController()
    {
        characterGameObjectMap = new Dictionary<Character, GameObject>();
        characterParent = new GameObject("Characters");

        World.Current.CharacterCreated += OnCharacterCreated;
        foreach (Character character in World.Current.Characters)
        {
            OnCharacterCreated(this, new CharacterEventArgs(character));
        }
    }

    private void OnCharacterCreated(object sender, CharacterEventArgs args)
    {
        GameObject characterGameObject = new GameObject("Character");
        characterGameObjectMap.Add(args.Character, characterGameObject);
        characterGameObject.transform.position = args.Character.Position.ToVector3();
        characterGameObject.transform.SetParent(characterParent.transform);

        SpriteRenderer spriteRenderer = characterGameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = SpriteManager.GetSprite("Character", "Player");

        args.Character.CharacterChanged += OnCharacterChanged;
    }

    private void OnCharacterChanged(object sender, CharacterEventArgs args)
    {
        if (!characterGameObjectMap.ContainsKey(args.Character)) return;
        characterGameObjectMap[args.Character].transform.position = args.Character.Position.ToVector3();
    }
}


using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterAttributeContainer : IEnumerable<CharacterAttribute>
{
    [SerializeField]
    private List<CharacterAttribute> characterAttributes = new List<CharacterAttribute>();

    public void Initialize()
    {
        foreach (CharacterAttribute attribute in this)
        {
            attribute.Initialize();
        }
    }

    public void Update()
    {
        foreach (CharacterAttribute attribute in this)
        {
            attribute.Update();
        }
    }

    public bool Contains(CharacterAttribute attribute)
    {
        return characterAttributes.Contains(attribute);
    }

    public CharacterAttribute Get(string name)
    {
        return characterAttributes.FirstOrDefault(attribute => attribute.Name == name);
    }

    public IEnumerator<CharacterAttribute> GetEnumerator()
    {
        return characterAttributes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

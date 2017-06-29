using UnityEngine;
using System;

public delegate void CharacterAttributeValueChangedEventHandler(object sender, CharacterAttributeValueChangedEventArgs args);
public class CharacterAttributeValueChangedEventArgs : EventArgs
{
    public readonly CharacterAttribute CharacterAttribute;
    public readonly int OldValue;
    public readonly int ByValue;

    public CharacterAttributeValueChangedEventArgs(CharacterAttribute characterAttribute, int oldValue, int byValue)
    {
        CharacterAttribute = characterAttribute;
        OldValue = oldValue;
        ByValue = byValue;
    }
}

[CreateAssetMenu(fileName = "Data", menuName = "Character Attribute")]
public class CharacterAttribute : ScriptableObject
{
    public string Name { get { return name; } }
    public int MinimumValue { get { return minimumValue; } }
    public int MaximumValue { get { return maximumValue; } }
    public int Value { get; private set; }

    public event CharacterAttributeValueChangedEventHandler ValueChanged;
    private void OnValueChanged(int oldValue, int byValue)
    {
        if (ValueChanged == null) return;
        ValueChanged(this, new CharacterAttributeValueChangedEventArgs(this, oldValue, byValue));
    }

    [SerializeField]
    private string name;
    [SerializeField]
    private int minimumValue;
    [SerializeField]
    private int maximumValue;
    [SerializeField]
    private bool defaultToMaximumValue = true;
    [SerializeField]
    [Range(0, 1)]
    private float regenerationPercentage;
    private float regenerationTimer;

    public void Initialize()
    {
        if (defaultToMaximumValue)
        {
            Value = maximumValue;
        }
    }

    public void Update()
    {
        regenerationTimer += Time.deltaTime;
        if (regenerationTimer >= 1)
        {
            Regenerate();
            regenerationTimer = 0;
        }
    }

    public void Modify(int byValue)
    {
        int oldValue = Value;
        Value += byValue;
        Value = Mathf.Clamp(Value, minimumValue, maximumValue);
        OnValueChanged(oldValue, byValue);
    }

    private void Regenerate()
    {
        if (regenerationPercentage <= 0) return;
        Modify((int)(maximumValue * regenerationPercentage));
    }
}

using System.Collections.Generic;
using System.Linq;
using MoonSharp.Interpreter;
using UnityEngine;

[MoonSharpUserData]
public class WorldEvent
{
    public string Name { get; private set; }
    public bool CanRepeat { get; set; }
    public int MaxRepeats { get; set; }
    public float Timer { get; set; }

    private readonly List<string> preconditions;
    private readonly List<string> executionActions;
    private readonly bool infiniteRepeats;
    private bool executed;
    private int repeatAmount;

    public WorldEvent(string name, bool canRepeat = false, int maxRepeats = 1)
    {
        Name = name;
        CanRepeat = canRepeat;
        MaxRepeats = maxRepeats;
        if (MaxRepeats == -1)
        {
            infiniteRepeats = true;
        }

        preconditions = new List<string>();
        executionActions = new List<string>();

        Timer = 0;
        repeatAmount = 0;
    }

    public void Update()
    {
        if (executed || MaxRepeats > 0 && repeatAmount >= MaxRepeats) return;
        if (preconditions.Any(precondition => !Lua.Call(precondition, this, Time.deltaTime).Boolean))
        {
            return;
        }

        if (!infiniteRepeats)
        {
            repeatAmount++;
        }
        Trigger();
    }


    private void Trigger()
    {
        if (executionActions != null)
        {
            Lua.Call(executionActions.ToArray(), this);
        }

        if (!CanRepeat && !infiniteRepeats)
        {
            executed = true;
        }
    }

    public void RegisterPreconditions(params string[] functions)
    {
        preconditions.AddRange(functions);
    }

    public void RegisterAction(params string[] functions)
    {
        executionActions.AddRange(functions);
    }
}

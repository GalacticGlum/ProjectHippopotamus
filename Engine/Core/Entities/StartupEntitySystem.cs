using System;

namespace Hippopotamus.Engine.Core
{
    /// <summary>
    /// Registers the entity system on startup.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class StartupEntitySystem : Attribute { }
}

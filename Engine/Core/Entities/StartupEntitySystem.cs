using System;

namespace Hippopotamus.Engine.Core
{
    /// <summary>
    /// Registers the entity system on startup.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class StartupEntitySystem : Attribute
    {
        public uint ExecutionLayer { get; }
        public StartupEntitySystem(uint executionLayer = 0)
        {
            ExecutionLayer = executionLayer;
        }
    }
}

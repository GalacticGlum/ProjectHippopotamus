using System;

namespace Hippopotamus.Engine.Core.Exceptions
{
    public class EntitySystemNotFoundException : Exception
    {
        public EntitySystemNotFoundException(Type systemType)
            : base($"Entity System of type: \"{systemType.Name}\" not found in the Entity System manager.")
        {
            
        }
    }
}

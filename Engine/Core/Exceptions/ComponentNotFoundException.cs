using System;

namespace Hippopotamus.Engine.Core.Exceptions
{
    public class ComponentNotFoundException : Exception
    {
        public ComponentNotFoundException(Entity entity, Type componentType)
            : base($"Could not find a component of type \"{componentType.Name}\" on entity: \"{entity.Name}\"")
        {
            
        }
    }
}

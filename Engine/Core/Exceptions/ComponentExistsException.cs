using System;

namespace Hippopotamus.Engine.Core.Exceptions
{
    public class ComponentExistsException : Exception
    {
        public ComponentExistsException(Entity entity, Type componentType)
            : base($"Entity: \"{entity.Name}\" already has a componentType of type \"{componentType.Name}\"")
        {
            
        }
    }
}

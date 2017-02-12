using System;

namespace Hippopotamus.Engine.Core.Exceptions
{
    public class InvalidEntityPoolException : Exception
    {
        public InvalidEntityPoolException(Entity entity)
            : base($"Entity: \"{entity.Name}\" does not belong to an entity pool!")
        {
            
        }
    }
}

using System;

namespace Hippopotamus.Engine.Core.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(EntityPool pool) : base($"GameObject not found in pool: \"{pool.Name}\".")
        {
                        
        }
    }
}

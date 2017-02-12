using System;

namespace Hippopotamus.Engine.Core.Exceptions
{
    public class DuplicateEntityException : Exception
    {
        public DuplicateEntityException(EntityPool pool, string entityName)
            : base($"EntityPool: \"{pool.Name}\" already contains an entity with name \"{entityName}\"")
        {
            
        }
    }
}

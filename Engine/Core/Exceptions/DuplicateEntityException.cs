using System;

namespace Hippopotamus.Engine.Core.Exceptions
{
    public class DuplicateEntityException : Exception
    {
        public DuplicateEntityException(string entityName)
            : base($"An entity with name \"{entityName}\" already exists!")
        {         
        }
    }
}

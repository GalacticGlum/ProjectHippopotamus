using System;

namespace Hippopotamus.Engine.Core.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string searchTerm) : base($"Entity of name: \"{searchTerm}\" not found.")
        {                     
        }
    }
}

using System;

namespace Hippopotamus.Engine.Core.Exceptions
{
    public class NullEntityPoolException : Exception
    {
        public NullEntityPoolException(EntityPool pool) : base($"Entity Pool: \"{pool.Name}\" was null.")
        {
            
        }
    }
}

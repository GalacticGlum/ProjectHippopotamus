using System;
using Hippopotamus.Engine.Core;

namespace Hippopotamus.Engine.Utilities
{
    public static class ComponentExtensions
    {
        public static bool IsComponent(this Type type) => typeof(Component).IsAssignableFrom(type);
    }
}

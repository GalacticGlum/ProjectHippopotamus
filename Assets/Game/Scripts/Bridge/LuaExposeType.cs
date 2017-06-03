using System;

namespace Hippopotamus.Engine.Bridge
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
    public class LuaExposeType : Attribute
    {
    }
}

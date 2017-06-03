using System;
using System.Reflection;
using MoonSharp.Interpreter;


internal static class LuaApiHandler
{
    public static void Register()
    {
        // Expose all types decorated with the LuaExposeType attribute.
        Type exposeAttributeType = typeof(LuaExposeType);
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (Attribute.IsDefined(type, exposeAttributeType))
                {
                    Lua.ExposeType(type);
                }
            }
        }

        // TODO: Move math to a Lua script. 
        Lua.ExposeType(typeof(MathHelper));
        Lua.ExposeType(typeof(Math));

        UserData.RegisterAssembly();
    }
}

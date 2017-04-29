﻿using System;
using System.Reflection;
using MoonSharp.Interpreter;

namespace Hippopotamus.Engine.Bridge
{
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

            UserData.RegisterAssembly();
        }
    }
}

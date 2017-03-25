using System;
using System.Collections.Generic;
using System.IO;
using MoonSharp.Interpreter;

namespace Hippopotamus.Engine.Bridge
{
    public static class Lua
    {
        private static readonly List<string> parsedFilePaths;
        private static readonly Script lua;

        static Lua()
        {
            lua = new Script();
            parsedFilePaths = new List<string>();
        }

        public static void Parse(string filePath)
        {
            if (parsedFilePaths.Contains(filePath)) return;
            parsedFilePaths.Add(filePath);

            try
            {
                lua.DoString(File.ReadAllText(Path.Combine("Content", filePath)));
            }
            catch (InterpreterException e)
            {
                Log(e);
            }
            catch (Exception e)
            {
                Log(e);
            }
        }

        public static DynValue Call(string function, params object[] args)
        {
            DynValue result = DynValue.Nil;
            try
            {
                result = lua.Call(lua.Globals[function], args);
            }
            catch (InterpreterException e)
            {
                Log(e);
            }
            catch (Exception e)
            {
                Log(e);
            }

            return result;
        }

        public static DynValue[] Call(string[] functions, params object[] args)
        {
            DynValue[] result = new DynValue[functions.Length];
            for (int i = 0; i < functions.Length; i++)
            {
                result[i] = Call(functions[i], args);
            }

            return result;
        }

        public static DynValue Call(Closure function, params object[] args)
        {
            DynValue result = DynValue.Nil;
            try
            {
                result = function.Call(args);
            }
            catch (InterpreterException e)
            {
                Log(e);
            }
            catch (Exception e)
            {
                Log(e);
            }

            return result;
        }

        public static Closure GetFunction(string functionName)
        {
            Closure function = (Closure) lua.Globals[functionName];
            if (function == null)
            {
                Console.WriteLine($"Lua::GetFunction: Tried to get non-existent function of name: \"{functionName}\".");
            }

            return function;
        }

        private static void Log(InterpreterException e)
        {
            string decoratedMessage = e.DecoratedMessage;
            string culpritFilePath = parsedFilePaths[int.Parse(decoratedMessage.Substring(6, decoratedMessage.IndexOf(":", StringComparison.Ordinal) - 6)) - 1];
            Console.WriteLine($"{decoratedMessage}\n at {culpritFilePath}");
        }

        private static void Log(Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}

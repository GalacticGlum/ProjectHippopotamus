using Hippopotamus.Engine.Bridge;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;

namespace Hippopotamus.Engine
{
    [LuaExposeType]
    [MoonSharpUserData]
    public struct Vector2i
    {
        public int X;
        public int Y;

        public Vector2i(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }
    }
}

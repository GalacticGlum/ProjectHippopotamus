using Hippopotamus.Engine.Bridge;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;

namespace Hippopotamus.Engine
{
    [LuaExposeType]
    [MoonSharpUserData]
    public struct Vector2i
    {
        public static Vector2i Zero { get; } = new Vector2i(0);
        public static Vector2i One { get; } = new Vector2i(1);

        public int X;
        public int Y;

        public Vector2i(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector2i(int scalar)
        {
            X = scalar;
            Y = scalar;
        }

        public Vector2i Negative()
        {
            return new Vector2i(-X, -Y);
        }

        public Vector2i Negate()
        {
            X *= -1;
            Y *= -1;

            return this;
        }

        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }
    }
}

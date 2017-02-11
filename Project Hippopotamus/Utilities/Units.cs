using Microsoft.Xna.Framework;

namespace Hippopotamus.Utilities
{
    public static class Units
    {
        private const int PixelsPerUnit = 32;

        public static float ToPixels(float value)
        {
            return value * PixelsPerUnit;
        }

        public static Vector2 ToPixels(Vector2 value)
        {
            return value * PixelsPerUnit;
        }

        public static float ToMeters(float value)
        {
            return value / PixelsPerUnit;
        }

        public static Vector2 ToMeters(Vector2 value)
        {
            return value / PixelsPerUnit;
        }
    }
}

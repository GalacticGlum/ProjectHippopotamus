using Microsoft.Xna.Framework;

namespace Hippopotamus.Engine.Utilities
{
    public static class TextureUtilities
    {
        public static Color[] GetTextureData(Color[] texturePixelData, int textureWidth, Rectangle destination)
        {
            Color[] pixels = new Color[destination.Width * destination.Height];
            for (int x = 0; x < destination.Width; x++)
            {
                for (int y = 0; y < destination.Height; y++)
                {
                    pixels[x + y * destination.Width] = texturePixelData[x + destination.X + (y + destination.Y) * textureWidth];
                }
            }

            return pixels;
        }
    }
}

using Hippopotamus.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hippopotamus.Engine.Utilities
{
    public static class TextureUtilities
    {
        public static Texture2D GetCroppedTexture(Texture2D texture, Rectangle source)
        {
            Texture2D cropTexture = new Texture2D(GameEngine.Context.GraphicsDevice, source.Width, source.Height);
            Color[] pixels = new Color[source.Width * source.Height];
            texture.GetData(0, source, pixels, 0, pixels.Length);
            cropTexture.SetData(pixels);

            return cropTexture;
        }
    }
}

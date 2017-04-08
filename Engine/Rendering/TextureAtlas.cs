using System.Collections.Generic;
using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hippopotamus.Engine.Rendering
{
    public class TextureAtlas
    {
        private readonly Color[] atlasPixels;
        private readonly Vector2i atlasSize;

        private readonly Dictionary<Vector2i, Texture2D> textures;
        private readonly Vector2i individualTextureSize;

        public TextureAtlas(string filePath, Vector2i individualTextureSize)
        {
            this.individualTextureSize = individualTextureSize;
            textures = new Dictionary<Vector2i, Texture2D>();

            // Copy our pixel data to a colour array
            Texture2D texture = GameEngine.Context.Content.Load<Texture2D>(filePath);
            atlasPixels = new Color[texture.Width * texture.Height];
            texture.GetData(atlasPixels);

            atlasSize = new Vector2i(texture.Width, texture.Height);
        }

        public Texture2D Get(int x, int y)
        {
            return Get(new Vector2i(x, y));
        }

        public Texture2D Get(Vector2i position)
        {
            if (textures.ContainsKey(position)) return textures[position];

            Color[] pixels = TextureUtilities.GetTextureData(atlasPixels, atlasSize.Y,new Rectangle(position.X, position.Y, individualTextureSize.X, individualTextureSize.Y));
            Texture2D texture = new Texture2D(GameEngine.Context.GraphicsDevice, individualTextureSize.X, individualTextureSize.Y);
            texture.SetData(pixels);

            return texture;
        }
    }
}

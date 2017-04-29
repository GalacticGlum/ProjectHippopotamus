using System.Collections.Generic;
using System.IO;
using System.Xml;
using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hippopotamus.Engine.Rendering
{
    public class TextureAtlas
    {
        private Texture2D atlas;
        private readonly Dictionary<string, Texture2D> textures;
        private Vector2i individualTextureSize;

        public TextureAtlas(string filePath)
        {
            textures = new Dictionary<string, Texture2D>();

            filePath = Path.Combine("Content", filePath);
            XmlUtilities.Read("TextureAtlas", "Texture", filePath, Load, SetupLoad);
        }

        private void SetupLoad(XmlReader reader)
        {
            atlas = GameEngine.Context.Content.Load<Texture2D>(reader.GetAttribute("FilePath"));
            individualTextureSize = new Vector2i(int.Parse(reader.GetAttribute("TextureWidth")), int.Parse(reader.GetAttribute("TextureHeight")));
        }

        private void Load(XmlReader reader)
        {
            string name = reader.GetAttribute("Name");
            int x = int.Parse(reader.GetAttribute("X"));
            int y = int.Parse(reader.GetAttribute("Y"));

            if (textures.ContainsKey(name)) return;
            Texture2D texture = TextureUtilities.GetCroppedTexture(atlas, new Rectangle(x * individualTextureSize.X, y * individualTextureSize.Y,
                                                                                        individualTextureSize.X, individualTextureSize.Y));
            textures.Add(name, texture);
        }

        public Texture2D Get(string name)
        {
            if (textures.ContainsKey(name)) return textures[name];

            Logger.Log("Engine", $"Could not find texture of name: {name} in atlas.", LoggerVerbosity.Warning);
            return null;
        }
    }
}

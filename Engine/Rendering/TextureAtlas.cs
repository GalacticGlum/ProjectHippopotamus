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
        private readonly Texture2D atlas;

        private readonly Dictionary<string, Texture2D> textures;
        private readonly Vector2i individualTextureSize;

        public TextureAtlas(string filePath)
        {
            textures = new Dictionary<string, Texture2D>();

            filePath = Path.Combine("Content", filePath);
            using (XmlReader reader = new XmlTextReader(new StreamReader(filePath)))
            {
                if (!reader.ReadToDescendant("TextureAtlas")) return;

                atlas = GameEngine.Context.Content.Load<Texture2D>(reader.GetAttribute("FilePath"));
                individualTextureSize = new Vector2i(int.Parse(reader.GetAttribute("TextureWidth")), int.Parse(reader.GetAttribute("TextureHeight")));

                if (!reader.ReadToDescendant("Texture")) return;


                do
                {
                    string name = reader.GetAttribute("Name");
                    int x = int.Parse(reader.GetAttribute("X"));
                    int y = int.Parse(reader.GetAttribute("Y"));

                    Load(name, x, y);
                }
                while (reader.ReadToNextSibling("Texture"));
            }
        }

        private void Load(string name, int x, int y)
        {
            if (textures.ContainsKey(name)) return;

            Texture2D texture = TextureUtilities.GetCroppedTexture(atlas, new Rectangle(x * individualTextureSize.X, y * individualTextureSize.Y,
                                                                                        individualTextureSize.X, individualTextureSize.Y));
            textures.Add(name, texture);
        }

        public Texture2D Get(string name)
        {
            if (textures.ContainsKey(name)) return textures[name];

            Logger.Log("Engine", $"Could not find texture of name: {name} in atlas.",  LoggerVerbosity.Warning);
            return null;
        }
    }
}

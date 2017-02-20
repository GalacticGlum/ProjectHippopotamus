using System.Collections.Generic;
using Hippopotamus.Engine.Core;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ninject;

namespace Hippopotamus.Engine.Rendering
{
    public static class FontManager
    {
        private static readonly Dictionary<string, SpriteFont> fonts;

        static FontManager()
        {
            fonts = new Dictionary<string, SpriteFont>();
        }

        public static void Load(string font, string fontPath)
        {
            if(fonts.ContainsKey(font)) return;
            fonts.Add(font, DependencyInjector.Kernel.Get<ContentManager>().Load<SpriteFont>(fontPath));
        }

        public static SpriteFont Get(string font)
        {
            return !fonts.ContainsKey(font) ? null : fonts[font];
        }
    }
}

using Hippopotamus.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hippopotamus.Engine.Rendering
{
    public class SpriteRenderer : Component
    {
        public Color Colour { get; set; }
        public Texture2D Texture { get; set; }
        public float Layer { get; set; }

        public SpriteRenderer(Texture2D texture)
        {
            Texture = texture;
            Colour = Color.White;
        }
    }
}

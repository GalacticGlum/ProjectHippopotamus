using Hippopotamus.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hippopotamus.Engine.Rendering
{
    public class SpriteRenderer : Component
    {
        public Color Colour { get; set; }
        public Texture2D Texture { get; set; }
        public int Layer { get; set; }

        public SpriteRenderer()
        {
            Reset();
        }

        public sealed override void Reset()
        {
            Texture = null;
            Colour = Color.White;
        }
    }
}

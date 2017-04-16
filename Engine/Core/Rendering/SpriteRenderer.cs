using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Core.Rendering;
using Microsoft.Xna.Framework;

namespace Hippopotamus.Engine.Rendering
{
    public class SpriteRenderer : Component
    {
        public Color Colour { get; set; }
        public Sprite Sprite { get; set; }
        public int Layer { get; set; }

        public SpriteRenderer()
        {
            Reset();
        }

        public sealed override void Reset()
        {
            Sprite.Texture = null;
            Colour = Color.White;
        }
    }
}

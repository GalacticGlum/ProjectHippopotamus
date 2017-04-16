using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hippopotamus.Engine.Core.Rendering
{
    public class Sprite
    {
        private Texture2D texture;
        public Texture2D Texture
        {
            get { return texture; }
            set
            {
                texture = value;

                if (texture == null) return;
                pivot = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            }
        }

        private Vector2 pivot;
        public Vector2 Pivot => texture != null ? pivot : Vector2.Zero;
        public int Width => texture?.Width ?? 0;
        public int Height => texture?.Height ?? 0;
    }
}

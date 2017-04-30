using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.UI.Framework.Rendering
{
    public abstract class Renderer
    {
        protected Texture2D ImageMap { get; set; }
        protected Renderer(Texture2D imageMap)
        {
            ImageMap = imageMap;
        }

        public abstract Rectangle CalculateChildArea(int width, int height);
        public abstract void Draw(SpriteBatch spriteBatch, Rectangle destinationArea);
    }
}

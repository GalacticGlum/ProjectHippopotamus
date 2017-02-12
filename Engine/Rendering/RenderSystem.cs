using Ninject;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Hippopotamus.Engine.Core;

namespace Hippopotamus.Engine.Rendering
{
    public class RenderSystem : EntitySystem
    {
        private readonly SpriteBatch spriteBatch;

        public RenderSystem() : base(typeof(SpriteRenderer))
        {
            spriteBatch = DependencyInjector.Kernel.Get<SpriteBatch>();
        }

        public void Draw()
        {
            spriteBatch.Begin();
            foreach (Entity entity in CompatibleEntities)
            {
                SpriteRenderer spriteRenderer = entity.GetComponent<SpriteRenderer>();
                spriteBatch.Draw(spriteRenderer.Texture, spriteRenderer.Transform.Position, null,
                    spriteRenderer.Colour, spriteRenderer.Transform.Rotation,
                    new Vector2(spriteRenderer.Texture.Width / 2.0f, spriteRenderer.Texture.Height / 2.0f),
                    spriteRenderer.Transform.Size, SpriteEffects.None, spriteRenderer.Layer);
            }
            spriteBatch.End();
        }
    }
}

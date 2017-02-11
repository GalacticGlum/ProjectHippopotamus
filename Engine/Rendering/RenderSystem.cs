using Ninject;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Hippopotamus.Engine.Core;
using Hippopoutamus.Engine.Core;

namespace Hippopotamus.Engine.Rendering
{
    public class RenderSystem : System<SpriteRenderer>
    {
        private readonly SpriteBatch spriteBatch;
        private readonly Dictionary<GameObject, SpriteRenderer> spriteRenderers;

        public RenderSystem()
        {
            spriteBatch = DependencyInjector.Kernel.Get<SpriteBatch>();
            spriteRenderers = new Dictionary<GameObject, SpriteRenderer>();
        }

        public override void Update(GameObject gameObject)
        {
            if (!spriteRenderers.ContainsKey(gameObject))
            {
                spriteRenderers.Add(gameObject, gameObject.GetComponent<SpriteRenderer>());
            }

            spriteBatch.Begin();
            SpriteRenderer spriteRenderer = spriteRenderers[gameObject];
            spriteBatch.Draw(spriteRenderer.Texture, spriteRenderer.Transform.Position, null, 
                spriteRenderer.Colour, spriteRenderer.Transform.Rotation, 
                new Vector2(spriteRenderer.Texture.Width / 2.0f, spriteRenderer.Texture.Height / 2.0f), 
                spriteRenderer.Transform.Size, SpriteEffects.None, spriteRenderer.Layer);
            spriteBatch.End();
        }
    }
}

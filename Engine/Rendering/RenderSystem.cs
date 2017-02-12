using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Hippopotamus.Engine.Core;
using IDrawable = Hippopotamus.Engine.Core.IDrawable;

namespace Hippopotamus.Engine.Rendering
{
    [StartupEntitySystem]
    public class RenderSystem : EntitySystem, IDrawable
    {
        public RenderSystem() : base(typeof(SpriteRenderer))
        {
        }

        public void Draw(object sender, GameLoopDrawEventArgs args)
        {
            args.SpriteBatch.Begin();
            foreach (Entity entity in CompatibleEntities)
            {
                SpriteRenderer spriteRenderer = entity.GetComponent<SpriteRenderer>();
                args.SpriteBatch.Draw(spriteRenderer.Texture, spriteRenderer.Transform.Position, null,
                    spriteRenderer.Colour, spriteRenderer.Transform.Rotation,
                    new Vector2(spriteRenderer.Texture.Width / 2.0f, spriteRenderer.Texture.Height / 2.0f),
                    spriteRenderer.Transform.Size, SpriteEffects.None, spriteRenderer.Layer);
            }
            args.SpriteBatch.End();
        }
    }
}

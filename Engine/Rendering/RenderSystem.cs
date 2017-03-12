using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Core.Entities;
using IDrawable = Hippopotamus.Engine.Core.IDrawable;

namespace Hippopotamus.Engine.Rendering
{
    [StartupEntitySystem]
    public class RenderSystem : EntitySystem, IDrawable
    {
        public RenderSystem()
        {
            FontManager.Load("Arial", "Fonts/Arial");
        }

        public void Draw(GameLoopEventArgs args)
        {
            args.SpriteBatch.Begin(transformMatrix: Camera.Main.ViewMatrix, samplerState: SamplerState.PointClamp);
            foreach (Entity entity in EntityPool.GetGroup(typeof(SpriteRenderer)))
            {
                DrawSprite(args.SpriteBatch, entity);
            }

            foreach (Entity entity in EntityPool.GetGroup(typeof(Text)))
            {
                DrawFont(args.SpriteBatch, entity);
            }
            args.SpriteBatch.End();
        }

        private static void DrawSprite(SpriteBatch spriteBatch, Entity entity)
        {
            SpriteRenderer spriteRenderer = entity.GetComponent<SpriteRenderer>();
            if (spriteRenderer?.Texture == null) return;

            spriteBatch.Draw(spriteRenderer.Texture, spriteRenderer.Transform.Position, null,
                spriteRenderer.Colour, spriteRenderer.Transform.Rotation,
                new Vector2(spriteRenderer.Texture.Width / 2.0f, spriteRenderer.Texture.Height / 2.0f),
                spriteRenderer.Transform.Size, SpriteEffects.None, spriteRenderer.Layer);
        }

        private static void DrawFont(SpriteBatch spriteBatch, Entity entity)
        {
            Text text = entity.GetComponent<Text>();
            if (string.IsNullOrEmpty(text?.Message)) return;

            spriteBatch.DrawString(FontManager.Get(text.Font), text.Message, entity.Transform.Position, 
                Color.Black, entity.Transform.Rotation, Vector2.Zero, entity.Transform.Size, SpriteEffects.None, 0);
        }
    }
}

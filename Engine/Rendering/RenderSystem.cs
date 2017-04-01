using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Core.Entities;

namespace Hippopotamus.Engine.Rendering
{
    [StartupEntitySystem]
    public class RenderSystem : EntitySystem
    {
        private readonly SortedDictionary<int, HashSet<SpriteRenderer>> spriteRenderers;

        public RenderSystem() 
        {
            FontManager.Load("Arial", "Fonts/Arial");
            EntityPool.ComponentAdded += OnComponentAdded;
            EntityPool.ComponentRemoved += OnComponentRemoved;

            spriteRenderers = new SortedDictionary<int, HashSet<SpriteRenderer>>();
        }

        public override void Draw(GameLoopEventArgs args)
        {
            args.SpriteBatch.Begin(transformMatrix: Camera.Main.ViewMatrix, samplerState: SamplerState.PointClamp);
            foreach (KeyValuePair<int, HashSet<SpriteRenderer>> pair in spriteRenderers)
            {
                foreach (SpriteRenderer spriteRenderer in pair.Value)
                {
                    DrawSprite(args.SpriteBatch, spriteRenderer);
                }
            }

            //foreach (Entity entity in EntityPool.GetGroup(typeof(SpriteRenderer)))
            //{
            //    DrawSprite(args.SpriteBatch, entity.GetComponent<SpriteRenderer>());
            //}

            foreach (Entity entity in EntityPool.GetGroup(typeof(Text)))
            {
                DrawFont(args.SpriteBatch, entity);
            } 
            args.SpriteBatch.End();
        }

        private static void DrawSprite(SpriteBatch spriteBatch, SpriteRenderer spriteRenderer)
        {
            if (spriteRenderer?.Texture == null) return;

            spriteBatch.Draw(spriteRenderer.Texture, spriteRenderer.Transform.Position, null,
                spriteRenderer.Colour, spriteRenderer.Transform.Rotation,
                new Vector2(spriteRenderer.Texture.Width / 2.0f, spriteRenderer.Texture.Height / 2.0f),
                spriteRenderer.Transform.Size, SpriteEffects.None, 0f);
        }

        private static void DrawFont(SpriteBatch spriteBatch, Entity entity)
        {
            Text text = entity.GetComponent<Text>();
            if (string.IsNullOrEmpty(text?.Message)) return;

            spriteBatch.DrawString(FontManager.Get(text.Font), text.Message, entity.Transform.Position, 
                Color.Black, entity.Transform.Rotation, Vector2.Zero, entity.Transform.Size, SpriteEffects.None, 0);
        }

        private void OnComponentAdded(ComponentChangedEventArgs args)
        {
            if (args.Component.GetType() != typeof(SpriteRenderer)) return;
            SpriteRenderer spriteRenderer = (SpriteRenderer)args.Component;

            if (spriteRenderers.ContainsKey(spriteRenderer.Layer))
            {
                spriteRenderers[spriteRenderer.Layer].Add(spriteRenderer);
            }
            else
            {
                spriteRenderers.Add(spriteRenderer.Layer, new HashSet<SpriteRenderer> { spriteRenderer });
            }
        }

        private void OnComponentRemoved(ComponentChangedEventArgs args)
        {
            if (args.Component.GetType() != typeof(SpriteRenderer)) return;

            SpriteRenderer spriteRenderer = (SpriteRenderer)args.Component;
            if (spriteRenderers.ContainsKey(spriteRenderer.Layer))
            {
                spriteRenderers[spriteRenderer.Layer].Remove(spriteRenderer);
            }
        }
    }
}

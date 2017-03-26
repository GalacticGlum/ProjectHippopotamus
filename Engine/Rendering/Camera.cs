﻿using Hippopotamus.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hippopotamus.Engine.Rendering
{
    public class Camera : Component
    {
        private static Camera main;
        public static Camera Main
        {
            get
            {
                if (main == null)
                {
                    EntityPool.Create("Main Camera").AddComponent(new Camera(GameEngine.Context.GraphicsDevice.Viewport));
                }

                return main;
            }
        }

        public float OrthographicSize;
        public Vector2 Origin { get; set; }

        internal Matrix ViewMatrix => Matrix.CreateTranslation(new Vector3(-Transform.Position, 0.0f)) *
                                      Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                                      Matrix.CreateRotationZ(Transform.Rotation) *
                                      Matrix.CreateScale(OrthographicSize, OrthographicSize, 1) *
                                      Matrix.CreateTranslation(new Vector3(Origin, 1));

        public Camera(Viewport viewport)
        {
            OrthographicSize = 1;
            Origin  = new Vector2(viewport.Width / 2.0f, viewport.Height / 2.0f);

            main = this;
        }

        public Vector2 ScreenToWorldPoint(Vector2 screenPoint)
        {
            return Transform.Position + screenPoint;
        }
    }
}

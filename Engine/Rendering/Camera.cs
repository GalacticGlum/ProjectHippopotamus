using Hippopotamus.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hippopotamus.Engine.Rendering
{
    public class Camera : Component
    {
        public static Camera Main { get; private set; }

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

            Main = this;
        }
    }
}

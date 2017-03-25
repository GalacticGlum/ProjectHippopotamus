using System;
using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Core.Entities;
using Hippopotamus.Engine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Hippopotamus
{
    [StartupEntitySystem]
    public class InputSystem : EntitySystem
    {
        public override void Update(GameLoopEventArgs args)
        {
            const float speed = 100;

            Vector2 targetPosition = Vector2.Zero;
            if (Input.GetKey(Keys.A)) targetPosition += new Vector2(-speed * args.DeltaTime, 0);
            if (Input.GetKey(Keys.D)) targetPosition += new Vector2(speed * args.DeltaTime, 0);
            if (Input.GetKey(Keys.W)) targetPosition += new Vector2(0, -speed * args.DeltaTime);
            if (Input.GetKey(Keys.S)) targetPosition += new Vector2(0, speed * args.DeltaTime);

            Camera.Main.Transform.Position += new Vector2((float)Math.Round(targetPosition.X), (float)Math.Round(targetPosition.Y));
        }
    }
}

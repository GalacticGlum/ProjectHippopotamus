using System;
using Hippopotamus.Components;
using Hippopotamus.Engine.Core;
using Microsoft.Xna.Framework.Input;

namespace Hippopotamus.Systems
{
    public class MovementSystem : EntitySystem
    {
        public MovementSystem() : base(typeof(Player))
        {
        }

        public void Update(Entity entity)
        {
            Player player = entity.GetComponent<Player>();
            if (Input.GetKey(Keys.A))
            {
                entity.Transform.Translate(-player.Speed, 0);
            }

            if (Input.GetKey(Keys.D))
            {
                entity.Transform.Translate(player.Speed, 0);
            }

            if (Input.GetKey(Keys.W))
            {
                entity.Transform.Translate(0, -player.Speed);
            }

            if (Input.GetKey(Keys.S))
            {
                entity.Transform.Translate(0, player.Speed);
            }
        }
    }
}

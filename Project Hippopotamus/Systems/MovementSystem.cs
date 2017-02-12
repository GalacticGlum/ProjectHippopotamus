using Hippopotamus.Components;
using Hippopotamus.Engine.Core;
using Microsoft.Xna.Framework.Input;

namespace Hippopotamus.Systems
{
    [StartupEntitySystem]
    public class MovementSystem : EntitySystem, IUpdatable
    {
        public MovementSystem() : base(typeof(Player))
        {
        }

        public void Update(object sender, GameLoopUpdateEventArgs args)
        {
            foreach (Entity entity in CompatibleEntities)
            {
                Player player = entity.GetComponent<Player>();
                if (Input.GetKey(Keys.A))
                {
                    entity.Transform.Translate(-player.Speed * args.DeltaTime, 0);
                }

                if (Input.GetKey(Keys.D))
                {
                    entity.Transform.Translate(player.Speed * args.DeltaTime, 0);
                }

                if (Input.GetKey(Keys.W))
                {
                    entity.Transform.Translate(0, -player.Speed * args.DeltaTime);
                }

                if (Input.GetKey(Keys.S))
                {
                    entity.Transform.Translate(0, player.Speed * args.DeltaTime);
                }
            }
        }
    }
}

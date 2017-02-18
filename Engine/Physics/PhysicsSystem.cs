using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Core.Entities;
using Microsoft.Xna.Framework;

namespace Hippopotamus.Engine.Physics
{
    public class PhysicsSystem : EntitySystem, IFixedUpdatable
    {
        public World PhysicsWorld { get; }

        public PhysicsSystem() : base(typeof(Rigidbody))
        {
            PhysicsWorld = new World(new Vector2(0, 1f));
        }

        public void FixedUpdate(object sender, GameLoopFixedUpdateEventArgs args)
        {
            foreach (Entity entity in CompatibleEntities)
            {
                entity.Transform.Position = ConvertUnits.ToDisplayUnits(entity.GetComponent<Rigidbody>().Body.Position);
            }

            PhysicsWorld.Step(args.FixedDeltaTime);
        }
    }
}

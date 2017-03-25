using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Core.Entities;
using Microsoft.Xna.Framework;

namespace Hippopotamus.Engine.Physics
{
    public class PhysicsSystem : EntitySystem
    {
        public World PhysicsWorld { get; }

        public PhysicsSystem() 
        {
            PhysicsWorld = new World(new Vector2(0, 1f));
        }

        public override void FixedUpdate(GameLoopEventArgs args)
        {
            foreach (Entity entity in EntityPool.GetGroup(typeof(Rigidbody)))
            {
                entity.Transform.Position = ConvertUnits.ToDisplayUnits(entity.GetComponent<Rigidbody>().Body.Position);
            }

            PhysicsWorld.Step(args.FixedDeltaTime);
        }
    }
}

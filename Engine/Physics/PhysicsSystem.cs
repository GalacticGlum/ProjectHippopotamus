﻿using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Core.Entities;
using Microsoft.Xna.Framework;

namespace Hippopotamus.Engine.Physics
{
    public class PhysicsSystem : EntitySystem, IFixedUpdatable
    {
        public World PhysicsWorld { get; }

        public PhysicsSystem() 
        {
            PhysicsWorld = new World(new Vector2(0, 1f));
        }

        public void FixedUpdate(GameLoopFixedUpdateEventArgs args)
        {
            foreach (Entity entity in Pool.GetGroup(typeof(Rigidbody)))
            {
                entity.Transform.Position = ConvertUnits.ToDisplayUnits(entity.GetComponent<Rigidbody>().Body.Position);
            }

            PhysicsWorld.Step(args.FixedDeltaTime);
        }
    }
}

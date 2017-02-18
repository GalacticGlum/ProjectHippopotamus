using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Core.Entities;
using Microsoft.Xna.Framework;

namespace Hippopotamus.Engine.Physics
{
    public class Rigidbody : Component, IStartable
    {
        public Body Body { get; }

        public Rigidbody(Vector2 dimensions, BodyType type = BodyType.Dynamic)
        {
            Vector2 convertedDimensions = ConvertUnits.ToSimUnits(dimensions);
            Body = BodyFactory.CreateRectangle(EntitySystemManager.Get<PhysicsSystem>().PhysicsWorld, convertedDimensions.X, convertedDimensions.Y, 1f);
            Body.BodyType = type;
            Body.UserData = Entity;
        }

        public void Start()
        {
            Body.Position = ConvertUnits.ToSimUnits(Transform.Position);
        }
    }
}

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Hippopotamus.Engine.Core;
using Microsoft.Xna.Framework;

namespace Hippopotamus.Engine.Physics
{
    public class Rigidbody : Component
    {
        public Body Body { get; }

        public Rigidbody(Vector2 dimensions, BodyType type = BodyType.Dynamic)
        {
            Vector2 convertedDimensions = ConvertUnits.ToSimUnits(dimensions);
            Body = BodyFactory.CreateRectangle(EntitySystemManager.Get<PhysicsSystem>().PhysicsWorld, convertedDimensions.X, convertedDimensions.Y, 1f);
            Body.BodyType = type;
            Body.UserData = Entity;
        }

        public override void Start()
        {
            Body.Position = ConvertUnits.ToSimUnits(Transform.Position);
        }

        public override void Reset()
        {
            throw new System.NotImplementedException();
        }
    }
}

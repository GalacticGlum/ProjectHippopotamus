using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Rendering;

using Hippopotamus.Engine.Physics;

namespace Hippopotamus
{
    public class Game : GameInstance
    {
        public override void Initialize()
        {
            GameEngine.EntityPool.Create("camera").AddComponent(new Camera(GameEngine.GraphicsDevice.Viewport));
            EntitySystemManager.Register<WorldSystem>();
        }
    }
}

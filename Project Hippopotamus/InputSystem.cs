using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Core.Entities;
using Hippopotamus.Engine.Rendering;
using Microsoft.Xna.Framework.Input;

namespace Hippopotamus
{
    [StartupEntitySystem]
    public class InputSystem : EntitySystem, IUpdatable
    {
        public void Update(object sender, GameLoopUpdateEventArgs args)
        {
            const float speed = 1000.0f;
           
            if (Input.GetKey(Keys.A)) Camera.Main.Transform.Translate(-speed * args.DeltaTime, 0); 
            if (Input.GetKey(Keys.D)) Camera.Main.Transform.Translate(speed * args.DeltaTime, 0); 
            if (Input.GetKey(Keys.W)) Camera.Main.Transform.Translate(0, -speed * args.DeltaTime);
            if (Input.GetKey(Keys.S)) Camera.Main.Transform.Translate(0, speed * args.DeltaTime); 
        }
    }
}

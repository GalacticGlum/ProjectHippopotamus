using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Rendering;
using Microsoft.Xna.Framework.Input;

namespace Hippopotamus
{
    [StartupEntitySystem]
    public class InputSystem : EntitySystem, IUpdatable
    {
        public void Update(object sender, GameLoopUpdateEventArgs args)
        {
            if (Input.GetKey(Keys.A)) { Camera.Main.Transform.Translate(-200.0f * args.DeltaTime, 0); }
            if (Input.GetKey(Keys.D)) { Camera.Main.Transform.Translate(200.0f * args.DeltaTime, 0); }
            if (Input.GetKey(Keys.W)) { Camera.Main.Transform.Translate(0, -200.0f * args.DeltaTime); }
            if (Input.GetKey(Keys.S)) { Camera.Main.Transform.Translate(0, 200.0f * args.DeltaTime); }
        }
    }
}

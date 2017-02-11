using Hippopotamus.Components;
using Hippopotamus.Engine.Core;
using Hippopoutamus.Engine.Core;
using Microsoft.Xna.Framework.Input;

namespace Hippopotamus.Systems
{
    public class MovementSystem : System<Constraint<Player>>
    {
        public override void Update(GameObject gameObject)
        {
            Player player = gameObject.GetComponent<Player>();
            if (Input.GetKey(Keys.A))
            {
                gameObject.Transform.Translate(-player.Speed, 0);
            }

            if (Input.GetKey(Keys.D))
            {
                gameObject.Transform.Translate(player.Speed, 0);
            }

            if (Input.GetKey(Keys.W))
            {
                gameObject.Transform.Translate(0, -player.Speed);
            }

            if (Input.GetKey(Keys.S))
            {
                gameObject.Transform.Translate(0, player.Speed);
            }
        }
    }
}

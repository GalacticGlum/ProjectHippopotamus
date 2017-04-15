using System;
using Hippopotamus.Engine.Core;
using Hippopotamus.Engine.Core.Entities;
using Hippopotamus.Engine.Rendering;
using Hippopotamus.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Hippopotamus
{
    [StartupEntitySystem]
    public class InputSystem : EntitySystem
    {
        private float speed = 100;
        private Vector2 targetPosition = Vector2.Zero;

        public override void Start()
        {
            InputManager.RegisterInputMapping("InputSystem: Key A", MappedInputModifier.None, Keys.A, Keys.Left);
            InputManager.RegisterKeyInputAction("InputSystem: Key A", MappedInputType.Held, (sender, args) => targetPosition += new Vector2(-speed * args.DeltaTime, 0));

            InputManager.RegisterInputMapping("InputSystem: Key D", MappedInputModifier.None, Keys.D, Keys.Right);
            InputManager.RegisterKeyInputAction("InputSystem: Key D", MappedInputType.Held, (sender, args) => targetPosition += new Vector2(speed * args.DeltaTime, 0));

            InputManager.RegisterInputMapping("InputSystem: Key W", MappedInputModifier.None, Keys.W, Keys.Up);
            InputManager.RegisterKeyInputAction("InputSystem: Key W", MappedInputType.Held, (sender, args) => targetPosition += new Vector2(0, -speed * args.DeltaTime));

            InputManager.RegisterInputMapping("InputSystem: Key S", MappedInputModifier.None, Keys.S, Keys.Down);
            InputManager.RegisterKeyInputAction("InputSystem: Key S", MappedInputType.Held, (sender, args) => targetPosition += new Vector2(0, speed * args.DeltaTime));

            InputManager.RegisterInputMapping("InputSystem: Mouse Left", MappedInputModifier.None, MouseButton.Left);
            InputManager.RegisterMouseInputAction("InputSystem: Mouse Left", MappedInputType.Down, (sender, args) => PlaceTile(TileType.Grass));

            InputManager.RegisterInputMapping("InputSystem: Mouse Right", MappedInputModifier.None, MouseButton.Right);
            InputManager.RegisterMouseInputAction("InputSystem: Mouse Right", MappedInputType.Down, (sender, args) => PlaceTile(TileType.Empty));

            InputManager.RegisterInputMapping("Simulate cave", MappedInputModifier.Control, MouseButton.Right);
            InputManager.RegisterMouseInputAction("Simulate cave", MappedInputType.Down, (sender, args) => MakeCave());

            InputManager.RegisterInputMapping("SpeedUp", MappedInputModifier.None, Keys.Add);
            InputManager.RegisterKeyInputAction("SpeedUp", MappedInputType.Down, (sender, args) => speed += 10);

            InputManager.RegisterInputMapping("SpeedDown", MappedInputModifier.None, Keys.Subtract);
            InputManager.RegisterKeyInputAction("SpeedDown", MappedInputType.Down, (sender, args) => speed -= 10);

            InputManager.RegisterInputMapping("Position logger", MappedInputModifier.Control, MouseButton.Left);
            InputManager.RegisterMouseInputAction("Position logger", MappedInputType.Down, (sender, args) => Logger.Log("InputSystem", Camera.Main.Transform.Position.ToString()));

            InputManager.RegisterInputMapping("Reset world", MappedInputModifier.Control, Keys.R);
            InputManager.RegisterKeyInputAction("Reset world", MappedInputType.Down, (sender, args) => World.World.Current.Generate());
        }

        public override void Update(GameLoopEventArgs args)
        {
            if (targetPosition != Vector2.Zero)
            {
                Camera.Main.Transform.Position += new Vector2((float)Math.Round(targetPosition.X), (float)Math.Round(targetPosition.Y));
            }

            targetPosition = Vector2.Zero;
        }

        private static void PlaceTile(TileType type)
        {
            Vector2 mousePosition = Camera.Main.ScreenToWorldPoint(Input.MousePosition);

            int x = (int) Math.Floor(mousePosition.X / Tile.Size + 0.5f);
            int y = (int) Math.Floor(mousePosition.Y / Tile.Size + 0.5f);

            Tile tile = World.World.Current.GetTileAt(x, y);

            if (tile == null) return;
            tile.Type = type;
        }

        private static void MakeCave()
        {
            Vector2 mousePosition = Camera.Main.ScreenToWorldPoint(Input.MousePosition);

            int x = (int)Math.Floor(mousePosition.X / Tile.Size + 0.5f);
            int y = (int)Math.Floor(mousePosition.Y / Tile.Size + 0.5f);

            Tile tile = World.World.Current.GetTileAt(x, y);

            if (tile == null) return;
            TerrainCaveGenerator.Generate(tile);
        }
    }
}

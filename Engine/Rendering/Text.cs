using Hippopotamus.Engine.Core;
using Microsoft.Xna.Framework;

namespace Hippopotamus.Engine.Rendering
{
    public class Text : Component
    {
        public string Font { get; set; }
        public string Message { get; set; }

        public Color Colour { get; set; }

        public Text(string message, string font = "Arial")
        {
            Font = font;
            Message = message;
            Colour = Color.White;
        }
    }
}

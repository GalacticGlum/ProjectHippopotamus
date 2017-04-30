using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.UI.Framework.Rendering
{
    public class TextRenderer
    {
        public SpriteFont SpriteFont { get; set; }
        public Color Colour { get; set; }

        public TextRenderer(SpriteFont spriteFont, Color colour)
        {
            SpriteFont = spriteFont;
            Colour = colour;
        }

        public void Draw(SpriteBatch spriteBatch, string message, Rectangle destinationArea,
            Anchor anchor = Anchor.Centre)
        {
            Vector2 position = new Vector2(destinationArea.X, destinationArea.Y);
            Vector2 size = SpriteFont.MeasureString(message);

            switch (anchor)
            {
                case Anchor.TopLeft:
                    break;
                case Anchor.TopCentre:
                   position.X += (destinationArea.Width - size.X) / 2f;
                    break;
                case Anchor.TopRight:
                    position.X += destinationArea.Width - size.X;
                    break;
                case Anchor.CentreLeft:
                    position.Y += (destinationArea.Height - size.Y) / 2f;
                    break;
                case Anchor.Centre:
                    position.X += (destinationArea.Width - size.X) / 2f;
                    position.Y += (destinationArea.Height - size.Y) / 2f;
                    break;
                case Anchor.CentreRight:
                    position.X += destinationArea.Width - size.X;
                    position.Y += (destinationArea.Height - size.Y) / 2f;
                    break;
                case Anchor.BottomLeft:
                    position.Y += destinationArea.Height - size.Y;
                    break;
                case Anchor.BottomCentre:
                    position.X += (destinationArea.Width - size.X) / 2f;
                    position.Y += destinationArea.Height - size.Y;
                    break;
                case Anchor.BottomRight:
                    position.X += destinationArea.Width - size.X;
                    position.Y += destinationArea.Height - size.Y;
                    break;
            }

            spriteBatch.DrawString(SpriteFont, message, position, Colour);
        }
    }
}

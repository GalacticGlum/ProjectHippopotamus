using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hippopotamus.Engine.UI
{
    public class Widget
    {
        public Widget Parent { get; set; }
        public List<Widget> Children { get; }

        public Vector2 Offset { get; set; }
        public Vector2 Size { get; set; }
        public Anchor Anchor { get; set; }

        public virtual Vector2 DefaultSize => Vector2.Zero;

        protected Rectangle Destination { get; set; }
        private bool isDirty = true;

        public Widget(Vector2? size = null, Anchor anchor = Anchor.Centre, Vector2? offset =  null)
        {
            Size = size ?? DefaultSize;
            Offset = offset ?? Vector2.Zero;
            Anchor = anchor;

            Children = new List<Widget>();
            Destination = new Rectangle(0, 0, (int)Size.X, (int)Size.Y);
        }

        private void MakeDirty()
        {
            isDirty = true;
        }

        public virtual void Update()
        {
             CalculateDestination();
        }

        public virtual void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, Destination, Color.White);
            spriteBatch.End();
        }

        protected virtual void CalculateDestination()
        {
            if (!isDirty || Parent == null) return;
            Rectangle result = new Rectangle
            {
                Width = Size.X == 0f ? Parent.Destination.Width : (Size.X > 0f && Size.X < 1f ? (int) (Parent.Destination.Width * Size.X) : (int) Size.X),
                Height = Size.Y == 0f ? Parent.Destination.Height : (Size.Y > 0f && Size.Y < 1f ? (int) (Parent.Destination.Height * Size.Y) : (int) Size.Y)
            };

            switch (Anchor)
            {
                case Anchor.Centre:
                    result.X = (Parent.Destination.X + Parent.Destination.Width) / 2 - Destination.Width / 2 + (int)Offset.X;
                    result.Y = (Parent.Destination.Y + Parent.Destination.Height) / 2 - Destination.Height / 2 + (int) Offset.Y;  
                    break;
                case Anchor.CentreLeft:
                    result.X = Parent.Destination.X + (int)Offset.X;
                    result.Y = (Parent.Destination.Y + Parent.Destination.Height) / 2 - Destination.Height / 2 + (int)Offset.Y;
                    break;
                case Anchor.CentreRight:
                    result.X = Parent.Destination.X + Parent.Destination.Width - Destination.Width - (int)Offset.X;
                    result.Y = (Parent.Destination.Y + Parent.Destination.Height) / 2 - Destination.Height / 2 + (int)Offset.Y;
                    break;
                case Anchor.TopLeft:
                    result.X = Parent.Destination.X + (int) Offset.X;
                    result.Y = Parent.Destination.Y + (int) Offset.Y;
                    break;
                case Anchor.TopRight:
                    result.X = Parent.Destination.X + Parent.Destination.Width - Destination.Width - (int) Offset.X;
                    result.Y = Parent.Destination.Y + (int) Offset.Y;
                    break;
                case Anchor.TopCentre:
                    result.X = (Parent.Destination.X + Parent.Destination.Width) / 2 - Destination.Width / 2 + (int)Offset.X;
                    result.Y = Parent.Destination.Y + (int)Offset.Y;
                    break;
                case Anchor.BottomLeft:
                    result.X = Parent.Destination.X + (int)Offset.X;
                    result.Y = Parent.Destination.Y + Parent.Destination.Height - Destination.Height - (int) Offset.Y;
                    break;
                case Anchor.BottomRight:
                    result.X = Parent.Destination.X + Parent.Destination.Width - Destination.Width - (int) Offset.X;
                    result.Y = Parent.Destination.Y + Parent.Destination.Height - Destination.Height - (int)Offset.Y;
                    break;
                case Anchor.BottomCentre:
                    result.X = (Parent.Destination.X + Parent.Destination.Width) / 2 - Destination.Width / 2 + (int)Offset.X;
                    result.Y = Parent.Destination.Y + Parent.Destination.Height - Destination.Height - (int)Offset.Y;
                    break;
            }

            Destination = result;
        }
    }
}

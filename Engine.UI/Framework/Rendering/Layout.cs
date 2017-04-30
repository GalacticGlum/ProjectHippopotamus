using Microsoft.Xna.Framework;

namespace Engine.UI.Framework.Rendering
{
    public class Layout
    {
        public Rectangle RenderArea { get; protected set; }
        public Rectangle ChildArea { get; protected set; }
        public Rectangle ScissorArea { get; protected set; }

        public Point ChildOffset { get; set; }
    }
}

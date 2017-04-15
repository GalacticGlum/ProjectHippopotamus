using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hippopotamus.Engine.UI
{
    public class Widget
    {
        private List<Widget> children = new List<Widget>();
        protected Widget parent = null;
        protected Widget indexInParent;
        protected bool isInteractable = false;

        public string Name { get; set; } = string.Empty;
        public bool PromiscuousMode { get; set; }= false;
        protected bool AlwaysTriggerEvents = false;
        protected bool InheritParentState = false;

        private Widget background = null;
        private bool isFirstUpdateDirty = true;
        private bool isDestRectDirty = true;

        private uint destinationRectVersion = 0;
        private uint parentDestinationRectVersion = 0;

        public object UserData { get; set; }
        public bool UseActualSize { get; set; } = true;
        protected Vector2 size;
        protected Vector2 offset;
        protected Anchor anchor;


    }
}

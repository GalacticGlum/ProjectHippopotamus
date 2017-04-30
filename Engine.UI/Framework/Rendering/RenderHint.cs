using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Engine.UI.Framework.Rendering
{
    public abstract class RenderHint
    {
        public bool IsLoaded { get; set; }

        protected internal RenderManager RenderManager { get; set; }

        public abstract Rectangle Area{ get; set; }
        public virtual Rectangle SafeArea => Area;
        public virtual Rectangle ClippingArea => RenderManager.GraphicsDevice.Viewport.Bounds;

        protected string DefaultSkin => RenderManager.DefaultSkin;
        protected string DefaultText => RenderManager.DefaultTextRenderer;

        private string skin;
        protected internal string Skin
        {
            get { return skin; }
            set
            {
                if (value == null) return;

                skin = value;
                if (RenderManager != null) Load();
            }
        }

        private string text;
        protected internal string Text
        {
            get{ return text; }
            set
            {
                if (value == null) return;

                text = value;
                if (RenderManager != null) Load();
            }
        }

        public abstract void CalculateSize(int width, int height);
        public abstract void Draw();
        public abstract void DrawWithoutClipping();
        protected abstract void LoadRenderers();

        protected RenderHint()
        {
            IsLoaded = false;
        }

        protected T LoadRenderer<T>(string skin, string widget) where T : Renderer
        {
            return (T) RenderManager.Skins[skin].WidgetRenderers[widget];
        }

        public virtual void Load()
        {
            if (Skin == null) Skin = DefaultSkin;
            if (Text == null) Text = DefaultText;

            LoadRenderers();
            IsLoaded = true;
        }
    }
}

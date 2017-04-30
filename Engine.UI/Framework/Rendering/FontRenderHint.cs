using Microsoft.Xna.Framework.Graphics;

namespace Engine.UI.Framework.Rendering
{
    public abstract class FontRenderHint : RenderHint
    {
        public SpriteFont SpriteFont => TextRenderer.SpriteFont;
        protected TextRenderer TextRenderer { get; set; }

        public override void Load()
        {
            if (Skin == null) Skin = DefaultSkin;
            if (TextRenderer == null) Text = DefaultText;

            TextRenderer = RenderManager.TextRenderers[Text];
            LoadRenderers();
            IsLoaded = true;
        }
    }
}

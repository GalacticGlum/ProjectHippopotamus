using System.Collections.Generic;
using Hippopotamus.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.UI.Framework.Rendering
{
    public class RenderManager
    {
        public GraphicsDevice GraphicsDevice => GameEngine.Context.GraphicsDevice;
        public SpriteBatch SpriteBatch { get; private set; }

        public string DefaultSkin { get; set; }
        public string DefaultTextRenderer { get; set; }

        public Dictionary<string, Skin> Skins { get; private set; }
        public Dictionary<string, TextRenderer> TextRenderers { get; private set; }

        private Texture2D highlightTexture;
        public Color HighlightingColour
        {
            get
            {
                Color[] data = new Color[1];
                highlightTexture.GetData(data);
                return data[0];
            }
            set
            {
                highlightTexture.SetData(new[] { value });   
            }
        }

        private DepthStencilState applicationStencilState;
        private DepthStencilState sampleStencilState;
        private readonly RasterizerState rasterizerState;

        internal RenderManager()
        {
            Skins = new Dictionary<string, Skin>();
            TextRenderers = new Dictionary<string, TextRenderer>();

            applicationStencilState = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.Always,
                StencilPass = StencilOperation.Replace,
                ReferenceStencil = 1,
                DepthBufferEnable = false
            };

            sampleStencilState = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.Equal,
                StencilPass = StencilOperation.Keep,
                ReferenceStencil = 1,
                DepthBufferEnable = false
            };

            rasterizerState = new RasterizerState
            {
                ScissorTestEnable = true
            };

            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        internal void Draw(RootNode<Widget> rootWidget)
        {
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, rasterizerState);

            rootWidget.ExecuteOnChildren(node =>
            {
                if (!node.UserData.Visible) return;
                GraphicsDevice.ScissorRectangle = node.UserData.ScissorArea;
                node.UserData.Draw();
            });

            rootWidget.ExecuteOnChildren(node =>
            {
                if (!node.UserData.Visible) return;
                GraphicsDevice.ScissorRectangle = node.UserData.ClipArea;
                node.UserData.DrawWithoutClipping();
            });

            SpriteBatch.End();

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }

        internal void AddSkin(string name, Skin skin)
        {
            Skins.Add(name, skin);
        }

        internal void AddTextRenderer(string name, TextRenderer renderer)
        {
            TextRenderers.Add(name, renderer);
        }
    }
}

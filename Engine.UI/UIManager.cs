using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.UI.Framework;
using Engine.UI.Framework.Input;
using Engine.UI.Framework.Rendering;

namespace Engine.UI
{
    public class UIManager
    {
        internal InputManager InputManager { get; private set; }
        internal RenderManager RenderManager { get; private set; }

        private RootNode<Widget> rootWidget;

        public UIManager(Skin defaultSkin, TextRenderer defaulTextRenderer,
            IEnumerable<Tuple<string, Skin>> skins = null, IEnumerable<Tuple<string, TextRenderer>> textRenderers = null)
        {
            InitializeTree();
        }

        private void InitializeTree()
        {
            rootWidget = new RootNode<Widget>();
            rootWidget.AttachedToRootEventHandler += node =>
            {
                node.ExecuteOnChildren(pNode => pNode.UserData.Prepare(this));
                node.ExecuteOnChildren(child =>
                {
                    if (child.Parent != null && child.Parent.Root != child.Parent)
                    {
                        child.Parent.UserData.CreateLayout();
                    }
                });
            };

            rootWidget.ChildrenChangedEventHandler += node => node.Execute(n => n.UserData.CreateLayout());
        }
    }
}

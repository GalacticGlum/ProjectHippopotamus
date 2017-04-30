using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.UI.Framework.Rendering
{
    public class Skin
    {
        internal Texture2D TextureMap { get; }
        internal Dictionary<string, Renderer> WidgetRenderers { get; set; }

        public Skin(Texture2D textureMap, string source)
        {
            TextureMap = textureMap;
            WidgetRenderers = new Dictionary<string, Renderer>();

            Parse(source);
        }

        private void Parse(string source)
        {
            source = source.Replace("\r", string.Empty);
            foreach (string line in source.Split('\n'))
            {
                if (string.IsNullOrWhiteSpace(line)) return;

                string[] equals = line.Split('=');
                string[] parameters = equals[1].Split(',');
                Type renderType = typeof(Renderer);
                List<Type> allRendererTypes = new List<Type>();
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    allRendererTypes.AddRange(assembly.GetTypes().Where(type => type != renderType && renderType.IsAssignableFrom(type)));
                }

                foreach (Type type in allRendererTypes)
                {
                    if(type.Name != parameters[1].Trim()) continue;
                    string[] rectangleInformation = parameters[0].Trim().Split(' ');
                    Rectangle rectangle = new Rectangle(
                        int.Parse(rectangleInformation[0]), 
                        int.Parse(rectangleInformation[1]), 
                        int.Parse(rectangleInformation[2]), 
                        int.Parse(rectangleInformation[3]));

                    List<Type> parameterTypes = new List<Type>()
                    {
                        typeof(Texture2D),
                        typeof(Rectangle)
                    };

                    for (int x = 2; x < parameters.Length; x++)
                    {
                        parameterTypes.Add(typeof(int));
                    }

                    List<object> paramObjects = new List<object>
                    {
                        TextureMap,
                        rectangle
                    };

                    for (int x = 2; x < parameters.Length; x++)
                    {
                        paramObjects.Add(int.Parse(parameters[x]));
                    }

                    ConstructorInfo rendererCtor = type.GetConstructor(parameterTypes.ToArray());
                    if (rendererCtor != null)
                    {
                        WidgetRenderers.Add(equals[0].Trim(), (Renderer)rendererCtor.Invoke(paramObjects.ToArray()));
                    }
                }
            }
        }
    }
}

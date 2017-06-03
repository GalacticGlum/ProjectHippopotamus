using System.IO;
using System.Xml;
using UnityEngine;

namespace Hippopotamus.World
{
    public class TerrainProcessor
    {
        private string rootProcess;

        public TerrainProcessor(string filePath)
        {
            filePath = Path.Combine(Application.streamingAssetsPath, filePath);
            XmlUtilities.Read("TerrainProcessor", "Parameters", filePath, null, LoadProcess);
        }

        private void LoadProcess(XmlReader reader)
        {
            Lua.Parse(reader.GetAttribute("FilePath"));
            rootProcess = reader.GetAttribute("FunctionName");
        }

        public void Execute(WorldData worldData)
        {
            Lua.Call(rootProcess, worldData.Width, worldData.Height);
        }
    }
}

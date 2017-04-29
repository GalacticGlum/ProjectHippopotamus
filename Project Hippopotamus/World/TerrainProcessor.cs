using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Hippopotamus.Engine.Bridge;
using Hippopotamus.Engine.Utilities;
using MoonSharp.Interpreter;

namespace Hippopotamus.World
{
    public class TerrainProcessor
    {
        private readonly List<Tuple<Closure, int>> processes;

        public TerrainProcessor(string filePath)
        {
            processes = new List<Tuple<Closure, int>>();

            filePath = Path.Combine("Content", filePath);
            XmlUtilities.Read("TerrainProcessor", "Process", filePath, LoadProcess);

            processes = processes.OrderBy(pair => pair.Item2).ToList();
        }

        private void LoadProcess(XmlReader reader)
        {
            Lua.Parse(reader.GetAttribute("FilePath"));
            Closure function = Lua.GetFunction(reader.GetAttribute("FunctionName"));
            if (function == null) return;

            int layer = 0;
            if (reader.GetAttribute("ExecutionLayer") != null)
            {
                int.TryParse(reader.GetAttribute("ExecutionLayer"), out layer);
            }

            processes.Add(new Tuple<Closure, int>(function, layer));
        }

        public void Execute(WorldData worldData)
        {
            foreach (Tuple<Closure, int> process in processes)
            {
                Lua.Call(process.Item1, worldData);
            }
        }
    }
}

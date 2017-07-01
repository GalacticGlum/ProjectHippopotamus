using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class WorldEventController
{
    private readonly Dictionary<string, WorldEvent> worldEvents;

    public WorldEventController()
    {
        worldEvents = new Dictionary<string, WorldEvent>();
        Load();
    }

    public void Update()
    {
        foreach (WorldEvent worldEvent in worldEvents.Values)
        {
            worldEvent.Update();
        }
    }

    private void Load()
    {
        string filePath = Path.Combine(Path.Combine(Application.streamingAssetsPath, "Data"), "WorldEvents.xml");
        XmlTextReader reader = new XmlTextReader(new StringReader(File.ReadAllText(filePath)));
        if (reader.ReadToDescendant("WorldEvents") && reader.ReadToDescendant("WorldEvent"))
        {
            do
            {
                string worldEventName = reader.GetAttribute("Name");
                bool repeat = false;
                int maxRepeats = 0;

                List<string> preconditionNames = new List<string>();
                List<string> onExecuteNames = new List<string>();

                while (reader.Read())
                {
                    switch (reader.Name)
                    {
                        case "Repeats":
                            string maxRepeatesAttribute = reader.GetAttribute("MaxRepeats");
                            if (maxRepeatesAttribute == "Infinity")
                            {
                                maxRepeats = -1;
                            }
                            else
                            {
                                maxRepeats = int.Parse(maxRepeatesAttribute);
                            }

                            repeat = true;
                            break;
                        case "Precondition":
                            string luaFilePath = reader.GetAttribute("FilePath");
                            Lua.Parse(luaFilePath);

                            string preconditionName = reader.GetAttribute("FunctionName");
                            preconditionNames.Add(preconditionName);
                            break;
                        case "OnExecute":
                            luaFilePath = reader.GetAttribute("FilePath");
                            Lua.Parse(luaFilePath);

                            string onExecuteName = reader.GetAttribute("FunctionName");
                            onExecuteNames.Add(onExecuteName);
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(worldEventName))
                {
                    Create(worldEventName, repeat, maxRepeats, preconditionNames.ToArray(), onExecuteNames.ToArray());
                }
            }
            while (reader.ReadToNextSibling("WorldEvent"));
        }
        else
        {
            Logger.Log("Engine", string.Format("WorldEventController::Load: Could not load file \"{0}\"", filePath));
        }
    }

    private void Create(string eventName, bool repeat, int maxRepeats, string[] preconditionNames,
        string[] onExecuteNames)
    {
        WorldEvent worldEvent = new WorldEvent(eventName, repeat, maxRepeats);
        worldEvent.RegisterPreconditions(preconditionNames);
        worldEvent.RegisterAction(onExecuteNames);

        worldEvents[eventName] = worldEvent;
    }
}
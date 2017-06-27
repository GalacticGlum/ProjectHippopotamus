using System;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldGraphicController
{
    private Material skybox;

    public WorldGraphicController()
    {
        WorldController.Instance.Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, WorldControllerEventArgs args)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(Application.dataPath, "Game/Resources/Skyboxes"));
        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }

        FileInfo[] files = directoryInfo.GetFiles("*.mat", SearchOption.AllDirectories);
        if (files.Length > 0)
        {
            FileInfo file = files[(int) Random.value * files.Length];
            if (!string.IsNullOrEmpty(file.DirectoryName))
            {
                string resourcePath = Path.Combine(file.DirectoryName.Substring(file.DirectoryName.IndexOf("Skyboxes", StringComparison.Ordinal)), file.Name);
                if (resourcePath.Contains("."))
                {
                    resourcePath = resourcePath.Remove(resourcePath.LastIndexOf(".", StringComparison.Ordinal));
                }

                skybox = Resources.Load<Material>(resourcePath);
            }

            RenderSettings.skybox = skybox;
        }
        else
        {
            Logger.Log("WorldGraphicController", "No skyboxes found, defaulting to black.", LoggerVerbosity.Warning);
        }
    }
}

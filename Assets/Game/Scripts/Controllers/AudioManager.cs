using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public static class AudioManager
{
    private static Dictionary<string, AudioClip> audioClips;

    public static void Initialize()
    {
        LoadAudioClips();
    }

    private static void LoadAudioClips()
    {
        audioClips = new Dictionary<string, AudioClip>();
        DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(Application.dataPath, "Game/Resources/Audio"));
        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }

        FileInfo[] files = directoryInfo.GetFiles("*.wav", SearchOption.AllDirectories);
        if (files.Length <= 0) return;

        FileInfo file = files[(int)Random.value * files.Length];
        if (string.IsNullOrEmpty(file.DirectoryName)) return;

        string resourcePath = Path.Combine(file.DirectoryName.Substring(file.DirectoryName.IndexOf("Audio", StringComparison.Ordinal)), file.Name);
        if (resourcePath.Contains("."))
        {
            resourcePath = resourcePath.Remove(resourcePath.LastIndexOf(".", StringComparison.Ordinal));
        }

        AudioClip audioClip = Resources.Load<AudioClip>(resourcePath);
        audioClips.Add(resourcePath.Remove(0, "Audio/".Length), audioClip);
    }

    public static AudioClip Get(string categoryName, string audioClipName)
    {
        if (!string.IsNullOrEmpty(categoryName))
        {
            audioClipName = categoryName + "/" + audioClipName;
        }

        if (audioClips.ContainsKey(audioClipName)) return audioClips[audioClipName];

        Logger.Log("AudioManager", string.Format("Could not load AudioClip: \"{0}\"", audioClipName), LoggerVerbosity.Warning);
        return null;
    }

    public static AudioClip Get(string audioClipName)
    {
        return Get(string.Empty, audioClipName);
    }
}


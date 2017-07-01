using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEngine;

public static class WordDictionary
{
    private static readonly Dictionary<string, string[]> wordMap;

    static WordDictionary()
    {
        wordMap = new Dictionary<string, string[]>(); 
        FileInfo[] files = new DirectoryInfo(Path.Combine(Application.streamingAssetsPath, "Dictionary")).GetFiles("*.json", SearchOption.AllDirectories);
        foreach (FileInfo fileInfo in files)
        {
            if (string.IsNullOrEmpty(fileInfo.DirectoryName) || fileInfo.Directory == null) continue;

            JsonData jsonData = JsonMapper.ToObject(File.ReadAllText(fileInfo.FullName));

            string name = jsonData["name"].ToString();
            string[] values = new string[jsonData["values"].Count];
            for (int i = 0; i < jsonData["values"].Count; i++)
            {
                values[i] = jsonData["values"][i].ToString();
            }

            wordMap.Add(name, values);
        }
    }

    public static string Get(string name)
    {
        if (wordMap.ContainsKey(name)) return wordMap[name][Random.Range(0, wordMap[name].Length)];

        Logger.LogFormat("Engine", "WordDictionary::Get: Word Collection of name \"{0}\" does not exist!", LoggerVerbosity.Warning, name);
        return string.Empty;
    }
}
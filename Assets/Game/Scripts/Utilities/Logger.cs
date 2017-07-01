using System;
using System.Collections.Generic;
using UnityEngine;

[LuaExposeType]
public static class Logger
{
    public const string AllCategoryVerbosities = "__ALL_CATEGORY_VERBOSITIES__";

    public static LoggerTimeStampMode TimeStampMode { get; set; }
    public static LoggerVerbosity Verbosity { get; set; }

    /// <summary>
    /// The category verbosity filter. If set to null, then the filter will allow all categories.
    /// </summary>
    public static Dictionary<string, LoggerVerbosity> CategoryVerbosities { get; set; }

    static Logger()
    {
        CategoryVerbosities = new Dictionary<string, LoggerVerbosity>
        {
            // Second parameter doesn't matter, if the key AllCategoryVerbosities is in the dictionary then it just logs any category.
            { AllCategoryVerbosities, LoggerVerbosity.Info}
        };
    }

    public static void Log(string category, string message, LoggerVerbosity messageVerbosity = LoggerVerbosity.Info)
    {

        if (Verbosity > messageVerbosity) return;
        if (CategoryVerbosities != null)
        {
            if (!CategoryVerbosities.ContainsKey(category) && !CategoryVerbosities.ContainsKey(AllCategoryVerbosities)) return;
            if (CategoryVerbosities.ContainsKey(category))
            {
                if (CategoryVerbosities[category] > messageVerbosity) return;
            }
        }

        string output = string.Concat(GetMessageHeader(category), message);
        switch (messageVerbosity)
        {
            case LoggerVerbosity.Info:
                Debug.Log(output);
                break;
            case LoggerVerbosity.Warning:
                Debug.LogWarning(output);
                break;
            case LoggerVerbosity.Error:
                Debug.LogError(output);
                break;
            default:
                throw new ArgumentOutOfRangeException("messageVerbosity", messageVerbosity, null);
        }
    }

    public static void Log(string category, int message, LoggerVerbosity messageVerbosity = LoggerVerbosity.Info)
    {
        Log(category, message.ToString(), messageVerbosity);
    }

    public static void Log(string category, float message, LoggerVerbosity messageVerbosity = LoggerVerbosity.Info)
    {
        Log(category, message.ToString(), messageVerbosity);
    }

    public static void Log(string message, LoggerVerbosity messageVerbosity = LoggerVerbosity.Info)
    {
        Log(string.Empty, message, messageVerbosity);
    }

    public static void Log(int message, LoggerVerbosity messageVerbosity = LoggerVerbosity.Info)
    {
        Log(string.Empty, message, messageVerbosity);
    }

    public static void Log(float message, LoggerVerbosity messageVerbosity = LoggerVerbosity.Info)
    {
        Log(string.Empty, message, messageVerbosity);
    }

    public static void LogFormat(string category, string message, LoggerVerbosity messageVerbosity = LoggerVerbosity.Info, params object[] args)
    {
        Log(category, string.Format(message, args));
    }

    public static void LogFormat(string message, LoggerVerbosity messageVerbosity = LoggerVerbosity.Info, params object[] args)
    {
        Log(string.Empty, string.Format(message, args), messageVerbosity);
    }

    private static string GetMessageHeader(string category)
    {
        string dateTimeStamp = string.Empty;
        switch (TimeStampMode)
        {
            case LoggerTimeStampMode.TimeStamp:
                dateTimeStamp = DateTime.Now.ToShortTimeString();
                break;
            case LoggerTimeStampMode.DateStamp:
                dateTimeStamp = DateTime.Now.ToShortDateString();
                break;
            case LoggerTimeStampMode.DateTimeStamp:
                dateTimeStamp = DateTime.Now.ToString();
                break;
        }

        string headerContents = !string.IsNullOrEmpty(dateTimeStamp) ? dateTimeStamp : string.Empty;
        string header = !string.IsNullOrEmpty(headerContents) ? string.Format("[{0}] ", headerContents) : string.Empty;
        string categoryHeader = !string.IsNullOrEmpty(category) ? string.Format("{0}: ", category) : string.Empty;
        return string.Concat(header, categoryHeader);
    }

    private static ConsoleColor GetConsoleColour(LoggerVerbosity verbosity)
    {
        switch (verbosity)
        {
            case LoggerVerbosity.Info:
                return ConsoleColor.White;
            case LoggerVerbosity.Warning:
                return ConsoleColor.Yellow;
            case LoggerVerbosity.Error:
                return ConsoleColor.Red;
        }

        return ConsoleColor.Gray;
    }

    private static string GetVerbosityName(LoggerVerbosity verbosity)
    {
        switch (verbosity)
        {
            case LoggerVerbosity.Info:
                return "Info";
            case LoggerVerbosity.Warning:
                return "Warning";
            case LoggerVerbosity.Error:
                return "Error";
        }

        return string.Empty;
    }
}

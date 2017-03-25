using System;

namespace Hippopotamus.Engine.Core
{
    public static class Logger
    {
        public static LoggerVerbosity Verbosity { get; set; } = LoggerVerbosity.All;

        public static LogTimeStampMode TimeStampMode { get; set; } = LogTimeStampMode.DateTimeStamp;
        public static void Log(string message, LogMessageVerbosity messageVerbosity = LogMessageVerbosity.Info, bool logUps = false, bool logFps = false)
        {
            lock (Console.Out)
            {
                if (Verbosity != LoggerVerbosity.All && (int) messageVerbosity != (int) Verbosity) return;

                ConsoleColor oldConsoleColor = Console.ForegroundColor;
                Console.ForegroundColor = (ConsoleColor) messageVerbosity;

                Console.WriteLine($"{GetMessageHeader(logUps, logFps)}{message}");
                Console.ForegroundColor = oldConsoleColor;
            }
        }

        private static string GetMessageHeader(bool logUps, bool logFps)
        {
            string dateTimeStamp = string.Empty;
            switch (TimeStampMode)
            {
                case LogTimeStampMode.TimeStamp:
                    dateTimeStamp =  $"{DateTime.Now.ToShortTimeString()}";
                    break;
                case LogTimeStampMode.DateStamp:
                    dateTimeStamp = $"{DateTime.Now.ToShortDateString()}";
                    break;
                case LogTimeStampMode.DateTimeStamp:
                    dateTimeStamp = $"{DateTime.Now}";
                    break;
            }

            string headerContents = !string.IsNullOrEmpty(dateTimeStamp) ? $"{dateTimeStamp}" : string.Empty;
            if (logUps)
            {
                if (string.IsNullOrEmpty(headerContents))
                {
                    headerContents += $"Update: {GameEngine.Context.TimeStats.UpdateCount}";
                }
                else
                {
                    headerContents += $" | Update: {GameEngine.Context.TimeStats.UpdateCount}";
                }
            }

            // ReSharper disable once InvertIf
            if (logFps)
            {
                if (string.IsNullOrEmpty(headerContents))
                {
                    headerContents += $"Frame: {GameEngine.Context.TimeStats.FrameCount}";
                }
                else
                {
                    headerContents += $" | Frame: {GameEngine.Context.TimeStats.FrameCount}";
                }
            }

            return !string.IsNullOrEmpty(headerContents) ? $"[{headerContents}] " : string.Empty;
        }
    }
}

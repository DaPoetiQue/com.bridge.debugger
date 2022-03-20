using Bridge.Core.Debug;

namespace Bridge.Core.UnityCustomEditor.Debugger
{
    /// <summary>
    /// This static class manages logging debug messages to the unity console.
    /// </summary>
    public static class DebugConsole
    {
        #region Debug

        /// <summary>
        /// logs messages to the console.
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="logChannel"></param>
        /// <param name="logclass"></param>
        /// <param name="logMessage"></param>
        public static void Log(LogLevel logLevel, object logclass, object logMessage)
        {
            string formattedMessage = $"<color=white>-->></color> {DebugFormat.BeginLogColor(logLevel)}{DebugFormat.logLevelTag(logLevel)}{DebugFormat.EndLogColor()} : <color=white>{logMessage}.</color> {DebugFormat.BeginLogColor(logLevel)} Invoked From {DebugFormat.EndLogColor()} : <color=grey>{logclass}</color>";

            if (logLevel == LogLevel.Success)
            {
                formattedMessage = $"<color=white>-->></color> {DebugFormat.BeginLogColor(logLevel)}Success{DebugFormat.EndLogColor()}{DebugFormat.BeginLogColor(logLevel)}{DebugFormat.logLevelTag(logLevel)}{DebugFormat.EndLogColor()} : <color=white>{logMessage}.</color> {DebugFormat.BeginLogColor(logLevel)} Invoked From {DebugFormat.EndLogColor()} : <color=grey>{logclass}</color>";
            }

            switch (logLevel)
            {
                case LogLevel.Debug:

                    UnityEngine.Debug.Log(formattedMessage);

                    break;

                case LogLevel.Error:

                    UnityEngine.Debug.LogError(formattedMessage);

                    break;

                case LogLevel.Success:

                    UnityEngine.Debug.Log(formattedMessage);

                    break;

                case LogLevel.Warning:

                    UnityEngine.Debug.LogWarning(formattedMessage);

                    break;
            }
        }

        /// <summary>
        /// logs messages to the console without the referenced class name.
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="logMessage"></param>
        public static void Log(LogLevel logLevel, object logMessage)
        {
            string formattedMessage = $"<color=white>-->></color> {DebugFormat.BeginLogColor(logLevel)}{DebugFormat.logLevelTag(logLevel)}{DebugFormat.EndLogColor()} : <color=white>{logMessage}.</color>";

            if (logLevel == LogLevel.Success)
            {
                formattedMessage = $"<color=white>-->></color> {DebugFormat.BeginLogColor(logLevel)}Success{DebugFormat.EndLogColor()}{DebugFormat.BeginLogColor(logLevel)}{DebugFormat.logLevelTag(logLevel)}{DebugFormat.EndLogColor()} : <color=white>{logMessage}.</color>";
            }

            switch (logLevel)
            {
                case LogLevel.Debug:

                    UnityEngine.Debug.Log(formattedMessage);

                    break;

                case LogLevel.Error:

                    UnityEngine.Debug.LogError(formattedMessage);

                    break;

                case LogLevel.Success:

                    UnityEngine.Debug.Log(formattedMessage);

                    break;

                case LogLevel.Warning:

                    UnityEngine.Debug.LogWarning(formattedMessage);

                    break;
            }
        }

        #endregion
    }
}

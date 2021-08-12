namespace Bridge.Core.Debug
{
    public abstract class MonoDebug : UnityEngine.MonoBehaviour, ILogger
    {
        #region Components

        public LogChannel enabledLogChannel;

        #endregion

        #region Debug
        /// <summary>
        /// logs messages to the console on scene objects.
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="logClass"></param>
        /// <param name="logMessage"></param>
        public void Log(LogLevel logLevel, object logClass, object logMessage)
         {
            if(enabledLogChannel == LogChannel.None) return;

            string formattedMessage = $"<color=white>-->></color> {DebugFormat.BeginLogColor(logLevel)}{DebugFormat.logLevelTag(logLevel)}{DebugFormat.EndLogColor()} : <color=white>{logMessage}.</color> {DebugFormat.BeginLogColor(logLevel)} Invoked From {DebugFormat.EndLogColor()} : <color=grey>{logClass}</color>";

            if(logLevel == LogLevel.Success)
            {
                formattedMessage = $"<color=white>-->></color> {DebugFormat.BeginLogColor(logLevel)}Success{DebugFormat.EndLogColor()}{DebugFormat.BeginLogColor(logLevel)}{DebugFormat.logLevelTag(logLevel)}{DebugFormat.EndLogColor()} : <color=white>{logMessage}.</color> {DebugFormat.BeginLogColor(logLevel)} Invoked From {DebugFormat.EndLogColor()} : <color=grey>{logClass}</color>";
            }

            if(enabledLogChannel != LogChannel.All)
            {
                if(enabledLogChannel.ToString() != logLevel.ToString()) return;
            }
            
            switch (logLevel)
             {
                 case LogLevel.Debug:

                    Debugger.Log(DebugData.LogType.LogInfo, logClass, formattedMessage);

                    UnityEngine.Debug.Log(formattedMessage);

                 break;

                case LogLevel.Error:

                    Debugger.Log(DebugData.LogType.LogError, logClass, formattedMessage);

                    UnityEngine.Debug.LogError(formattedMessage);

                 break;

                case LogLevel.Success:

                   UnityEngine.Debug.Log(formattedMessage);

                 break;

                case LogLevel.Warning:

                    Debugger.Log(DebugData.LogType.LogWarning, logClass, formattedMessage);

                    UnityEngine.Debug.LogWarning(formattedMessage);

                 break;
             }
         }

        #endregion
    }
}
namespace Bridge.Core.Debug
{
    public abstract class MonoDebug : UnityEngine.MonoBehaviour, ILogger
    {
        #region Components

        public LogData.LogChannel enabledLogChannel;

        #endregion

         #region Debug

        public void Log(LogData.LogLevel logLevel, object logclass, object logMessage)
         {
            if(enabledLogChannel == LogData.LogChannel.None) return;

            string formattedMessage = $"<color=white>-->></color> {BeginLogColor(logLevel)}{logLevelTag(logLevel)}{EndLogColor()} : <color=white>{logMessage}.</color> {BeginLogColor(logLevel)} Invoked From {EndLogColor()} : <color=grey>{logclass}</color>";

            if(logLevel == LogData.LogLevel.Success)
            {
                formattedMessage = $"<color=white>-->></color> {BeginLogColor(logLevel)}Success{EndLogColor()}{BeginLogColor(logLevel)}{logLevelTag(logLevel)}{EndLogColor()} : <color=white>{logMessage}.</color> {BeginLogColor(logLevel)} Invoked From {EndLogColor()} : <color=grey>{logclass}</color>";
            }

            if(enabledLogChannel != LogData.LogChannel.All)
            {
                if(enabledLogChannel.ToString() != logLevel.ToString()) return;
            }
            
            switch (logLevel)
             {
                 case LogData.LogLevel.Debug:

                    UnityEngine.Debug.Log(formattedMessage);

                 break;

                case LogData.LogLevel.Error:

                   UnityEngine.Debug.LogError(formattedMessage);

                 break;

                case LogData.LogLevel.Success:

                   UnityEngine.Debug.Log(formattedMessage);

                 break;

                case LogData.LogLevel.Warning:

                  UnityEngine.Debug.LogWarning(formattedMessage);

                 break;

                 
             }

         }

        #endregion

        #region Data

        private string BeginLogColor(LogData.LogLevel logLevel)
        {
             string logColor = "";

            switch (logLevel)
            {
                case LogData.LogLevel.Debug:

                logColor = "<color=cyan>";

                break;

                case LogData.LogLevel.Error:

                 logColor = "<color=red>";

                break;

                case LogData.LogLevel.Success:

                 logColor = "<color=green>";

                break;

                case LogData.LogLevel.Warning:

                 logColor = "<color=orange>";

                break;
            }

            return logColor;
        }

        private string EndLogColor()
        {
            return "</color>";
        }

        public string logLevelTag(LogData.LogLevel logLevel)
        {
             string logLevelTag = "";

            switch (logLevel)
            {
                case LogData.LogLevel.Debug:

                logLevelTag = "Info Logged";

                break;

                case LogData.LogLevel.Warning:

                 logLevelTag = "Warning Logged";

                break;

                case LogData.LogLevel.Error:

                 logLevelTag = "Error Logged";

                 break;
            }

            return logLevelTag;
        }

        #endregion
    }
}
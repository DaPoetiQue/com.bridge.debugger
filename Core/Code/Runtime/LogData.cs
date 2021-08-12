namespace Bridge.Core.Debug
{
    #region Options

    public enum LogLevel
    {
        Debug, Error, Success, Warning
    }

    public enum LogChannel
    {
        All, Debug, Error, Success, Warning, None
    }

    #endregion


    #region Data

    /// <summary>
    /// This class contains debugger text formating code.
    /// </summary>
    public static class DebugFormat
    {
        public static string BeginLogColor(LogLevel logLevel)
        {
            string logColor = "";

            switch (logLevel)
            {
                case LogLevel.Debug:

                    logColor = "<color=cyan>";

                    break;

                case LogLevel.Error:

                    logColor = "<color=red>";

                    break;

                case LogLevel.Success:

                    logColor = "<color=green>";

                    break;

                case LogLevel.Warning:

                    logColor = "<color=orange>";

                    break;
            }

            return logColor;
        }

        public static string EndLogColor()
        {
            return "</color>";
        }

        public static string logLevelTag(LogLevel logLevel)
        {
            string logLevelTag = "";

            switch (logLevel)
            {
                case LogLevel.Debug:

                    logLevelTag = "Info Logged";

                    break;

                case LogLevel.Warning:

                    logLevelTag = "Warning Logged";

                    break;

                case LogLevel.Error:

                    logLevelTag = "Error Logged";

                    break;
            }

            return logLevelTag;
        }
    }

    #endregion Data
}
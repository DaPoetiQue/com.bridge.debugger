namespace Bridge.Core.Debug
{
    public class LogData
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
    }
}
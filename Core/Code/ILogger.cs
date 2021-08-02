namespace Bridge.Core.Debug
{
    public interface ILogger
    {
        void Log(LogData.LogLevel logLevel, object logclass, object logMessage);
    }
}
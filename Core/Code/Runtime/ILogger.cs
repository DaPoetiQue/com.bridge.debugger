namespace Bridge.Core.Debug
{
    public interface ILogger
    {
        void Log(LogLevel logLevel, object logclass, object logMessage);
    }
}
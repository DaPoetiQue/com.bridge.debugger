using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bridge.Core.Debug
{
    public interface ILogger
    {
        void Log(LogData.LogLevel logLevel, object logclass, object logMessage);
    }
}
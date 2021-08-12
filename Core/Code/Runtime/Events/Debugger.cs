namespace Bridge.Core.Debug
{
    public class Debugger
    {
        #region Delegates

        public delegate void DebugEvent<T,C,M>(T logType, C logClassName, M logMessage);

        #endregion

        #region Events

        public static event DebugEvent<DebugData.LogType, object, object> AddListener;

        #endregion

        #region Invokes

        public static void Log(DebugData.LogType logType, object logClassName, object logMessage)
        {
            if(AddListener != null)
            {
                AddListener(logType, logClassName, logMessage);
            }
        }

        #endregion
    }
}

using System;
using UnityEngine;

namespace Bridge.Core.Debug
{
    public class DebugData    
    {
        #region Enum Data

        public enum LogType
        {
            LogInfo, LogWarning, LogError, LogAll
        }

        public enum ConsoleToggleButtonPosition
        {
            BottomLeft, BottonRight, TopLeft, TopRight
        }

        public enum WindowButtonType
        {
            // Button types.
            None, Close, Clear, LogInfo, LogWarning, LogError, LogAll
        }

        #endregion

        #region Structs

        [Serializable]
        public struct LogPanel
        {
            public GameObject content;
            public LogType logType;
        }

        public struct LogItem
        {
            public string logName, logMessage;
            public GameObject logPanel;
            public Color panelColor;
            public float minLogPanelHeight, preferedLogPanelHeight;
        }
        
        #endregion;
    }
}

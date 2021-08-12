using UnityEngine;

namespace Bridge.Core.Debug
{
    public class RuntimeDebugger : DebugWindow
    {
        #region Components

        [SerializeField]
        private DebugData.ConsoleToggleButtonPosition consoleToggleButtonPosition;

        private Rect windowRect = new Rect { position = new Vector2(Screen.width / 4.0f, Screen.height / 4.0f), size = new Vector2(Screen.width/2.0f, Screen.height/2.0f)};
        private bool showDebugWindow;
        private int maxLogDisplayCharacterCount = 35;

        #endregion

        #region Unity

        private void OnEnable() => AddListeners(true);

        private void OnDisable() => AddListeners(false);

        private void OnDestroy() => AddListeners(false);

        private void Awake()
        {
            if (enable == false) return;

            DrawWindow(new Vector2(Screen.width, Screen.height), this.gameObject, (RectTransform)this.transform);
        }

        #endregion

        #region Main

        private void AddListeners(bool add)
        {
            if(add)
            {
                Debugger.AddListener += this.OnDebug;
            }
            else
            {
                Debugger.AddListener -= this.OnDebug;
            }
        }

        private void OnDebug(DebugData.LogType logType, object logClassName, object logMessage)
        {
            string formatedMessage = (string)logMessage;

            if(formatedMessage.Length <= maxLogDisplayCharacterCount)
            {
                formatedMessage = (string)logMessage;
            }
            else
            {
                formatedMessage = formatedMessage.Substring(0, maxLogDisplayCharacterCount - 2);
                formatedMessage += "...";
            }

            switch(logType)
            {
                case DebugData.LogType.LogInfo:

                    string rawInfoFormat = string.Format($"<color=blue>Info Message Results : </color><color=white>{logMessage}.</color><color=grey> From class : </color><color=white>{logClassName}</color>.");
                    string logInfoFormat = string.Format($"<color=blue>Log Info! : </color><color=white>{formatedMessage}.</color><color=grey> From class : </color><color=white>{logClassName}</color>.");

                    base.LogConsoleWindow(rawInfoFormat, logInfoFormat, DebugData.LogType.LogInfo);

                    if(logToUnityConsole)
                    {
                        Log(LogLevel.Debug, this, logInfoFormat);
                    }

                    break;

                case DebugData.LogType.LogWarning:

                    string rawWarningFormat = string.Format($"<color=orange>Warning Message Results : </color><color=white>{logMessage}.</color><color=grey> From class : </color><color=white>{logClassName}</color>.");
                    string logWarningFormat = string.Format($"<color=orange>Log Warning! </color>: <color=white>{formatedMessage}.</color><color=grey> From class : </color><color=white>{logClassName}</color>.");

                    base.LogConsoleWindow(rawWarningFormat, logWarningFormat, DebugData.LogType.LogWarning);

                    if (logToUnityConsole)
                    {
                        Log(LogLevel.Warning, this, logWarningFormat);
                    }

                    break;

                case DebugData.LogType.LogError:

                    string rawErrorFormat = string.Format($"<color=red>Error Message Results : </color><color=white>{logMessage}.</color><color=grey> From class : </color><color=white>{logClassName}</color>.");
                    string logErrorFormat = string.Format($"<color=red>Log Error!</color> : <color=white>{formatedMessage}.</color><color=grey> From class : </color><color=white>{logClassName}</color>.");

                    base.LogConsoleWindow(rawErrorFormat, logErrorFormat, DebugData.LogType.LogError);

                    if (logToUnityConsole)
                    {
                        Log(LogLevel.Error, this, logErrorFormat);
                    }

                    break;
            }
        }

        #endregion
    }
}

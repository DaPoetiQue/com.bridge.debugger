using UnityEngine;
using UnityEditor;
using Bridge.Core.Debug;

namespace Bridge.Core.UnityEditor.Debugger
{
    public class RuntimeDebugConsoleEditor : Editor
    {
        #region Create Console Window

        [MenuItem("3ridge/Create/Runtime Debugger")]
        private static void CreateRuntimeDebugConsole()
        {
            GameObject debugWindow = new GameObject("_3ridge Runtime Debug Console");
            debugWindow.AddComponent<RuntimeDebugger>();
        }

        [MenuItem("3ridge/Create/Runtime Debugger", true)]
        private static bool CanCreateRuntimeDebugConsole()
        {
            if (FindObjectOfType<RuntimeDebugger>())
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}

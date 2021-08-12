using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Bridge.Core.Debug
{
    [RequireComponent(typeof(RectTransform))]

    [DisallowMultipleComponent]

    public abstract class DebugWindow : MonoDebug, IDragHandler
    {
        #region Properties

        [SerializeField]
        protected bool enable = true;

        [Space(5)]
        [SerializeField]
        protected bool logToUnityConsole = false;

       [Space(5)]
        [SerializeField]
        protected Color consoleHeaderWindow = new Color(60.0f, 60.0f, 60.0f),
                        consoleWindow = new Color(30.0f, 30.0f, 30.0f),
                        consoleLogWindow = new Color(20.0f, 20.0f, 20.0f); 

        private RectTransform debugWindow = null, mainConsoleWindow = null;
        private bool windowCreated = false;
        private bool windowOpen = false;

        private Text logInfoCountDisplayer = null, logWarningCountDisplayer = null, logErrorCountDisplayer = null, logAllCountDisplayer = null;
        private Text descriptiveLogWindowMessageDisplayer = null;
        private int logInfoCount = 0, logWarningCount = 0, logErrorCount = 0, logAllCount = 0;
        private int headerTitleFontSize = 20, logMessageDisplayerFontSize = 20, descriptiveLogDisplayerFontSize = 20;
        private int descriptiveLogDisplayerLeftPaddingAmount = 25, descriptiveLogDisplayerRightPaddingAmount = 25;
        private float logPanelPaddingAmount = 25.0f;
        private float logPanelMinHeight = 50.0f, logPanelPreferedHeight = 85.0f, descriptiveLogInfoWindowMinHeight = 100.0f, descriptiveLogInfoWindowPreferedHeight = 150.0f;
        private string defaultDesLogWinDisplayerInfo = "Log.";

        [SerializeField]
        private List<DebugData.LogPanel> activeLogPanelList = new List<DebugData.LogPanel>();

        private Dictionary<string, Text> logMessageDictionary = new Dictionary<string, Text>();
        protected List<GameObject> logPanelDeleteList = new List<GameObject>();
        private List<string> logMessageDeleteList = new List<string>();

        #endregion

        #region Unity

        public void OnDrag(PointerEventData ped)
        {
            DragWindow(ped.delta);
        }

        #endregion

        #region Main

        protected virtual void DrawWindow(Vector2 screenResolution, GameObject debugWindowCanvas, RectTransform windowParent)
        {
            if (windowCreated) return;

            debugWindow = CreateWindow(screenResolution : screenResolution, debugWindowCanvas : debugWindowCanvas, windowParent : windowParent);
        }

        protected void LogConsoleWindow(string rawLogMessage, string logMessage, DebugData.LogType logType)
        {
            if(logMessageDeleteList.Contains(logMessage) == false)
            {
                switch (logType)
                {
                    case DebugData.LogType.LogInfo:

                        logInfoCount++;

                        break;

                    case DebugData.LogType.LogWarning:

                        logWarningCount++;

                        break;

                    case DebugData.LogType.LogError:

                        logErrorCount++;

                        break;
                }

                logAllCount++;

                CreateConsoleLog("Logger", rawLogMessage, logMessage, Color.black, this.logPanelMinHeight, this.logPanelPreferedHeight, mainConsoleWindow, logType);
                UpdateWindowContent();
                logMessageDeleteList.Add(logMessage);
            }
            else
            {
                // Return.
                return;
            }
        }

        private void UpdateWindowContent()
        {
            // Update window buttons title.
            SetTitle($"Info ({logInfoCount})", logInfoCountDisplayer);
            SetTitle($"Warning ({logWarningCount})", logWarningCountDisplayer);
            SetTitle($"Error ({logErrorCount})", logErrorCountDisplayer);
            SetTitle($"All ({logAllCount})", logAllCountDisplayer);
        }

        private void SetTitle(string title, Text titleDisplayer)
        {
            titleDisplayer.text = title;
        }

        private RectTransform CreateWindow(Vector2 screenResolution, GameObject debugWindowCanvas, RectTransform windowParent)
        {
            Vector2 currentResolution = Vector2.zero;

            if(screenResolution.x > screenResolution.y)
            {
                currentResolution = screenResolution;
            }
            else
            {
                currentResolution.x = screenResolution.y;
                currentResolution.y = screenResolution.x;
            }

            CreateCanvas(debugWindow: debugWindowCanvas, referenceResolution: currentResolution);

            GameObject debugWindow = new GameObject("Runtime Debug Window");
            RectTransform window = debugWindow.AddComponent<RectTransform>();
            window.anchoredPosition = Vector2.zero;
            window.sizeDelta = currentResolution / 2;

            Image windowBackground = debugWindow.AddComponent<Image>();
            windowBackground.color = Color.black;

            window.SetParent(windowParent, false);
            windowCreated = true;
            CreateWindowLayout(window);

            return window;
        }

        private void CreateCanvas(GameObject debugWindow, Vector2 referenceResolution)
        {
            Canvas debugWinCanvas = debugWindow.AddComponent<Canvas>();
            CanvasSetup(debugWinCanvas);

            CanvasScaler debugWinCanvasScaler = debugWindow.AddComponent<CanvasScaler>();
            CanvasScalerSetup(debugWindowCanvasScaler : debugWinCanvasScaler, referenceResolution : referenceResolution);

            debugWindow.AddComponent<GraphicRaycaster>();

            CreateEvent(debugWindow);
        }

        private void CreateWindowLayout(RectTransform window)
        {
            VerticalLayoutGroup layout = window.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 5.0f;

            RectTransform consoleHeaderWindowPanel = CreateWindowPanel("Console Window Header", this.consoleHeaderWindow, 50.0f, 50.0f, window);
            consoleHeaderWindowPanel.gameObject.AddComponent<HorizontalLayoutGroup>().spacing = 5.0f;

            Button clearConsoleButton = CreateWindowButton("Clear Button", "Clear", Color.grey, 100.0f, 150.0f, consoleHeaderWindowPanel, DebugData.WindowButtonType.Clear);
            Button logInfoConsoleButton = CreateWindowButton("Show Log Info Button", "Info (0)", Color.grey, 100.0f, 150.0f, consoleHeaderWindowPanel, DebugData.WindowButtonType.LogInfo);
            Button logWarningsConsoleButton = CreateWindowButton("Show Log Warning Button", "Warning (0)", Color.grey, 100.0f, 150.0f, consoleHeaderWindowPanel, DebugData.WindowButtonType.LogWarning);
            Button logErrorsConsoleButton = CreateWindowButton("Show Log Error Button", "Error (0)", Color.grey, 100.0f, 150.0f, consoleHeaderWindowPanel, DebugData.WindowButtonType.LogError);
            Button logAllConsoleButton = CreateWindowButton("Show Log All Button", "All (0)", Color.grey, 100.0f, 150.0f, consoleHeaderWindowPanel, DebugData.WindowButtonType.LogAll);

            mainConsoleWindow = CreateWindowPanel("Console Window", this.consoleWindow, window.sizeDelta.y / 2.0f, window.sizeDelta.y, window);
            mainConsoleWindow.gameObject.AddComponent<Mask>();
            VerticalLayoutGroup consloleWindowLayout = mainConsoleWindow.gameObject.AddComponent<VerticalLayoutGroup>();

            consloleWindowLayout.childControlWidth = true;
            consloleWindowLayout.childControlHeight = false;
            consloleWindowLayout.childForceExpandWidth = true;
            consloleWindowLayout.childForceExpandHeight = false;

            consloleWindowLayout.spacing = 5.0f;
            RectTransform consoleDescriptiveLogWindowPanel = CreateWindowPanel("Log Window", this.consoleLogWindow,descriptiveLogInfoWindowMinHeight, descriptiveLogInfoWindowPreferedHeight, window);
            HorizontalLayoutGroup consoleDescriptiveLogWindowPanelLayout = consoleDescriptiveLogWindowPanel.gameObject.AddComponent<HorizontalLayoutGroup>();

            consoleDescriptiveLogWindowPanelLayout.padding.left = descriptiveLogDisplayerLeftPaddingAmount;
            consoleDescriptiveLogWindowPanelLayout.padding.right = descriptiveLogDisplayerRightPaddingAmount;

            AddTextComponentToWindowPanel("Descriptive Log Displayer", descriptiveLogInfoWindowPreferedHeight, consoleDescriptiveLogWindowPanel);

            Button closeConsoleButton = CreateWindowButton("Close Button", "X", Color.red, 25.0f, 50.0f, consoleHeaderWindowPanel, DebugData.WindowButtonType.Close);
        }


        private void AddTextComponentToWindowPanel(string logDisplayerName, float panelPreferedHeight, RectTransform parentWindow)
        {
            GameObject logWindowMessageDisplayer = new GameObject(logDisplayerName);
            RectTransform logWindowMessageDisplayerRect = logWindowMessageDisplayer.AddComponent<RectTransform>();

            Vector2 anchorMin = Vector2.zero;
            anchorMin.y = 0.5f;
            Vector2 anchorMax = Vector2.zero;
            anchorMax.y = 0.5f;

            logWindowMessageDisplayerRect.anchorMin = anchorMin;
            logWindowMessageDisplayerRect.anchorMax = anchorMax;

            logWindowMessageDisplayerRect.pivot = anchorMin;

            Vector2 logDisplayTextPosition = Vector2.zero;
            logDisplayTextPosition.x = logPanelPaddingAmount;
            logWindowMessageDisplayerRect.anchoredPosition = logDisplayTextPosition;


            Text logTextDisplayer = logWindowMessageDisplayer.AddComponent<Text>();
            logTextDisplayer.lineSpacing = 1.5f;
            logTextDisplayer.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            logTextDisplayer.alignment = TextAnchor.MiddleLeft;
            logTextDisplayer.fontSize = descriptiveLogDisplayerFontSize;

            logTextDisplayer.horizontalOverflow = HorizontalWrapMode.Wrap;
            logTextDisplayer.verticalOverflow = VerticalWrapMode.Overflow;
            logTextDisplayer.text = defaultDesLogWinDisplayerInfo;

            Vector2 messageDisplayerSize = Vector2.zero;
            messageDisplayerSize.x = Screen.width / 2.0f;
            messageDisplayerSize.y = panelPreferedHeight;


            logWindowMessageDisplayerRect.sizeDelta = messageDisplayerSize;
            logWindowMessageDisplayer.transform.SetParent(parentWindow, false);

            descriptiveLogWindowMessageDisplayer = logTextDisplayer;
        }

        private RectTransform CreateWindowPanel(string panelName, Color bgColor, float minPanelHeight, float panelHeight, RectTransform parentWindow)
        {
            GameObject windowPanel = new GameObject(panelName);
            windowPanel.transform.SetParent(parentWindow, false);
            RectTransform panelRect = windowPanel.AddComponent<RectTransform>();
            Image panelBackground = windowPanel.AddComponent<Image>();
            panelBackground.color = bgColor;
            LayoutElement panelLayout = windowPanel.AddComponent<LayoutElement>();

            panelLayout.minHeight = minPanelHeight;
            panelLayout.preferredHeight = panelHeight;

            return panelRect;
        }

        private Button CreateWindowButton(string buttonName, string buttonTitle, Color buttonColor, float minButtonWidth, float preferedButtonWidth, RectTransform parentWindow, DebugData.WindowButtonType windowButtonType)
        {
            GameObject windowButton = new GameObject(buttonName);
            RectTransform windowButtonRect = windowButton.AddComponent<RectTransform>();
            windowButton.AddComponent<Image>().color = buttonColor;
            GameObject buttonText = new GameObject("Button Title");
            Button button = windowButton.AddComponent<Button>();

            switch (windowButtonType)
            {
                case DebugData.WindowButtonType.Close:

                    Text closeButtonTitleDisplayer = buttonText.AddComponent<Text>();

                    closeButtonTitleDisplayer.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

                    closeButtonTitleDisplayer.alignment = TextAnchor.MiddleCenter;
                    closeButtonTitleDisplayer.fontSize = headerTitleFontSize;
                    closeButtonTitleDisplayer.horizontalOverflow = HorizontalWrapMode.Overflow;
                    closeButtonTitleDisplayer.verticalOverflow = VerticalWrapMode.Overflow;

                    closeButtonTitleDisplayer.text = buttonTitle;

                    button.onClick.AddListener(this.CloseWindow);

                    break;

                case DebugData.WindowButtonType.LogInfo:

                    logInfoCountDisplayer = buttonText.AddComponent<Text>();
                    logInfoCountDisplayer.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

                    logInfoCountDisplayer.alignment = TextAnchor.MiddleCenter;
                    logInfoCountDisplayer.fontSize = headerTitleFontSize;

                    logInfoCountDisplayer.horizontalOverflow = HorizontalWrapMode.Overflow;
                    logInfoCountDisplayer.verticalOverflow = VerticalWrapMode.Overflow;

                    logInfoCountDisplayer.text = buttonTitle;
                    button.onClick.AddListener(() => this.ShowSelectedLogType(DebugData.LogType.LogInfo));

                    ColorBlock infoColorBlock = button.colors;
                    infoColorBlock.selectedColor = consoleWindow;
                    infoColorBlock.highlightedColor = consoleWindow;
                    infoColorBlock.pressedColor = consoleWindow;

                    button.colors = infoColorBlock;

                    break;

                case DebugData.WindowButtonType.LogWarning:

                    logWarningCountDisplayer = buttonText.AddComponent<Text>();
                    logWarningCountDisplayer.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
                    logWarningCountDisplayer.alignment = TextAnchor.MiddleCenter;
                    logWarningCountDisplayer.fontSize = headerTitleFontSize;
                    logWarningCountDisplayer.horizontalOverflow = HorizontalWrapMode.Overflow;
                    logWarningCountDisplayer.verticalOverflow = VerticalWrapMode.Overflow;
                    logWarningCountDisplayer.text = buttonTitle;

                    button.onClick.AddListener(() => this.ShowSelectedLogType(DebugData.LogType.LogWarning));

                    ColorBlock warningColorBlock = button.colors;
                    warningColorBlock.selectedColor = consoleWindow;
                    warningColorBlock.highlightedColor = consoleWindow;
                    warningColorBlock.pressedColor = consoleWindow;

                    button.colors = warningColorBlock;

                    break;

                case DebugData.WindowButtonType.LogError:

                    logErrorCountDisplayer = buttonText.AddComponent<Text>();
                    logErrorCountDisplayer.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
                    logErrorCountDisplayer.alignment = TextAnchor.MiddleCenter;
                    logErrorCountDisplayer.fontSize = headerTitleFontSize;

                    logErrorCountDisplayer.horizontalOverflow = HorizontalWrapMode.Overflow;
                    logErrorCountDisplayer.verticalOverflow = VerticalWrapMode.Overflow;
                    logErrorCountDisplayer.text = buttonTitle;

                    button.onClick.AddListener(() => this.ShowSelectedLogType(DebugData.LogType.LogError));

                    ColorBlock errorColorBlock = button.colors;
                    errorColorBlock.selectedColor = consoleWindow;
                    errorColorBlock.highlightedColor = consoleWindow;
                    errorColorBlock.pressedColor = consoleWindow;

                    button.colors = errorColorBlock;

                    break;

                case DebugData.WindowButtonType.LogAll: 

                    logAllCountDisplayer = buttonText.AddComponent<Text>();
                    logAllCountDisplayer.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
                    logAllCountDisplayer.alignment = TextAnchor.MiddleCenter;
                    logAllCountDisplayer.fontSize = headerTitleFontSize;
                    logAllCountDisplayer.horizontalOverflow = HorizontalWrapMode.Overflow;
                    logAllCountDisplayer.verticalOverflow = VerticalWrapMode.Overflow;
                    logAllCountDisplayer.text = buttonTitle;

                    button.onClick.AddListener(() => this.ShowSelectedLogType(DebugData.LogType.LogAll));

                    ColorBlock allColorBlock = button.colors;
                    allColorBlock.selectedColor = consoleWindow;
                    allColorBlock.highlightedColor = consoleWindow;
                    allColorBlock.pressedColor = consoleWindow;

                    button.colors = allColorBlock;

                    break;

                case DebugData.WindowButtonType.Clear:

                    Text clearButtonTitleDisplayer = buttonText.AddComponent<Text>();
                    clearButtonTitleDisplayer.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
                    clearButtonTitleDisplayer.alignment = TextAnchor.MiddleCenter;
                    clearButtonTitleDisplayer.fontSize = headerTitleFontSize;
                    clearButtonTitleDisplayer.horizontalOverflow = HorizontalWrapMode.Overflow;
                    clearButtonTitleDisplayer.verticalOverflow = VerticalWrapMode.Overflow;
                    clearButtonTitleDisplayer.text = buttonTitle;

                    button.onClick.AddListener(this.ClearLogs);

                    break;
            }

            buttonText.transform.SetParent(windowButtonRect, false);

            LayoutElement layoutElement = windowButton.AddComponent<LayoutElement>();
            layoutElement.minWidth = minButtonWidth;
            layoutElement.preferredWidth = preferedButtonWidth;

            windowButton.transform.SetParent(parentWindow, false);

            return button;
        }

        private void CreateConsoleLog(string logName, string rawLogMessage, string logMessage, Color panelColor, float minLogPanelHeight, float preferedLogPanelHeight, RectTransform parentWindow, DebugData.LogType logType)
        {
            GameObject logPanel = new GameObject(logName + " : " + logAllCount.ToString());
            RectTransform logPanelRect = logPanel.AddComponent<RectTransform>();

            Vector2 logPanelSize = Vector2.zero;
            logPanelSize.x = logPanelRect.sizeDelta.x;
            logPanelSize.y = preferedLogPanelHeight;

            logPanelRect.sizeDelta = logPanelSize;

            LayoutElement layoutElement = logPanel.AddComponent<LayoutElement>();
            layoutElement.minHeight = minLogPanelHeight;
            layoutElement.preferredHeight = preferedLogPanelHeight;

            Button logButton = logPanel.AddComponent<Button>();
            logButton.onClick.AddListener(() => this.ShowSelectedLog(rawLogMessage));
            logPanel.AddComponent<Image>().color = panelColor;

            GameObject logDisplayText = new GameObject("Log Message Text Displayer");
            RectTransform logDisplayTextRect = logDisplayText.AddComponent<RectTransform>();

            Vector2 anchorMin = Vector2.zero;
            anchorMin.y = 0.5f;
            Vector2 anchorMax = Vector2.zero;
            anchorMax.y = 0.5f;

            logDisplayTextRect.anchorMin = anchorMin;
            logDisplayTextRect.anchorMax = anchorMax;
            logDisplayTextRect.pivot = anchorMin;

            Vector2 logDisplayTextPosition = Vector2.zero;
            logDisplayTextPosition.x = logPanelPaddingAmount;
            logDisplayTextRect.anchoredPosition = logDisplayTextPosition;

            Text logTextDisplayer = logDisplayText.AddComponent<Text>();
            logTextDisplayer.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            logTextDisplayer.alignment = TextAnchor.MiddleLeft;

            // Set text font size.
            logTextDisplayer.fontSize = logMessageDisplayerFontSize;
            logTextDisplayer.horizontalOverflow = HorizontalWrapMode.Overflow;
            logTextDisplayer.verticalOverflow = VerticalWrapMode.Overflow;
            logTextDisplayer.text = logMessage;

            logDisplayText.transform.SetParent(logPanel.transform, false);
            logPanel.transform.SetParent(parentWindow, false);

            DebugData.LogPanel logPanelData = new DebugData.LogPanel { content = logPanel, logType = logType };

            activeLogPanelList.Add(logPanelData);
            logPanelDeleteList.Add(logPanel);
        }

        private void CreateEvent(GameObject debugWindow)
        {
            if (FindObjectOfType<EventSystem>() == false)
            {
                debugWindow.AddComponent<EventSystem>();
                debugWindow.AddComponent<StandaloneInputModule>();
                debugWindow.AddComponent<BaseInput>();
            }
        }

        private void ShowSelectedLog(string logMessage)
        {
            if (descriptiveLogWindowMessageDisplayer == null) return;

            descriptiveLogWindowMessageDisplayer.text = logMessage;
        }

        private void ShowSelectedLogType(DebugData.LogType logType)
        {
            if (logAllCount <= 0) return;

            switch (logType)
            {
                case DebugData.LogType.LogInfo:

                    for(int i = 0; i < logAllCount; i++)
                    {
                        if(activeLogPanelList[i].logType == logType)
                        {
                            activeLogPanelList[i].content.SetActive(true);
                        }
                        else
                        {
                            activeLogPanelList[i].content.SetActive(false);
                        }
                    }

                    break;

                case DebugData.LogType.LogWarning:

                    for (int i = 0; i < logAllCount; i++)
                    {
                        if (activeLogPanelList[i].logType == logType)
                        {
                            activeLogPanelList[i].content.SetActive(true);
                        }
                        else
                        {
                            activeLogPanelList[i].content.SetActive(false);
                        }
                    }

                    break;

                case DebugData.LogType.LogError:

                    for (int i = 0; i < logAllCount; i++)
                    {
                        if (activeLogPanelList[i].logType == logType)
                        {
                            activeLogPanelList[i].content.SetActive(true);
                        }
                        else
                        {
                            activeLogPanelList[i].content.SetActive(false);
                        }
                    }

                    break;


                case DebugData.LogType.LogAll:

                    for (int i = 0; i < logAllCount; i++)
                    {
                        activeLogPanelList[i].content.SetActive(true);
                    }

                    break;
            }
        }

        private Canvas CanvasSetup(Canvas debugWindowCanvas)
        {
            debugWindowCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            debugWindowCanvas.pixelPerfect = true;
            return debugWindowCanvas;
        }

        private CanvasScaler CanvasScalerSetup(CanvasScaler debugWindowCanvasScaler, Vector2 referenceResolution)
        {
            debugWindowCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            debugWindowCanvasScaler.referenceResolution = referenceResolution;

            return debugWindowCanvasScaler;
        }

        private void DragWindow(Vector2 dragPosition)
        {
            if (windowCreated == false) return;

            debugWindow.anchoredPosition += dragPosition;
        }

        private void CloseWindow()
        {
            windowOpen = !windowOpen;
            debugWindow.gameObject.SetActive(windowOpen);
        }

        private void ClearLogs()
        {
            if (logAllCount <= 0) return;

            for (int i = 0; i < logAllCount; i++)
            {
                Destroy(logPanelDeleteList[i]);
            }

            descriptiveLogWindowMessageDisplayer.text = defaultDesLogWinDisplayerInfo;

            logPanelDeleteList = new List<GameObject>();
            logMessageDeleteList = new List<string>();
            activeLogPanelList = new List<DebugData.LogPanel>();

            logInfoCount = 0;
            logWarningCount = 0;
            logErrorCount = 0;
            logAllCount = 0;

            UpdateWindowContent();
        }

        #endregion
    }
}

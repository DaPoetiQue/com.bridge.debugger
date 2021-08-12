using UnityEngine;
using UnityEngine.EventSystems;

namespace Bridge.Core.Debug
{
    [DisallowMultipleComponent]

    public class DraggableWindow : MonoBehaviour, IDragHandler
    {
        [SerializeField]
        private RectTransform draggableWindow;

        private void Start()
        {
            draggableWindow = this.GetComponent<RectTransform>();
        }

        public void OnDrag(PointerEventData ped)
        {
            Debugger.Log(DebugData.LogType.LogInfo, this, $"--->>><color=orange>Dragging mouse at pos : </color><color=white>{ped.delta.ToString()}</color>");

            draggableWindow.anchoredPosition += ped.delta;
        }
    }
}

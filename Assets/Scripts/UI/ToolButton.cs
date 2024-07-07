using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private ToolType toolType;

    private HasNumberButton hasNumberButton = null;

    private void Start()
    {
        hasNumberButton = GetComponent<HasNumberButton>();
    }
   
    public void OnPointerDown(PointerEventData eventData)
    {
        if (hasNumberButton != null)
        {
            if (hasNumberButton.GetCurrentNumber() > 0)
            {
                ToolManager.Instance.SelectTool(toolType);
            }
            else
            {
                MenuPanelManager.Instance.SelectPanel(MenuPanel.Shop);
            }
        }
        else
        {
            ToolManager.Instance.SelectTool(toolType);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ToolManager.Instance.DeSelectTool();
    }
}

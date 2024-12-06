using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public enum TutorialButtonType
{
    Click,
    Drag
}

public class TutorialButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private TutorialButtonType type;
    private GameObject indicator;
    private bool isDragging = false;

    public void SetUp(GameObject indicatorSpawned, TutorialButtonType type)
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClickButton);
        indicator = indicatorSpawned;
        this.type = type;
    }

    private void OnClickButton()
    {
        if (type == TutorialButtonType.Click)
        {
            Debug.Log("Has cliked: remove indicator");
            RemoveIndicator();
        }    
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDragging && type == TutorialButtonType.Drag)
        {
            Debug.Log("Has drag: remove indicator");
            RemoveIndicator();
        }
        isDragging = false;
    }

    private void RemoveIndicator()
    {
        if (indicator != null)
        {
            indicator.SetActive(false);
            Destroy(indicator);
        }
        else
        {
            RemoveComponent();
        }    
    }

    public void RemoveComponent()
    {
        Destroy(this.GetComponent<TutorialButton>());
    }
}

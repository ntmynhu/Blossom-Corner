using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameObject indicator;
    private bool isDragging = false;

    public void SetUp(GameObject indicatorSpawned)
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClickButton);
        indicator = indicatorSpawned;
    }

    private void OnClickButton()
    {
        RemoveIndicator();
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
        if (isDragging)
        {
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
    }

    public void RemoveComponent()
    {
        Destroy(this.GetComponent<TutorialButton>());
    }
}

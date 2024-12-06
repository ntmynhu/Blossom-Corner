using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageTutorialButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (GameManager.Instance.IsFirstTimePlayer())
        {
            if (TutorialManager.Instance.GetCurrentStepIndex() == 10)
            {
                TutorialManager.Instance.OnActionCompleted(2f);
            }    
        }  
        else
        {
            GetComponent<Button>().onClick.RemoveListener(OnButtonClick);
        }    
    }    
}

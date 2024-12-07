using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialButtonClick : MonoBehaviour
{
    [SerializeField] private int step;
    [SerializeField] private float time;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (GameManager.Instance.IsFirstTimePlayer())
        {
            if (TutorialManager.Instance.GetCurrentStepIndex() == step)
            {
                TutorialManager.Instance.OnActionCompleted(time);
            }    
        }  
        else
        {
            GetComponent<Button>().onClick.RemoveListener(OnButtonClick);
        }    
    }    
}

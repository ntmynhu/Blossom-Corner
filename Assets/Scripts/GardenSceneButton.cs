using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GardenSceneButton : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        sceneController.LoadGardenScene();

        if (GameManager.Instance.IsFirstTimePlayer())
        {
            if (TutorialManager.Instance.GetCurrentStepIndex() == 1) // Step chờ click vào menu sang garden
            {
                TutorialManager.Instance.OnActionCompleted(.5f);
            } 
        }    
    }    
}

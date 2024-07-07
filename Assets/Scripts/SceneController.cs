using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public void LoadHomeScene()
    {
        MainMenuManager.Instance.SwitchToHomeScene();
    }

    public void LoadGardenScene()
    {
        MainMenuManager.Instance.SwitchToGardenScene();
    }

    public void LoadFlowerShopScene()
    {
        MainMenuManager.Instance.SwitchToFlowerShopScene();
    }
}

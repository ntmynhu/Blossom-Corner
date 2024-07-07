using System;
using System.Collections;
using UnityEngine;
public enum Scene
{
    HomeScene,
    GardenScene,
    FlowerShopScene,
}

public class MainMenuManager : MonoBehaviour
{
    #region Singleton

    private static MainMenuManager instance;
    public static MainMenuManager Instance => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    #endregion

    public event EventHandler OnSceneChanged;

    private Vector3 homeScenePos = new Vector3(50, 0, -20);
    private Vector3 gardenScenePos = new Vector3(0, 0, -20);
    private Vector3 flowershopScenePos = new Vector3(-50.4f, 0, -20);

    private Camera mainCamera;
    private Scene currentScene;

    /*public IEnumerator Init()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
            yield break;
        }

        yield return new WaitForSeconds(0.2f);

        SwitchToHomeScene();
        Debug.Log("Main menu switch to home scene");
    }*/

    public void Init()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
            return;
        }

        SwitchToHomeScene();
    }

    public void SwitchToHomeScene()
    {
        SetCameraPosition(homeScenePos);
        currentScene = Scene.HomeScene;
        GameUIManager.Instance.DeactivateUIList();
        OnSceneChanged?.Invoke(this, new EventArgs());
    }

    public void SwitchToGardenScene()
    {
        SetCameraPosition(gardenScenePos);
        currentScene = Scene.GardenScene;
        GameUIManager.Instance.ActivateGardenUIList();
        OnSceneChanged?.Invoke(this, new EventArgs());
    }

    public void SwitchToFlowerShopScene()
    {
        SetCameraPosition(flowershopScenePos);
        currentScene = Scene.FlowerShopScene;
        GameUIManager .Instance.ActivateFlowerShopUIList();
        OnSceneChanged?.Invoke(this, new EventArgs());
    }

    private void SetCameraPosition(Vector3 position)
    {
        if (mainCamera != null)
        {
            mainCamera.transform.position = position;
        }
    }

    public Scene GetCurrentScene()
    {
        return currentScene;
    }
}


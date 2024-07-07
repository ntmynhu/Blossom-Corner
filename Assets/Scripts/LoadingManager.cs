using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    [Header("Main Menu Objects")]
    [SerializeField] private GameObject loadingBarObject;
    [SerializeField] private Image loadingBar;
    [SerializeField] private GameObject gCanvasLoading;

    [Header("Scenes to Load")]
    [SerializeField] private string persistenceGamePlayScene = "PersistenceGamePlayScene";
    [SerializeField] private string UIScene = "UIScene";
    [SerializeField] private string homeScene = "HomeScene";
    [SerializeField] private string gardenScene = "GardenScene";
    [SerializeField] private string flowerShopScene = "FlowerShopScene";

    private List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

    private IEnumerator Start()
    {
        ///B
        gCanvasLoading.gameObject.SetActive(true);
        loadingBarObject.SetActive(true);

        //Load game data phải được ưu tiên trước nhất
        DataPersistenceManager.Instance.InitAndLoadGame();

        yield return new WaitForEndOfFrame();

        //thời gian load xong của 5 scene này là không đảm bảo
        //Trên dt của em, khả năng cao là: Scene UI/Home... load lên trước khi GamePlayScene => GameplayScene không pushData về các scene này được
        //ASync = bất đồng bộ
        //Sync = Đồng bộ
        scenesToLoad.Add(SceneManager.LoadSceneAsync(persistenceGamePlayScene));
        scenesToLoad.Add(SceneManager.LoadSceneAsync(UIScene, LoadSceneMode.Additive));
        scenesToLoad.Add(SceneManager.LoadSceneAsync(homeScene, LoadSceneMode.Additive));
        scenesToLoad.Add(SceneManager.LoadSceneAsync(gardenScene, LoadSceneMode.Additive));
        scenesToLoad.Add(SceneManager.LoadSceneAsync(flowerShopScene, LoadSceneMode.Additive));

        StartCoroutine(ProgressLoadingBar());
    }

    private IEnumerator ProgressLoadingBar()
    {
        float loadProgress = 0f;
        for (int i = 0; i < scenesToLoad.Count; i++)
        {
            while (!scenesToLoad[i].isDone)
            {
                loadProgress += scenesToLoad[i].progress;
                loadingBar.fillAmount = loadProgress / scenesToLoad.Count;
                yield return null;
            }
        }

        //sau khi mọi scene đã được load xong => gọi hàm Init của mỗi scene thay vì để scene tự gọi Start => quyết định được thứ tự
        InitAllScene();

        //tắt loading
        gCanvasLoading.gameObject.SetActive(false);
    }

    void InitAllScene()
    {
        DataPersistenceManager.Instance.PushLoadedDataToObject();

        //bị ràng buộc
        //init các scene

        Debug.Log("Init gameUIManager");
        GameUIManager.Instance.Init();
        Debug.Log("Init gameUIManager DOne");
        InventoryUIManager.Instance.Init();
        MainMenuManager.Instance.Init();

        ///Em sửa các hàm Start của các Manager khác, xong gọi ở đây nha
        ///

        ///Hoặc có thể xài Observer pattern ở đây
        //push data tới object -> lúc này các scene đã load xong hết

        DataPersistenceManager.Instance.SetLoadedDataDone();
    }
}

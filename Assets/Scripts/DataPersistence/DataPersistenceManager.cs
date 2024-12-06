using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    private string dataKey = "gameData";
    private bool useEncryption = false;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private PlayerPrefsDataHandler dataHandler;

    public bool isLoadedDataDone = false;

    #region Singleton

    private static DataPersistenceManager instance;
    public static DataPersistenceManager Instance => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    #endregion

    /// <summary>
    /// đổi hàm call
    /// Nếu dùng Start: Rủi ro không quản lý được Hàm start của DataPerManager hay là hàm Start của LoadingManager gọi trước
    /// Nên mình sẽ dồn lại 1 chỗ luôn
    /// </summary>
    private void Start()
    {

    }
    /// <summary>
    /// Dùng hàm này thay vì hàm Start
    /// </summary>
    public void InitAndLoadGame() //void Start
    {
        this.dataHandler = new PlayerPrefsDataHandler(dataKey, useEncryption);
        //lúc này đang ở loading -> chưa có scene -> ko call dc
        //this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    /*private IEnumerator Start()
    {
        this.dataHandler = new PlayerPrefsDataHandler(dataKey, useEncryption);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        yield return new WaitForSeconds(0.2f);
        LoadGame();
    }*/

    const float SAVE_INTERVAL = 30f;
    float timeSave = SAVE_INTERVAL;
    private void Update()
    {
        timeSave -= Time.deltaTime;
        if (timeSave <= 0)
        {
            SaveGame();
            timeSave = SAVE_INTERVAL;
        }
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        // load any saved data from a file using data handler
        this.gameData = dataHandler.Load();

        // if there is no data to load
        if (this.gameData == null)
        {
            Debug.Log("No date was found. Initializing data to defaults.");
            NewGame();
        }

        // push the loaded data to all other scripts that need it
        //Hoang: dời ra riêng sau khi đảm bảo scene đã load xong nha
    }
    public void PushLoadedDataToObject()
    {
        //this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        UpdateAllDataPersistenceObjects();

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            Debug.Log("Load: " + dataPersistenceObj);
            dataPersistenceObj.LoadData(gameData);
        }
    }
    public void SetLoadedDataDone()
    {
        isLoadedDataDone = true;
        Debug.Log("Loaded done");
    }

    public void SaveGame()
    {
        if (isLoadedDataDone && !GameManager.Instance.IsFirstTimePlayer())
        {
            UpdateAllDataPersistenceObjects();

            // save the data in all other scripts that need to save data
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.SaveData(ref gameData);
            }

            // save the data to a file using data handler
            dataHandler.Save(gameData);
        }    
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (isLoadedDataDone && pauseStatus)
        {
            SaveGame();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (isLoadedDataDone && !hasFocus)
        {
            SaveGame();
        }
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        // FindObjectsofType takes in an optional boolean to include inactive gameobjects
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IDataPersistence>();        

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public void UpdateAllDataPersistenceObjects()
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        Debug.Log("Update");
    }
}

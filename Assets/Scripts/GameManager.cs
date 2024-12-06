using System;
using UnityEngine;

public class GameManager : MonoBehaviour, IDataPersistence
{
    #region Singleton
    private static GameManager instance;
    public static GameManager Instance => instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

    }
    #endregion

    public event EventHandler OnLevelChanged;

    private int currentMoney;
    private int playerLevel;
    private int expPointMax;
    private int currentExpPoint;

    private bool isFirstTimePlayer;

    public int GetCurrentMoney()
    {
        return currentMoney;
    }

    public float GetCurrentExpPointPercentage()
    {
        //Debug.Log(1.0f * currentExpPoint / expPointMax);
        return 1.0f * currentExpPoint / expPointMax;
    }

    public void SetCurrentMoney(int money)
    {
        currentMoney = money;
        GameUIManager.Instance.UpdateMoneyUI();
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        GameUIManager.Instance.UpdateMoneyUI();
        Debug.Log("AddMoney: " + currentMoney);
    }

    public void SubtractMoney(int amount)
    {
        currentMoney -= amount;
        Debug.Log("SubtractMoney: " + currentMoney);
    }

    public void LoadData(GameData data)
    {
        currentExpPoint = data.playerEXPPoint;
        expPointMax = data.playerEXPPointMax;
        this.currentMoney = data.playerMoney;
        this.playerLevel = data.playerLevel;
        Debug.Log("Load GameManager: " + currentExpPoint + " " + expPointMax + " " + currentMoney + " " + playerLevel);

        isFirstTimePlayer = data.isFirstTimePlayer;
        if (isFirstTimePlayer)
        {
            Debug.Log("Start tutorial");
            TutorialManager.Instance.StartTutorial();
        }
        else
        {
            TutorialManager.Instance.EndTutorial();
        }
    }
    public void SaveData(ref GameData data)
    {
        data.playerMoney = currentMoney;  
        data.playerLevel = playerLevel;
        data.playerEXPPoint = currentExpPoint;
        data.playerEXPPointMax = expPointMax;

        data.isFirstTimePlayer = !TutorialManager.Instance.IsFinished();
    }

    public int GetPlayerLevel()
    {
        return playerLevel;
    }

    public void AddExpPoint(int experienceAmount)
    {
        currentExpPoint += experienceAmount;
        if (currentExpPoint >= expPointMax)
        {
            LevelUp();
        }
        GameUIManager.Instance.UpdateInfoUI();
    }

    public void LevelUp()
    {
        playerLevel++;
        currentExpPoint = 0;
        expPointMax += 150;
        GameUIManager.Instance.UpdateInfoUI();
        OnLevelChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsFirstTimePlayer()
    {
        isFirstTimePlayer = !TutorialManager.Instance.IsFinished();
        return isFirstTimePlayer;
    }
}

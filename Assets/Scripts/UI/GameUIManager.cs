using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    #region Singleton

    private static GameUIManager instance;
    public static GameUIManager Instance => instance;

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

    [System.Serializable]
    public struct levelData
    {
        public List<GameObject> level;
    }

    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private List<GameObject> gardenUIList;
    [SerializeField] private List<GameObject> flowerShopUIList;
    [SerializeField] private Image experienceBar;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private List<levelData> levelList;

    public void Init()
    {
        UpdateMoneyUI();
        DeactivateUIList();
        UpdateInfoUI();
        ShowUIObjectsOfLevel(GameManager.Instance.GetPlayerLevel());
        GameManager.Instance.OnLevelChanged += Instance_OnLevelChanged;
        Debug.Log("Game UI Init");
    }

    private void Instance_OnLevelChanged(object sender, System.EventArgs e)
    {
        ShowUIObjectsOfLevel(GameManager.Instance.GetPlayerLevel());
    }

    public void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = GameManager.Instance.GetCurrentMoney().ToString();
        }
    }

    public void UpdateInfoUI()
    {
        experienceBar.fillAmount = GameManager.Instance.GetCurrentExpPointPercentage();
        levelText.text = "Level " + GameManager.Instance.GetPlayerLevel();
    }

    public void DeactivateUIList()
    {
        foreach (GameObject obj in flowerShopUIList)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in gardenUIList)
        {
            obj.SetActive(false);
        }
    }

    public void ActivateGardenUIList()
    {
        foreach (GameObject obj in flowerShopUIList)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in gardenUIList)
        {
            if (obj.gameObject.name != "Seeds")
            {
                obj.gameObject.SetActive(true);
            }
        }
    }

    public void ActivateFlowerShopUIList()
    {
        foreach (GameObject obj in gardenUIList)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in flowerShopUIList)
        {
            obj.SetActive(true);
        }
    }

    public void ShowUIObjectsOfLevel(int level)
    {
        int targetLevel = Mathf.Min(level - 1, levelList.Count - 1);

        for (int i = 0; i <= targetLevel; i++)
        {
            foreach (GameObject gobj in levelList[i].level)
            {
                gobj.SetActive(true);
            }
        }
    }    
}

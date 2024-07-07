using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    #region Singleton

    private static ShopManager instance;
    public static ShopManager Instance => instance;

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

    [SerializeField] private List<HasNumberButton> hasNumberButtons = new List<HasNumberButton>();

    public void Purchase(ToolType toolType)
    {
        HasNumberButton target = null;
        foreach (var hasNumberButton in hasNumberButtons)
        {
            if (hasNumberButton.GetToolType() == toolType)
            {
                target = hasNumberButton;
                break;
            }
        }

        if (target != null)
        {
            target.IncreaseNumber();
        }
    }

    public void DecreaseNumber(ToolType toolType)
    {
        HasNumberButton target = null;
        foreach (var hasNumberButton in hasNumberButtons)
        {
            if (hasNumberButton.GetToolType() == toolType)
            {
                target = hasNumberButton;
                break;
            }
        }

        if (target != null)
        {
            target.DecreaseNumber();
        }
    }

    public int GetcurrentNumber(ToolType toolType)
    {
        HasNumberButton target = null;
        foreach (var hasNumberButton in hasNumberButtons)
        {
            if (hasNumberButton.GetToolType() == toolType)
            {
                target = hasNumberButton;
                break;
            }
        }

        if (target != null)
        {
            return target.GetCurrentNumber();
        }
        else
        {
            return 0;
        }
    }
}

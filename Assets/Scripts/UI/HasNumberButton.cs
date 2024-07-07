using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HasNumberButton : MonoBehaviour, IDataPersistence
{
    [SerializeField] protected TextMeshProUGUI numberText;
    [SerializeField] protected ToolType toolType;

    protected int currentNumber;

    private void SetUINumber()
    {
        numberText.text = currentNumber.ToString();
    }

    public int GetCurrentNumber()
    {
        return currentNumber;
    }

    public void IncreaseNumber()
    {
        currentNumber++;
        Debug.Log(currentNumber);
        numberText.text = currentNumber.ToString();
    }

    public void DecreaseNumber()
    {
        currentNumber--;
        numberText.text = currentNumber.ToString();
    }

    public ToolType GetToolType()
    {
        return toolType;
    }

    public void LoadData(GameData data)
    {
        data.hasNumberButtons.TryGetValue(gameObject.name, out int currentNumber);
        this.currentNumber = currentNumber;
        Debug.Log(gameObject.name + currentNumber);
        SetUINumber();
    }

    public void SaveData(ref GameData data)
    {
        if (data.hasNumberButtons.ContainsKey(gameObject.name))
        {
            data.hasNumberButtons.Remove(gameObject.name);
        }
        data.hasNumberButtons.Add(gameObject.name, currentNumber);
        Debug.Log(gameObject.name + currentNumber);
    }
}

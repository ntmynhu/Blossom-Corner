using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameInput : MonoBehaviour, IDataPersistence
{
    [SerializeField] private TMP_InputField inputField;

    private string lastestName;

    private void Start()
    {
        // Subscribe to the input field's OnEndEdit event
        inputField.onEndEdit.AddListener(OnEndEdit);
    }

    // Method to handle the end of editing in the input field
    private void OnEndEdit(string text)
    {
        // Save the data whenever the editing of the input field ends
        lastestName = text;
        inputField.text = lastestName;
    }

    // Method to programmatically activate the input field
    public void ActivateInputField()
    {
        inputField.ActivateInputField();
    }


    public void LoadData(GameData data)
    {
        lastestName = data.playerName;
        inputField.text = lastestName;
    }

    public void SaveData(ref GameData data)
    {
        data.playerName = lastestName;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MoneyButton : MonoBehaviour
{
    public event EventHandler OnCollectMoney;
    [SerializeField] private GameObject moneyPanel;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }
    private void OnButtonClick()
    {
        OnCollectMoney?.Invoke(this, EventArgs.Empty);
        moneyPanel.SetActive(false);
    }
}

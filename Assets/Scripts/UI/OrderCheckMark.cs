using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderCheckMark : MonoBehaviour
{
    public event EventHandler OnOrderCheckMarkClicked;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        AudioManager.Instance.PlaySPF(AudioManager.Instance.moneyPickup);
        OnOrderCheckMarkClicked?.Invoke(this, EventArgs.Empty);
    }
}

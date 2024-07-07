using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SetActiveButton: MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private void Start() 
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }
    private void OnButtonClick()
    {
        panel.SetActive(!panel.gameObject.activeSelf);
    }
}

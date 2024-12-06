using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotSpot : MonoBehaviour
{
    [SerializeField] private GameObject potPrefab;

    private bool hasPot = false;
    private GameObject pot = null;

    private void Update()
    {
        if (pot == null && hasPot)
        {
            hasPot = false ;
            GetComponent<BoxCollider2D>().enabled = true;
        }    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pot") && !hasPot)
        {
            if (PotButton.Instance.GetCurrentNumber() > 0)
            {
                pot = Instantiate(potPrefab, this.transform);
                hasPot = true;

                // Tắt collider để tránh trùng với collider của pot
                GetComponent<BoxCollider2D>().enabled = false;
                PotButton.Instance.UpdatePotButtonUI();
            }    
        }    
    }
}

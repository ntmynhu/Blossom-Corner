using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    [SerializeField] private Transform flowerPos;

    private bool isPlacingFlower = false;
    private bool isSelected = false;
    private SpriteRenderer spriteRenderer;
    private Vector3 initialPos;
    private bool isClicked = false;
    private FlowerSO flowerSOofPot;

    private bool isAboveOthers = false;

    private void Start()
    {
        initialPos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isPlacingFlower && isClicked && Input.GetMouseButton(0))
        {
            Vector2 mousePos = GetMousePosition();
            transform.position = mousePos;
        }
        else
        {
            transform.position = initialPos;
            isClicked = false;
        }
    }   

    private Vector2 GetMousePosition()
    {
        return (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDown()
    {
        if (!isPlacingFlower)
        {
            if (!isSelected)
            {
                PotStandManager.Instance.SelectPot(this);
            }
            else
            {
                PotStandManager.Instance.DeselectPot();
            }
        }

        isClicked = true;
    }  

    public bool IsClicked()
    {
        return isClicked;
    }

    public void ActivateSelectedVisual()
    {
        if (!isPlacingFlower)
        {
            spriteRenderer.color = new Color(0.8f, 0.8f, 0.8f, 1f);
        }
        isSelected = true;
    }

    public void DeactivateSelectedVisual()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = Color.white;
        isSelected = false;
    }

    public void SetIsAboveOthers(bool isAbove)
    {
        isAboveOthers = isAbove;
    }

    public void PlaceFlower(FlowerSO flowerSO)
    {
        GameObject flowerSpawned = Instantiate(flowerSO.flowerPrefab, this.transform);
        flowerSpawned.transform.position = flowerPos.position;

        if (isAboveOthers)
        {
            SpriteRenderer flowerRenderer = flowerSpawned.GetComponent<SpriteRenderer>();
            flowerRenderer.sortingOrder += 2;
        }
        
        isPlacingFlower = true;
        flowerSOofPot = flowerSO;
    }

    public bool IsPlacingFlower()
    {
        return isPlacingFlower;
    }

    /*public void HandleSelling(BaseCustomer baseCustomer)
    {
        if (isCollidingWithCustomer && flowerSOofPot != null)
        {
            if (Input.GetMouseButtonUp(0))
            {
                foreach (var flowerPreference in baseCustomer.GetCustomerPreferenceSO().flowerPreferences)
                {
                    if (flowerSOofPot == flowerPreference.flowerSO)
                    {

                    }
                }
            }
        }
    } *//*   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Customer"))
        {
            isCollidingWithCustomer = true;
            OnSellingFlowerPot?.Invoke(this, new OnSellingFLowerPotEventArgs { pot = this });
        }  
    }*/

    public FlowerSO GetFlowerSOofPot()
    {
        return flowerSOofPot;
    }
}

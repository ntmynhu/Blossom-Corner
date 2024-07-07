using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotStand : MonoBehaviour
{
    [Serializable]
    public class PlacingSpot
    {
        public Transform placingPos;
        public bool isPlaced;

    }

    [SerializeField] private List<PlacingSpot> placingSpotList;

    private bool isAllPotsPlaced = false;
    private bool isSelected = false;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (!isSelected && !IsAllPotsPlaced())
        {
            PotStandManager.Instance.SelectPotStand(this);
        }
        else
        {
            isSelected = false;
            PotStandManager.Instance.DeselectPotStand();
        }
    }

    public void ActivateSelectedVisual()
    {
        spriteRenderer.color = new Color(0.8f, 0.8f, 0.8f, 1f);
        isSelected = true;
    }

    public void DeactivateSelectedVisual()
    {
        spriteRenderer.color = Color.white;
        isSelected = false;
    }

    public void PlacePot(GameObject potPrefab)
    {
        for (int i = 0; i < placingSpotList.Count; i++)
        {
            if (!placingSpotList[i].isPlaced)
            {
                GameObject potSpawned = Instantiate(potPrefab, placingSpotList[i].placingPos);
                potSpawned.transform.position = placingSpotList[i].placingPos.position;

                if (i > 3)
                {
                    SpriteRenderer potRenderer = potSpawned.GetComponent<SpriteRenderer>();
                    potRenderer.sortingOrder += 2;
                    Pot pot = potSpawned.GetComponent<Pot>();
                    pot.SetIsAboveOthers(true);
                }

                if (placingSpotList[i].placingPos.childCount > 0)
                {
                    placingSpotList[i].isPlaced = true;
                }

                break;
            }
        }

        isAllPotsPlaced = AllPotsPlaced();
    }

    private bool AllPotsPlaced()
    {
        foreach (PlacingSpot t in placingSpotList)
        {
            if (t.placingPos.childCount == 0)
            {
                t.isPlaced = false;
                return false;
            }
        }

        PotStandManager.Instance.DeselectPotStand();
        return true;
    }

    public bool IsAllPotsPlaced()
    {
        return isAllPotsPlaced = AllPotsPlaced();
    }
}

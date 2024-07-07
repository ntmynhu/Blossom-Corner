using System.Collections.Generic;
using UnityEngine;

public class PotStandManager : MonoBehaviour
{
    #region Singleton

    private static PotStandManager instance;
    public static PotStandManager Instance => instance;

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


    [SerializeField] private GameObject potPrefab;

    private PotStand selectedPotStand;
    private List<Pot> selectedPots = new List<Pot>();
    private bool isSelectingPotStand = false;
    private bool isSelectingPot = false;

    public void SelectPotStand(PotStand potStand)
    {
        if (selectedPotStand != null)
        {
            DeselectPotStand();
        }

        if (selectedPots.Count > 0)
        {
            DeselectPot();
        }
        
        if (!potStand.IsAllPotsPlaced())
        {
            selectedPotStand = potStand;
            selectedPotStand.ActivateSelectedVisual();
            isSelectingPotStand = true;
        }
    }

    public void DeselectPotStand()
    {
        if (isSelectingPotStand)
        {
            selectedPotStand.DeactivateSelectedVisual();
            isSelectingPotStand = false;
            selectedPotStand = null;
        }
    }

    public void SelectPot(Pot pot)
    {
        if (selectedPotStand != null)
        {
            DeselectPotStand();
        }

        selectedPots.Add(pot);
        pot.ActivateSelectedVisual();
        isSelectingPot = true;
    }

    public void DeselectPot()
    {
        if (isSelectingPot)
        {
            foreach (var selectedPot in selectedPots)
            {
                selectedPot.DeactivateSelectedVisual();
            }
            isSelectingPot = false;
            selectedPots.Clear();
        }
    }

    public void PlacePot()
    {
        if (isSelectingPotStand && !selectedPotStand.IsAllPotsPlaced())
        {
            selectedPotStand.PlacePot(potPrefab);
        }
    }

    public void PlaceFlower(FlowerSO flowerSO)
    {
        foreach (var selectedPot in selectedPots)
        {
            if (isSelectingPot && !selectedPot.IsPlacingFlower())
            {
                selectedPot.PlaceFlower(flowerSO);

                selectedPot.DeactivateSelectedVisual();

                InventoryManager.Instance.DecreaseNumber(flowerSO);

                FlowerShopManager.Instance.AddFlowerOnShown(flowerSO);
            }
        }

        DeselectPot();
    }

    public List<Pot> GetSelectedPots()
    {
        return selectedPots;
    }

    public bool IsSelectingPotStand()
    {
        return isSelectingPotStand;
    }
}

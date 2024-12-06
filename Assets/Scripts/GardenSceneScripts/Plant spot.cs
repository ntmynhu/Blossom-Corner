using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum PlantSpotState
{
    BlankSpot,
    Hole,
    SoilMoundlet,
}

public class Plantspot : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject blankSpot;
    [SerializeField] private GameObject hole;
    [SerializeField] private GameObject soilMoundlet;
    [SerializeField] private GameObject tulipPrefab;
    [SerializeField] private GameObject sunflowerPrefab;
    [SerializeField] private GameObject daisyPrefab;
    [SerializeField] private GameObject hydrangeaPrefab;
    [SerializeField] private ShovelAnimation shovelAnimation;
    [SerializeField] private float diggingTimeMax;

    [SerializeField] private string id;
    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = Guid.NewGuid().ToString();
    }

    private PlantSpotState currentState;
    private GameObject currentVisual;
    private bool isCollidingWithShovel = false;
    private bool isCollidingWithSeedBag= false;
    private float yDistance = 1.55f;
    private float diggingTime;


    private GameObject currentFlower;
    private string currentFlowerID;
    private GameObject currentFlowerPrefab; //Giữ lại prefab mà code instantiate

    private bool isFirstTimeDigging = true;
    private bool isFirstTimePlanting = true;

    private void Start()
    {

    }
    private void Update()
    {
        if (currentState == PlantSpotState.SoilMoundlet && currentFlower == null)
        {
            SetState(PlantSpotState.BlankSpot);
        }
    }

    private void SetState(PlantSpotState state)
    {
        currentState = state;
        if (currentVisual != null)
        {
            currentVisual.SetActive(false);
        }

        switch (state)
        {
            case PlantSpotState.BlankSpot:
                blankSpot.SetActive(true);
                currentVisual = blankSpot;
                break;
            case PlantSpotState.Hole:
                hole.SetActive(true);
                currentVisual = hole;
                break;
            case PlantSpotState.SoilMoundlet:
                soilMoundlet.SetActive(true);
                currentVisual = soilMoundlet;
                break;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Shovel"))
        {
            isCollidingWithShovel = true;
        }
        else if (collision.gameObject.CompareTag("SeedBag"))
        {
            isCollidingWithSeedBag = true;
        }

        if (currentState == PlantSpotState.BlankSpot && ToolManager.Instance.IsHoldingTool() && isCollidingWithShovel && Input.GetMouseButton(0))
        {
            AudioManager.Instance.PlaySPF(AudioManager.Instance.shovel);
            SetState(PlantSpotState.Hole);

            if (GameManager.Instance.IsFirstTimePlayer() && isFirstTimeDigging)
            {
                TutorialManager.Instance.OnActionCompleted(2f);
                isFirstTimeDigging = false;
            }
        }

        if (currentState == PlantSpotState.Hole && ToolManager.Instance.IsHoldingTool() && isCollidingWithSeedBag && Input.GetMouseButton(0))
        {
            ToolType toolType = ToolManager.Instance.GetToolType();
            if (ShopManager.Instance.GetcurrentNumber(toolType) > 0)
            {
                switch (toolType)
                {
                    case ToolType.TulipSeedBag:
                        PlantFlower(tulipPrefab);
                        break;
                    case ToolType.SunflowerSeedBag:
                        PlantFlower(sunflowerPrefab);
                        break;
                    case ToolType.DaisySeedBag:
                        PlantFlower(daisyPrefab);
                        break;
                    case ToolType.HydrangeaSeedBag:
                        PlantFlower(hydrangeaPrefab);
                        break;
                    default:
                        break;
                }

                SetState(PlantSpotState.SoilMoundlet);
                ShopManager.Instance.DecreaseNumber(toolType);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Shovel"))
        {
            isCollidingWithShovel = false;
        }
        else if (collision.gameObject.CompareTag("SeedBag"))
        {
            isCollidingWithSeedBag = false;
        }
    }

    private void PlantFlower(GameObject flowerPrefab)
    {
        //Giữ lại prefab
        currentFlowerPrefab = flowerPrefab;

        Vector3 targetPosition = transform.position;
        targetPosition.y += yDistance;

        AudioManager.Instance.PlaySPF(AudioManager.Instance.rustling);  

        GameObject flowerSpawned = Instantiate(flowerPrefab, targetPosition, Quaternion.identity);

        currentFlower = flowerSpawned;

        BaseFlower baseFlowerSpawned = flowerSpawned.GetComponent<BaseFlower>();
        currentFlowerID = Guid.NewGuid().ToString();
        baseFlowerSpawned.SetId(currentFlowerID);

        //Gán index bắt đầu là 0 cho cây được gieo trồng
        baseFlowerSpawned.Init();

        if (GameManager.Instance.IsFirstTimePlayer() && isFirstTimePlanting)
        {
            TutorialManager.Instance.hasSownTheSeed = true;
            isFirstTimePlanting = false;
        }
    }

    public void LoadData(GameData data)
    {
        data.gardenPlantSpots.TryGetValue(id, out PlantSpotSateData plantSpotStateData);
        
        if (plantSpotStateData != null)
        {
            switch (plantSpotStateData.state)
            {
                case PlantSpotState.BlankSpot:
                    SetState(PlantSpotState.BlankSpot);
                    break;
                case PlantSpotState.Hole:
                    SetState(PlantSpotState.Hole);
                    break;
                case PlantSpotState.SoilMoundlet:
                    SetState(PlantSpotState.SoilMoundlet);
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(plantSpotStateData.flowerID))
            {
                currentFlowerPrefab = Resources.Load<GameObject>(plantSpotStateData.flowerPrefabPath);

                Vector3 targetPosition = transform.position;
                targetPosition.y += yDistance;
                Debug.Log(currentFlowerPrefab);

                GameObject flowerSpawned = Instantiate(currentFlowerPrefab, targetPosition, Quaternion.identity);
                currentFlower = flowerSpawned;

                BaseFlower baseFlower = flowerSpawned.GetComponent<BaseFlower>();
                currentFlowerID = plantSpotStateData.flowerID;
                baseFlower.SetId(currentFlowerID);


                baseFlower.LoadData(data);
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        //Lấy flowerId với savedata của flower nếu plantSpot hiện tại đang trồng hoa
        if (currentFlower != null)
        {
            BaseFlower baseFlower = currentFlower.GetComponent<BaseFlower>();
            baseFlower.SaveData(ref data);
            currentFlowerID = baseFlower.GetId();

        }
        else
        {
            currentFlowerID = "";
        }

        var currentStateData = new PlantSpotSateData
        {
            flowerPrefabPath = GetPrefabPath(currentFlowerPrefab),
            state = currentState,
            flowerID = this.currentFlowerID
        };

        data.gardenPlantSpots[id] = currentStateData;

        Debug.Log(currentFlowerPrefab);
        Debug.Log("Save PlantSpotData!");
    }

    private string GetPrefabPath(GameObject prefab)
    {
        // This function should return the path of the prefab as stored in the Resources folder.
        // Example: "Prefabs/Flowers/Tulip" for a prefab located at Resources/Prefabs/Flowers/Tulip.prefab
        return prefab != null ? "Prefabs/Flowers/" + prefab.name : string.Empty;
    }
}

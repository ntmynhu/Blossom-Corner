using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseFlower : MonoBehaviour
{
    [Serializable]
    public struct FlowerData
    {
        public GameObject stateVisual;
        public float stateTime;
        public Transform indicatorPos;
    }

    [SerializeField] private List<FlowerData> flowerDataList;
    [SerializeField] private GameObject dropletIndicator;
    [SerializeField] private GameObject fertilizerIndicator;
    [SerializeField] private List<GameObject> grassObjectsList;
    [SerializeField] private FlowerSO itemSO;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Transform timerPos;
    [SerializeField] private GameObject timerPanel;
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Transform floatingTextPanel;

    private GameObject currentVisual;
    private bool isCollidingWithWateringCan = false;
    private bool isCollidingWithFertilizer = false;
    private bool isCollidingWithGloves = false;
    private float wateringTimeMax = 1f;
    private float wateringTime;
    private int stateCount;
    private int currentStateIndex;
    private float currentGrowthTime;
    private float currentGrowthTimeMax;
    private float indicatorWaitingTime;
    private float indicatorWaitingTimeMax;
    private float grassSpawnedTimeMax;
    private float grassSpawnedTime;
    private int grassAmount;
    private int grassAmountMax = 4;
    private bool isGrassPresent = false;
    private bool isWatering = false;
    private bool isFinish = false;

    private float grassDamageAverageTimeMax = 8f;
    private float grassDamageAverageTime;

    private int currentFertilizerNumber;
    private float remainingTime;
    private float totalGrowingTime;

    float increasingTimePerGrass = 1f;

    private List<FlowerSO> itemSOList = new List<FlowerSO>();

    private Camera mainCamera;

    private string flowerID;
    private bool isCollected = false;

    private void Start()
    {
        stateCount = flowerDataList.Count;
        wateringTime = wateringTimeMax;

        grassSpawnedTimeMax = GetRandomGrassSpawnedTime();
        grassSpawnedTime = grassSpawnedTimeMax;

        //Lấy ngẫu nhiên thời gian hiện yêu cầu
        indicatorWaitingTimeMax = Random.Range(5f, 20f);
        indicatorWaitingTime = indicatorWaitingTimeMax;

        /*//Xác định vị trí của timer
        mainCamera = Camera.main;
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(timerPos.position);
        Debug.Log("Screen Position timer: " + screenPosition);
        timerPanel.transform.position = screenPosition;*/
        mainCamera = Camera.main;
        timerPanel.SetActive(false);


        // Bắt event scenechanged để ẩn những ui hiện trong garden
        MainMenuManager.Instance.OnSceneChanged += Menu_OnSceneChanged;

    }

    public void Init()
    {
        currentStateIndex = 0;

        SetState(currentStateIndex);
        currentVisual = flowerDataList[0].stateVisual;
        currentGrowthTimeMax = flowerDataList[0].stateTime;
        currentGrowthTime = 0;


        //Lay thoi gian lớn của cây
        totalGrowingTime = GetTotalGrowingTime();
    }

    private void Menu_OnSceneChanged(object sender, EventArgs e)
    {
        if (MainMenuManager.Instance.GetCurrentScene() != Scene.GardenScene)
        {
            if (timerPanel != null)
            {
                timerPanel.gameObject.SetActive(false);
            }

            if (floatingTextPanel != null)
            {
                floatingTextPanel.gameObject.SetActive(false);
            }
        }
        else
        {
            if (floatingTextPanel != null)
            {
                floatingTextPanel.gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
        //Lấy thời gian set timer
        remainingTime = totalGrowingTime - GetCurrentlGrowingTimeFromStart();

        /*Debug.Log("Remaining time: " + remainingTime);
        Debug.Log("Total time: " + totalGrowingTime);
        Debug.Log("Currentgrowingtimefromstart time: " + GetCurrentlGrowingTimeFromStart());
        Debug.Log("Currentgrow time: " + currentGrowthTime);*/

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (!isFinish)
        {
            currentGrowthTime += Time.deltaTime;
            HandleState();
            HandleGrass();
            HandleIndicator();
            HandleWatering();
            HandleFertilizing();
            HandleSpawningGrass();
        }
        else
        {
            Destroy(timerPanel);
            if (GameManager.Instance.IsFirstTimePlayer())
            {
                if (TutorialManager.Instance.GetCurrentStepIndex() == 8 && TutorialManager.Instance.hasCutTheGrass)
                {
                    //step8SO.RaiseEvent();
                    TutorialManager.Instance.OnActionCompleted(3f);
                }
            }
            HandleCollecting();
            dropletIndicator.SetActive(false);
            fertilizerIndicator.SetActive(false);
        }

        if (grassAmount > 0)
        {
            isGrassPresent = true;
        }
        else
        {
            isGrassPresent = false;
        }

    }

    private float GetTotalGrowingTime()
    {
        float totalGrowingTime = 0;
        foreach (var data in flowerDataList)
        {
            totalGrowingTime += data.stateTime;
        }
        return totalGrowingTime;
    }

    private float GetCurrentlGrowingTimeFromStart()
    {
        float currentGrowingTimeFromStart = 0;

        for (int i = 0; i < currentStateIndex; i++)
        {
            currentGrowingTimeFromStart += flowerDataList[i].stateTime;
        }

        return currentGrowingTimeFromStart + (currentGrowthTime);
    }

    private void HandleState()
    {
        if (currentGrowthTime >= currentGrowthTimeMax || remainingTime <= 0f)
        {
            if (currentStateIndex < stateCount - 1)
            {
                currentStateIndex++;
            }

            SetState(currentStateIndex);
            if (dropletIndicator.activeSelf || fertilizerIndicator.activeSelf)
            {
                dropletIndicator.transform.position = flowerDataList[currentStateIndex].indicatorPos.position;
                fertilizerIndicator.transform.position = flowerDataList[currentStateIndex].indicatorPos.position;
            }

            currentGrowthTimeMax = flowerDataList[currentStateIndex].stateTime;
            currentGrowthTime = 0;

            if (currentStateIndex == flowerDataList.Count - 1)
            {
                isFinish = true;
            }
        }
    }
    private void SetState(int stateIndex)
    {
        if (currentVisual != null)
        {
            currentVisual.SetActive(false);
        }

        Debug.Log(stateIndex);
        flowerDataList[stateIndex].stateVisual.SetActive(true);
        currentVisual = flowerDataList[stateIndex].stateVisual;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WateringCan"))
        {
            isCollidingWithWateringCan = true;
        }
        else if (collision.gameObject.CompareTag("Fertilizer"))
        {
            isCollidingWithFertilizer = true;

            currentFertilizerNumber = FertilizerButton.Instance.GetCurrentNumber();
        }
        else if (collision.gameObject.CompareTag("Gloves"))
        {
            isCollidingWithGloves = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WateringCan"))
        {
            isCollidingWithWateringCan = false;
        }
        else if (collision.gameObject.CompareTag("Fertilizer"))
        {
            isCollidingWithFertilizer = false;
        }
        else if (collision.gameObject.CompareTag("Gloves"))
        {
            isCollidingWithGloves = false;
        }
    }

    private void HandleGrass()
    {
        if (isGrassPresent)
        {
            grassDamageAverageTime += Time.deltaTime;
            if (grassDamageAverageTime >= grassDamageAverageTimeMax)
            {
                currentGrowthTime -= increasingTimePerGrass * grassAmount;
                ShowFloatingText(-1 * increasingTimePerGrass * grassAmount);
                grassDamageAverageTime = 0;
            }
        }
    }

    private void WateringThePlant()
    {
        wateringTime -= Time.deltaTime;
        if (wateringTime < 0f)
        {
            dropletIndicator.SetActive(false);
            float decreasePercentage = Random.Range(.01f, .1f);
            currentGrowthTime += totalGrowingTime * decreasePercentage;

            ShowFloatingText(totalGrowingTime * decreasePercentage);
            wateringTime = wateringTimeMax;
        }
    }

    private void FertilizingThePlant()
    {
        float decreasePercentage = Random.Range(.05f, .2f);
        currentGrowthTime += totalGrowingTime * decreasePercentage;

        ShowFloatingText(totalGrowingTime * decreasePercentage);

        fertilizerIndicator.SetActive(false);
    }

    private void ActivateRandomGrass()
    {
        int index = Random.Range(0, grassObjectsList.Count - 1);
        if (!grassObjectsList[index].activeSelf)
        {
            grassObjectsList[index].SetActive(true);
            grassAmount++;
        }
        else
        {
            ActivateRandomGrass();
        }
    }

    private float GetRandomGrassSpawnedTime()
    {
        return Random.Range(20f, 30f);
    }

    private void HandleIndicator()
    {
        if (!isFinish && !dropletIndicator.activeSelf && !fertilizerIndicator.activeSelf)
        {
            GameObject indicatorChosen;
            int choice = Random.Range(1, 5);
            if (choice == 1)
            {
                indicatorChosen = fertilizerIndicator;
            }
            else
            {
                indicatorChosen = dropletIndicator;
            }

            indicatorWaitingTime -= Time.deltaTime;
            if (indicatorWaitingTime < 0)
            {
                if (GameManager.Instance.IsFirstTimePlayer())
                {
                    if (!TutorialManager.Instance.hasWaterAppeared)
                    {
                        indicatorChosen = dropletIndicator;
                        TutorialManager.Instance.hasWaterAppeared = true;

                        if (TutorialManager.Instance.GetCurrentStepIndex() == 5 /*Gieo hạt*/)
                        {
                            TutorialManager.Instance.OnActionCompleted(2f);
                        }
                    }
                }    

                indicatorChosen.transform.position = flowerDataList[currentStateIndex].indicatorPos.position;
                indicatorChosen.SetActive(true);
                indicatorWaitingTimeMax = Random.Range(5f, 10f);
                indicatorWaitingTime = indicatorWaitingTimeMax;
            }
        }
    }
    private IEnumerator WaitForSeconds(float time)
    {
        yield return new WaitForSeconds(time);
    }

    private void HandleWatering()
    {
        if (dropletIndicator.activeSelf)
        {
            if (ToolManager.Instance.IsHoldingTool() && isCollidingWithWateringCan && Input.GetMouseButton(0))
            {
                isWatering = true;
                if (!isGrassPresent)
                {
                    WateringThePlant();
                }
            }
            else
            {
                isWatering = false;
            }
        }
        else
        {
            isWatering = false;
        }
    }

    private void HandleFertilizing()
    {
        if (fertilizerIndicator.activeSelf)
        {
            if (ToolManager.Instance.IsHoldingTool() && isCollidingWithFertilizer && Input.GetMouseButton(0) && currentStateIndex < stateCount)
            {
                if (currentFertilizerNumber > 0)
                {
                    AudioManager.Instance.PlaySPF(AudioManager.Instance.rustling);

                    FertilizingThePlant();
                    FertilizerButton.Instance.DecreaseNumber();
                }
            }
        }
    }

    private void HandleSpawningGrass()
    {
        if (gameObject.activeSelf)
        {
            grassSpawnedTime -= Time.deltaTime;
            if (grassSpawnedTime < 0)
            {
                if (grassAmount < grassAmountMax)
                {
                    ActivateRandomGrass();
                    grassSpawnedTimeMax = GetRandomGrassSpawnedTime();
                    grassSpawnedTime = grassSpawnedTimeMax;

                    if (GameManager.Instance.IsFirstTimePlayer())
                    {
                        if (TutorialManager.Instance.GetCurrentStepIndex() >= 5) //After gieo hạt
                        {
                            if (!TutorialManager.Instance.hasGrassAppeared)
                            {
                                //step8SO.RaiseEvent();
                                TutorialManager.Instance.hasGrassAppeared = true;
                            }

                            if (TutorialManager.Instance.GetCurrentStepIndex() == 6 && TutorialManager.Instance.hasGrassAppeared)
                            {
                                TutorialManager.Instance.OnActionCompleted(3f);
                            }
                        }
                    }
                }
            }
        }
    }

    private void HandleCollecting()
    {
        if (currentStateIndex == stateCount - 1 && ToolManager.Instance.IsHoldingTool() && isCollidingWithGloves && Input.GetMouseButton(0))
        {
            if (!AudioManager.Instance.GetSPXSource().isPlaying)
            {
                AudioManager.Instance.PlaySPF(AudioManager.Instance.rustling);
            }

            itemSOList = InventoryManager.Instance.GetItemSOList();
            bool isItemFound = false;
            foreach (var item in itemSOList)
            {
                if (itemSO == item)
                {
                    isItemFound = true;
                    InventoryManager.Instance.IncreaseNumber(itemSO);
                    break;
                }
            }

            if (!isItemFound)
            {
                InventoryManager.Instance.Add(itemSO);
                InventoryManager.Instance.IncreaseNumber(itemSO);
            }

            isCollected = true;

            if (GameManager.Instance.IsFirstTimePlayer())
            {
                if (TutorialManager.Instance.GetCurrentStepIndex() == 9)
                {
                    //step10SO.RaiseEvent();
                    TutorialManager.Instance.OnActionCompleted(3f);
                }
            }

            Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        if (timerPanel != null)
        {
            //Xác định vị trí của timer
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(timerPos.position);
            Debug.Log("Screen Position timer: " + screenPosition);
            timerPanel.transform.position = screenPosition;

            timerPanel.SetActive(!timerPanel.activeSelf);
        }
    }

    /// <summary>
    /// Giving negative parameter for spawing "-" and vice versa
    /// </summary>
    /// <param name="number"></param>
    private void ShowFloatingText(float number)
    {
        if (MainMenuManager.Instance.GetCurrentScene() == Scene.GardenScene)
        {
            int numberInt = Mathf.FloorToInt(number);

            Debug.Log(transform.position);
            Debug.Log(gameObject);
            Debug.Log(floatingTextPanel);
            Debug.Log(mainCamera);
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(gameObject.transform.position);

            // Set the position of the floatingTextPanel to the screen position of the gameObject position
            floatingTextPanel.transform.position = screenPosition;
            Debug.Log("Screen Position: " + screenPosition);

            // Instantiate the floating text prefab as a child of the floatingTextPanel
            var floatingTextSpawned = Instantiate(floatingTextPrefab, floatingTextPanel.transform);

            // Get the TextMeshProUGUI component from the instantiated prefab
            TextMeshProUGUI textMesh = floatingTextSpawned.GetComponent<TextMeshProUGUI>();

            // Set the text content based on the number
            if (numberInt < 0)
            {
                textMesh.text = numberInt.ToString();
                Color lightRedColor = new Color(1f, 179f / 255f, 171f / 255f, 1f);
                textMesh.color = lightRedColor;
            }
            else
            {
                textMesh.text = "+" + numberInt.ToString();
                Color lightGreenColor = new Color(187f / 255f, 1f, 171f / 255f, 1f);
                textMesh.color = lightGreenColor;
            }
        }
    }

    public bool IsWatering()
    {
        return isWatering;
    }

    public void DecreaseGrassAount()
    {
        grassAmount--;

        if (GameManager.Instance.IsFirstTimePlayer())
        {
            if (TutorialManager.Instance.GetCurrentStepIndex() == 8)
            {
                //step9SO.RaiseEvent();
                TutorialManager.Instance.hasCutTheGrass = true;
            }
        }
    }

    private void ActivateGrassAccordingToAmount(int amount)
    {
        grassAmount = 0;
        for (int i = 0; i < amount; i++)
        {
            ActivateRandomGrass();
        }
    }

    public string GetId()
    {
        return flowerID;
    }

    public void SetId(string id)
    {
        flowerID = id;
    }

    public void LoadData(GameData data)
    {
        if (data.flowerStates.TryGetValue(flowerID, out var flowerState))
        {
            currentStateIndex = flowerState.currentStateIndex;
            currentGrowthTime = flowerState.currentGrowthTime;
            transform.position = flowerState.position;
            grassAmount = flowerState.grassAmount;
            totalGrowingTime = flowerState.totalGrowingTime;

            currentGrowthTimeMax = flowerDataList[currentStateIndex].stateTime;

            // Restore the current visual state
            SetState(currentStateIndex);

            //Avtivate the grass
            Debug.Log("Grass amount: " + grassAmount);
            ActivateGrassAccordingToAmount(grassAmount);
        }
    }

    public void SaveData(ref GameData data)
    {
        var flowerState = new FlowerState
        {
            flowerID = flowerID,
            currentStateIndex = currentStateIndex,
            currentGrowthTime = currentGrowthTime,
            position = transform.position,
            grassAmount = grassAmount,
            totalGrowingTime = totalGrowingTime,
        };

        Debug.Log("Grass amount " + grassAmount);
        // Update existing flower state or add new one
        data.flowerStates[flowerID] = flowerState;

        Debug.Log("Save Flower");
    }

}

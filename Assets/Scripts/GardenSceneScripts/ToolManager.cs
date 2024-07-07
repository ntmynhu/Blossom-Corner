using UnityEngine;

public enum ToolType
{
    None,
    Shovel,
    WateringCan,
    Scissors,
    Fertilizer,
    Gloves,
    TulipSeedBag,
    SunflowerSeedBag,
    DaisySeedBag,
    HydrangeaSeedBag,
    SeedBag,
    BasicPot,
}

public class ToolManager : MonoBehaviour
{
    #region Singleton
    private static ToolManager instance;
    public static ToolManager Instance => instance;

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

    [SerializeField] private GameObject shovel;
    [SerializeField] private GameObject tulipSeedBag;
    [SerializeField] private GameObject sunflowerSeedBag;
    [SerializeField] private GameObject daisySeedBag;
    [SerializeField] private GameObject hydrangeaSeedBag;
    [SerializeField] private GameObject wateringCan;
    [SerializeField] private GameObject scissors;
    [SerializeField] private GameObject fertilizer;
    [SerializeField] private GameObject gloves;
    [SerializeField] private GameObject seedBag;

    private ToolType currentToolType = ToolType.None;
    private GameObject currentTool;
    private Vector3 currentInitialPos;

    private bool isHoldingTool = false;

    private void Start()
    {
        MainMenuManager.Instance.OnSceneChanged += MainMenu_OnSceneChanged;
    }

    private void MainMenu_OnSceneChanged(object sender, System.EventArgs e)
    {
        ChangeTool(ToolType.None);
    }

    public bool IsHoldingTool()
    {
        return isHoldingTool;
    }

    public void SelectTool(ToolType toolType)
    {
        if (toolType == currentToolType)
        {
            ToggleTool();
        }
        else
        {
            ChangeTool(toolType);
            ToggleTool();
        }

        isHoldingTool = currentTool != null;
    }

    public void DeSelectTool()
    {
        if (currentTool != null)
        {          
            currentTool.transform.position = currentInitialPos;
            currentTool.SetActive(false);
        }

        currentToolType = ToolType.None;
        isHoldingTool = false;
    }

    public ToolType GetCurrentToolType()
    {
        return currentToolType;
    }

    private void ChangeTool(ToolType toolType)
    {
        currentToolType = toolType;
        if (currentTool !=  null)
        {
            currentTool.SetActive(false);
        }

        switch(toolType)
        {
            case ToolType.Shovel:
                currentTool = shovel;
                break;
            case ToolType.TulipSeedBag:
                currentTool = tulipSeedBag;
                break;
            case ToolType.SunflowerSeedBag:
                currentTool = sunflowerSeedBag;
                break;
            case ToolType.DaisySeedBag:
                currentTool = daisySeedBag;
                break;
            case ToolType.HydrangeaSeedBag:
                currentTool = hydrangeaSeedBag;
                break;
            case ToolType.WateringCan:
                currentTool = wateringCan;
                break;
            case ToolType.Scissors:
                currentTool = scissors;
                break;
            case ToolType.Fertilizer:
                currentTool = fertilizer;
                break;
            case ToolType.Gloves:
                currentTool = gloves;
                break;
            case ToolType.SeedBag:
                currentTool = seedBag;
                break;
        }

    }

    private void ToggleTool()
    {
        currentTool.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentInitialPos = currentTool.transform.position;

        if (currentTool != null)
        {
            currentTool.SetActive(!currentTool.activeSelf);
            isHoldingTool = currentTool.activeSelf;
        }
    }

    public ToolType GetToolType()
    {
        return currentToolType;
    }
   
}

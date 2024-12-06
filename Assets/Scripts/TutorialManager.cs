using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Serializable]
    private struct TutorialStep
    {
        public GameObject tutorialPanel;
        public Button confirmButton;
    }

    [SerializeField] private List<TutorialStep> tutorialStepList = new List<TutorialStep>();
    [SerializeField] private GameObject circleIndicator;

    [SerializeField] private GameObject toolPanel;
    [SerializeField] private Transform mapButton;
    [SerializeField] private Transform gardenButton;
    [SerializeField] private Transform potIcon;
    [SerializeField] private Transform basicPotIcon;
    [SerializeField] private Transform basicPotIconInShop;
    [SerializeField] private Transform shovelIcon;
    [SerializeField] private Transform fertilizerIcon;
    [SerializeField] private Transform seedIcon;
    [SerializeField] private Transform waterIcon;
    [SerializeField] private Transform harvestIcon;
    [SerializeField] private Transform scissorsIcon;
    [SerializeField] private Transform storageIcon;

    private int currentStepIndex;
    private TutorialStep currentStep;
    private bool isFinished = false;

    public bool hasWatered = false;
    public bool hasFertilized = false;
    public bool hasCutTheGrass = false;
    public bool hasSownTheSeed = false;
    public bool hasGrassAppeared = false;
    public bool hasFertilizerAppeared = false;
    public bool hasWaterAppeared = false;

    private GameObject indicator;
    private List<TutorialButton> tutorialButtonList = new List<TutorialButton>();

    #region Singleton
    private static TutorialManager instance;
    public static TutorialManager Instance => instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

    }
    #endregion

    public void StartTutorial()
    {
        foreach (var step in tutorialStepList)
        {
            step.tutorialPanel.SetActive(false);
            step.confirmButton?.onClick.AddListener(() => OnConfirmClicked());
        }

        currentStepIndex = 0;
        currentStep = tutorialStepList[currentStepIndex];
        ShowCurrentStep();
    }

    private void ShowCurrentStep()
    {
        Debug.Log(currentStep.tutorialPanel);
        currentStep.tutorialPanel.SetActive(true);
        Debug.Log(currentStep.tutorialPanel.activeSelf);
    }

    private void NextStep()
    {
        Debug.Log("NextStep");
        // Hide the current panel
        currentStep.tutorialPanel.SetActive(false);

        if (currentStepIndex < tutorialStepList.Count)
        {
            Debug.Log("Show currentStep");
            currentStep = tutorialStepList[currentStepIndex];
            ShowCurrentStep();
        }
        else
        {
            EndTutorial();
        }
    }

    private void OnConfirmClicked()
    {
        currentStep.tutorialPanel.SetActive(false);
        ShowScreenIndicator();
    }

    public void OnActionCompleted(float time)
    {
        Debug.Log("Action complete");
        indicator?.SetActive(false);
        StartCoroutine(WaitForSecondsAndNextStep(time));
    }

    public void EndTutorial()
    {
        isFinished = true;
        Debug.Log("Tutorial Finished");
    }

    public bool IsFinished()
    {
        return isFinished;
    }

    public int GetCurrentStepIndex()
    {
        return currentStepIndex;
    }

    private IEnumerator WaitForSecondsAndNextStep(float time)
    {
        currentStepIndex++;

        yield return new WaitForSeconds(time);

        NextStep();
    }

    private void ShowScreenIndicator()
    {
        switch (currentStepIndex)
        {
            case 1:
                indicator = circleIndicator;

                AddButtonIndicator(mapButton);
                AddButtonIndicator(gardenButton);
                break;
            case 3:
                indicator = circleIndicator;

                AddButtonIndicator(potIcon);
                AddButtonIndicator(basicPotIcon);
                AddButtonIndicator(basicPotIconInShop);
                break;
            case 5:
                if (!toolPanel.activeSelf)
                {
                    toolPanel.SetActive(true);
                }
                indicator = circleIndicator;
                AddButtonIndicator(shovelIcon);
                break;
            case 6:
                indicator = circleIndicator;
                AddButtonIndicator(seedIcon);
                break;
            case 7:
                if (!toolPanel.activeSelf)
                {
                    toolPanel.SetActive(true);
                }
                indicator = circleIndicator;
                AddButtonIndicator(waterIcon);
                break;
            case 8:
                indicator = circleIndicator;
                AddButtonIndicator(fertilizerIcon);
                break;
            case 9:
                indicator = circleIndicator;
                AddButtonIndicator(scissorsIcon);
                break;
            case 10:
                indicator = circleIndicator;
                AddButtonIndicator(harvestIcon);
                break;
            case 11:
                indicator = circleIndicator;
                AddButtonIndicator(storageIcon);
                break;
            default:
                break;
        }
    }

    private void AddButtonIndicator(Transform transform)
    {
        indicator.SetActive(true);
        var indicatorSpawned = Instantiate(indicator, transform);
        var tutorialButton = transform.gameObject.AddComponent<TutorialButton>();
        tutorialButton.SetUp(indicatorSpawned);
        tutorialButtonList.Add(tutorialButton);
    }
}

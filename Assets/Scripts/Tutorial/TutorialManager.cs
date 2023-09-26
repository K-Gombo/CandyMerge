using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum TutorialState
{
    None,
    TapGiftBox,
    MergeBasicCandy,
    TapGiftBoxAgain,
    ClaimQuestReward
}

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    public TutorialState currentState;
    public GameObject tutorialMerge;
    public GameObject tutorialCreate;
    public GameObject tutorialQuest;
    public bool isTutorialActive;
    
    
    
    public int tutorialCount
    {
        get { return PlayerPrefs.GetInt("TutoCheck_" + Application.productName, 0); }
        set { PlayerPrefs.SetInt("TutoCheck_"+ Application.productName, value); }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
        }
        else
        {
            Destroy(gameObject);
        }
        
        

        currentState = TutorialState.None;

        if (tutorialCount == 0)
        {
            isTutorialActive = true;
            currentState = TutorialState.TapGiftBox;
            tutorialCreate.SetActive(true);
        }
        else
        {
            isTutorialActive = false;
            currentState = TutorialState.None;
        }
    }

    public void NextTutorialStep()
    {
        if (!isTutorialActive) return; 
        switch (currentState)
        {
            case TutorialState.TapGiftBox:
                currentState = TutorialState.MergeBasicCandy;
                tutorialCreate.SetActive(false);
                tutorialMerge.SetActive(true);
                break;

            case TutorialState.MergeBasicCandy:
                currentState = TutorialState.TapGiftBoxAgain;
                tutorialMerge.SetActive(false);
                tutorialCreate.SetActive(true);
                break;

            case TutorialState.TapGiftBoxAgain:
                currentState = TutorialState.ClaimQuestReward;
                tutorialCreate.SetActive(false);
                tutorialQuest.SetActive(true);
                break;

            case TutorialState.ClaimQuestReward:
                tutorialQuest.SetActive(false);
                tutorialCount++;
                isTutorialActive = false;
                break;
        }
    }
}
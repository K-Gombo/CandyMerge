using System;
using UnityEngine;
using UnityEngine.UI;
using ES3Types;

public class AutoCreateBtn : MonoBehaviour
{
    public Text durationText;
    public GameObject adChoicePanel; // Drag your Ad Choice Panel here in Unity Editor
    public Button onButton;
    public Button offButton;

    private bool isAutoMergeOn;
    private float remainingDuration;
    private float lastUpdateTime;


    void Start()
    {
        LoadState();
        lastUpdateTime = Time.time;
        UpdateDurationText();
    }

    void Update()
    {
        if (isAutoMergeOn && remainingDuration > 0)
        {
            float deltaTime = Time.time - lastUpdateTime;
            remainingDuration -= deltaTime;

            if (remainingDuration <= 0)
            {
                isAutoMergeOn = false;
                Debug.Log("AutoMerge turned OFF due to no remaining duration");
            }

            UpdateDurationText();
        }

        lastUpdateTime = Time.time;
    }

    private void UpdateDurationText()
    {
        int totalSeconds = Mathf.FloorToInt(remainingDuration);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        durationText.text = $"{minutes}:{seconds:D2}";
    }

    public void ToggleAutoMerge()
    {
        if (isAutoMergeOn)
        {
            onButton.gameObject.SetActive(!isAutoMergeOn);
            offButton.gameObject.SetActive(isAutoMergeOn);
            isAutoMergeOn = false;
            Debug.Log("AutoMerge is OFF");
        }
        else
        {
            if (remainingDuration > 0)
            {
                onButton.gameObject.SetActive(!isAutoMergeOn);
                offButton.gameObject.SetActive(isAutoMergeOn);
                isAutoMergeOn = true;
                Debug.Log("AutoMerge is ON");
            }
            else
            {
                // Show the Ad Choice Panel
                adChoicePanel.SetActive(true);
            }
        }

        SaveState();
    }

    public void ToggleAutoMerge(bool isOn)
    {
        onButton.gameObject.SetActive(isOn);
        offButton.gameObject.SetActive(!isOn);
        isAutoMergeOn = isOn;
    }

    public void WatchAd()
    {
        // This will actually display the ad, for demonstration we just add time
        remainingDuration += 600; // Add 10 minutes in seconds
        Debug.Log("Gained 10 minutes of AutoMerge duration");

        // Hide the Ad Choice Panel
        adChoicePanel.SetActive(false);


        SaveState();
    }

    public void ShowAds()
    {
        AdsManager.instance.ShowRewarded(RewardType.AutoCreate);
    }

    public void CancelAd()
    {
        // Hide the Ad Choice Panel
        adChoicePanel.SetActive(false);
    }

    public void SaveState()
    {
        //ES3.Save<bool>("IsAutoMergeOn", isAutoMergeOn);
        ES3.Save<float>("RemainingDuration", remainingDuration);
    }

    public void LoadState()
    {
        //if (ES3.KeyExists("IsAutoMergeOn"))
        //    isAutoMergeOn = ES3.Load<bool>("IsAutoMergeOn");
        //else
        //    isAutoMergeOn = false;

        if (ES3.KeyExists("RemainingDuration"))
            remainingDuration = ES3.Load<float>("RemainingDuration");
        else
            remainingDuration = 0;
    }
}

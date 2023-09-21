using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OptionToggle : MonoBehaviour
{
    [SerializeField] GameObject onButton;
    [SerializeField] GameObject offButton;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Toggle>().onValueChanged.AddListener(SwitchToggle);
    }

    private void SwitchToggle(bool isSwitch)
    {
        if (isSwitch)
        {
            onButton.SetActive(true);
            offButton.SetActive(false);

            string parentName = transform.parent.name;
            Debug.Log("이름이 뭐니 " + parentName);
            if (parentName == "BGM")
            {
                SoundManager.Instance.SetBGMMasterVolume(1);
            }
            else if (parentName == "Sound")
            {        
                SoundManager.Instance.SetSoundMasterVolume(1);
            }
        }
        else
        {
            onButton.SetActive(false);
            offButton.SetActive(true);


            string parentName = transform.parent.name;

            if (parentName == "BGM")
            {
                SoundManager.Instance.SetBGMMasterVolume(0);
            }
            else if (parentName == "Sound")
            {
                SoundManager.Instance.SetSoundMasterVolume(0);
            }
        }
    }

}

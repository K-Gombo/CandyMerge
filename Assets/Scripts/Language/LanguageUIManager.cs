using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageUIManager : MonoBehaviour
{
    [Header("[ UI 언어 번역 ]")]
    public Text[] UILanguageList;

    public enum UILanguageIndex
    {
        AutoModeBtn,
        AutoMixBtn,
        Gacha,
        
        
    }

    private Dictionary<string, Text> UILanguageDictionary;

    public enum Language
    {
        Korean,
        English
    }

    private Language currentLanguage = Language.English; // 기본설정 영어

    void Start()
    {
        // 초기 언어 설정
        ChangeLanguage(currentLanguage);
    }

    public void ChangeLanguage(Language newLanguage)
    {
        currentLanguage = newLanguage;
        UpdateUI();
    }

    private void UpdateUI()
    {
        // 언어에 따라 UI 텍스트를 업데이트 하기
        switch (currentLanguage)
        {
            case Language.Korean:
                UILanguageList[(int)UILanguageIndex.AutoModeBtn].text = "자동 제작";
                UILanguageList[(int)UILanguageIndex.AutoMixBtn].text = "자동 조합";
                UILanguageList[(int)UILanguageIndex.AutoMixBtn].text = "뽑기";
                
                break;

            case Language.English:
                UILanguageList[(int)UILanguageIndex.AutoModeBtn].text = "Auto";
                UILanguageList[(int)UILanguageIndex.AutoMixBtn].text = "Auto Mix";
                UILanguageList[(int)UILanguageIndex.AutoMixBtn].text = "Gacha";
                
                break;
        }
    }
    
    
    
    
    
    public void OnClickChangeToEnglish()
    {
        ChangeLanguage(Language.English);
    }
    
    
    public void OnClickChangeToKorean()
    {
        ChangeLanguage(Language.Korean);
    }
}
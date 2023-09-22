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
        CandyLevelBtn,
        AraangeBtn,
        LuckyCreateTitle,
        LuckyCreateExplain,
        CreateSpeedTitle,
        CreateSpeedExplain,
        HaveCandyTitle,
        HaveCandyExplain,
        MaxCandyTitle,
        MaxCandyExplain,
        CandyLevelSkillTitle,
        CandyLevelSkillExplain,
        PassiveCreateTitle,
        PassiveCreateExplain,
        GoldUpSkillTitle,
        GoldUpExplain,
        LuckyGoldTitle,
        LuckyGoldExplain,
        OfflineRewardSkillTitle,
        OfflineRewardSkillExplain,
        equipStatusSkillTitle,
        EquipSet,
        EquipUpgrade,
        DescriptionTitle,
        ShowTitle,
        SSRankScore,
        SRankScore,
        ARankScore,
        BRankScore,
        CRankScore,
        DRankScore,
        FRankScore,
        SpecialPercentTitle,
        ScorePercent1,
        ScorePercent2,
        ScorePercent3,
        ScorePercent4,
        AutoMergeAdsExplain,
        EquipKeyExplain,
        EquipMixAll,
        EquipMixPanel,
        SlotArrangeBtn,
        RankArrangeBtn,
        EquipMix,
        MyEquip,
        MixExplain,
        MixEquipGold,
        EquipArrangeUpBtn,
        EquipArrangeDownBtn,
        SpecialBoxExplain,
        NormalBoxExplain,
        SpecialBoxTitle,
        NormalBoxTitle,
        MixSumLevel
        
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
                UILanguageList[(int)UILanguageIndex.Gacha].text = "뽑기";
                UILanguageList[(int)UILanguageIndex.CandyLevelBtn].text = "캔디 레벨";
                UILanguageList[(int)UILanguageIndex.AraangeBtn].text = "정렬";
                UILanguageList[(int)UILanguageIndex.LuckyCreateTitle].text = "제작 캔디 2배 확률(Max Lv.40)";
                UILanguageList[(int)UILanguageIndex.LuckyCreateExplain].text = "n% 확률로 한번에 2개의 캔디 제작";
                UILanguageList[(int)UILanguageIndex.CreateSpeedTitle].text = "빠른 제작(Max Lv.5)";
                UILanguageList[(int)UILanguageIndex.CreateSpeedExplain].text = "캔디 제작 시간 n초 감소";
                UILanguageList[(int)UILanguageIndex.HaveCandyTitle].text = "캔디 필드 확장(Max Lv.33)";
                UILanguageList[(int)UILanguageIndex.HaveCandyExplain].text = "필드에 보유 가능한 캔디의 최대 개수 n개 증가";
                UILanguageList[(int)UILanguageIndex.MaxCandyTitle].text = "캔디 생성 증가(Max Lv.30)";
                UILanguageList[(int)UILanguageIndex.MaxCandyExplain].text = "제작 가능한 캔디의 최대 개수 n개 증가";
                UILanguageList[(int)UILanguageIndex.CandyLevelSkillTitle].text = "제작 캔디 레벨 증가(Max Lv.57)";
                UILanguageList[(int)UILanguageIndex.CandyLevelSkillExplain].text = "제작된 캔디의 레벨 n 증가";
                UILanguageList[(int)UILanguageIndex.PassiveCreateTitle].text = "자동 제작 쿨타임 감소(Max Lv.100)";
                UILanguageList[(int)UILanguageIndex.PassiveCreateExplain].text = "(자동)10초에 n번 캔디를 제작";
                UILanguageList[(int)UILanguageIndex.GoldUpSkillTitle].text = "골드량 증가(Max Lv.100)";
                UILanguageList[(int)UILanguageIndex.GoldUpExplain].text = "골드 획득량 n% 증가";
                UILanguageList[(int)UILanguageIndex.LuckyGoldTitle].text = "골드 2배 획득(Max Lv.150)";
                UILanguageList[(int)UILanguageIndex.LuckyGoldExplain].text = "n% 확률로 2배의 골드를 획득";
                UILanguageList[(int)UILanguageIndex.OfflineRewardSkillTitle].text = "오프라인 보상 보너스(Max Lv.100)";
                UILanguageList[(int)UILanguageIndex.OfflineRewardSkillExplain].text = "n% 만큼 오프라인 보상을 추가로 획득";
                UILanguageList[(int)UILanguageIndex.equipStatusSkillTitle].text = "추가 스킬";
                UILanguageList[(int)UILanguageIndex.EquipSet].text = "장착 해제";
                UILanguageList[(int)UILanguageIndex.EquipUpgrade].text = "업그레이드";
                UILanguageList[(int)UILanguageIndex.DescriptionTitle].text = "장착된 장비의 등급에 따라 부여되는 장착 점수가 다릅니다. 점수의 총 합에 따라 특별 손님이 퀘스트를 들고 오는 확률이 달라집니다.";
                UILanguageList[(int)UILanguageIndex.ShowTitle].text = "장착 점수";
                UILanguageList[(int)UILanguageIndex.SSRankScore].text = "7점";
                UILanguageList[(int)UILanguageIndex.SRankScore].text = "6점";
                UILanguageList[(int)UILanguageIndex.ARankScore].text = "5점";
                UILanguageList[(int)UILanguageIndex.BRankScore].text = "4점";
                UILanguageList[(int)UILanguageIndex.CRankScore].text = "3점";
                UILanguageList[(int)UILanguageIndex.DRankScore].text = "2점";
                UILanguageList[(int)UILanguageIndex.FRankScore].text = "1점";
                UILanguageList[(int)UILanguageIndex.SpecialPercentTitle].text = "특별 손님 확률";
                UILanguageList[(int)UILanguageIndex.ScorePercent1].text = "1-7점";
                UILanguageList[(int)UILanguageIndex.ScorePercent2].text = "8-14점";
                UILanguageList[(int)UILanguageIndex.ScorePercent3].text = "15-21";
                UILanguageList[(int)UILanguageIndex.ScorePercent4].text = "22-28점";
                UILanguageList[(int)UILanguageIndex.AutoMergeAdsExplain].text = "광고를 시청하고 10분간 자동 머지를 실행합니다.";
                UILanguageList[(int)UILanguageIndex.EquipKeyExplain].text = "키를 사용해 장비 상자를 오픈하세요";
                UILanguageList[(int)UILanguageIndex.EquipMixAll].text = "일괄 합성";
                UILanguageList[(int)UILanguageIndex.EquipMixPanel].text = "합성";
                UILanguageList[(int)UILanguageIndex.SlotArrangeBtn].text = "슬롯별";
                UILanguageList[(int)UILanguageIndex.RankArrangeBtn].text = "등급별";
                UILanguageList[(int)UILanguageIndex.EquipMix].text = "합성";
                UILanguageList[(int)UILanguageIndex.MyEquip].text = "나의 장비";
                UILanguageList[(int)UILanguageIndex.MixExplain].text = "합성을 원하는 장비를 선택하세요 !";
                UILanguageList[(int)UILanguageIndex.MixEquipGold].text = "골드 획득량 1 % -> 1.5 %";
                UILanguageList[(int)UILanguageIndex.EquipArrangeUpBtn].text = "정렬";
                UILanguageList[(int)UILanguageIndex.EquipArrangeDownBtn].text = "정렬";
                UILanguageList[(int)UILanguageIndex.SpecialBoxExplain].text = "S ~ C 등급 장비 5개 소환";
                UILanguageList[(int)UILanguageIndex.NormalBoxExplain].text = "S ~ C 등급 장비 5개 소환";
                UILanguageList[(int)UILanguageIndex.SpecialBoxTitle].text = "특별 선물 상자";
                UILanguageList[(int)UILanguageIndex.NormalBoxTitle].text = "일반 선물 상자";
                UILanguageList[(int)UILanguageIndex.MixSumLevel].text = "믹스 레벨";
                
                
                break;

            case Language.English:
                UILanguageList[(int)UILanguageIndex.AutoModeBtn].text = "Auto";
                UILanguageList[(int)UILanguageIndex.AutoMixBtn].text = "Auto Mix";
                UILanguageList[(int)UILanguageIndex.Gacha].text = "Gacha";
                UILanguageList[(int)UILanguageIndex.CandyLevelBtn].text = "Level";
                UILanguageList[(int)UILanguageIndex.AraangeBtn].text = "Arrange";
                UILanguageList[(int)UILanguageIndex.LuckyCreateTitle].text = "Produce double candies(Max Lv.40)";
                UILanguageList[(int)UILanguageIndex.LuckyCreateExplain].text = "Produce two candies with a n% chance";
                UILanguageList[(int)UILanguageIndex.CreateSpeedTitle].text = "Quick production(Max Lv.5)";
                UILanguageList[(int)UILanguageIndex.CreateSpeedExplain].text = "Reduce candy production time by n seconds";
                UILanguageList[(int)UILanguageIndex.HaveCandyTitle].text = "Increase candies in the field(Max Lv.33)";
                UILanguageList[(int)UILanguageIndex.HaveCandyExplain].text = "Increase n numbers of candies field ";
                UILanguageList[(int)UILanguageIndex.MaxCandyTitle].text = "Production candy increase(Max Lv.30)";
                UILanguageList[(int)UILanguageIndex.MaxCandyExplain].text = "Increase n numbers of candies made ";
                UILanguageList[(int)UILanguageIndex.CandyLevelSkillTitle].text = "Candy level increase(Max Lv.57)";
                UILanguageList[(int)UILanguageIndex.CandyLevelSkillExplain].text = "Increase n levels of the candies made";
                UILanguageList[(int)UILanguageIndex.PassiveCreateTitle].text = "Automatic production(Max Lv.100)";
                UILanguageList[(int)UILanguageIndex.PassiveCreateExplain].text = "(Auto)Make n candies in 10 seconds";
                UILanguageList[(int)UILanguageIndex.GoldUpSkillTitle].text = "Increase in gold(Max Lv.100)";
                UILanguageList[(int)UILanguageIndex.GoldUpExplain].text = "Increase gold acquisition by n%";
                UILanguageList[(int)UILanguageIndex.LuckyGoldTitle].text = "Double gold(Max Lv.150)";
                UILanguageList[(int)UILanguageIndex.LuckyGoldExplain].text = "Double gold with n% chance";
                UILanguageList[(int)UILanguageIndex.OfflineRewardSkillTitle].text = "Offline reward bonus(Max Lv.100)";
                UILanguageList[(int)UILanguageIndex.OfflineRewardSkillExplain].text = "Gain n% additional offline rewards";
                UILanguageList[(int)UILanguageIndex.equipStatusSkillTitle].text = "Skill List";
                UILanguageList[(int)UILanguageIndex.EquipSet].text = "Unequipped";
                UILanguageList[(int)UILanguageIndex.EquipUpgrade].text = "UPGRADE";
                UILanguageList[(int)UILanguageIndex.DescriptionTitle].text = "The equipped equipment has different equipment scores depending on its grade. Depending on the total sum of the scores, the probability of a special customer bringing a quest changes.";
                UILanguageList[(int)UILanguageIndex.ShowTitle].text = "Equip Score";
                UILanguageList[(int)UILanguageIndex.SSRankScore].text = "Score 7";
                UILanguageList[(int)UILanguageIndex.SRankScore].text = "Score 6";
                UILanguageList[(int)UILanguageIndex.ARankScore].text = "Score 5";
                UILanguageList[(int)UILanguageIndex.BRankScore].text = "Score 4";
                UILanguageList[(int)UILanguageIndex.CRankScore].text = "Score 3";
                UILanguageList[(int)UILanguageIndex.DRankScore].text = "Score 2";
                UILanguageList[(int)UILanguageIndex.FRankScore].text = "Score 1";
                UILanguageList[(int)UILanguageIndex.SpecialPercentTitle].text = "Special customer probability";
                UILanguageList[(int)UILanguageIndex.ScorePercent1].text = "Score 1-7";
                UILanguageList[(int)UILanguageIndex.ScorePercent2].text = "Score 8-14";
                UILanguageList[(int)UILanguageIndex.ScorePercent3].text = "Score 15-21";
                UILanguageList[(int)UILanguageIndex.ScorePercent4].text = "Score 22-28";
                UILanguageList[(int)UILanguageIndex.AutoMergeAdsExplain].text = "watch an advertisement and run auto-merge for 10 minutes.";
                UILanguageList[(int)UILanguageIndex.EquipKeyExplain].text = "Open the equipment box using a key";
                UILanguageList[(int)UILanguageIndex.EquipMixAll].text = "ALL Mix";
                UILanguageList[(int)UILanguageIndex.EquipMixPanel].text = "Mix";
                UILanguageList[(int)UILanguageIndex.SlotArrangeBtn].text = "Slot";
                UILanguageList[(int)UILanguageIndex.RankArrangeBtn].text = "Rank";
                UILanguageList[(int)UILanguageIndex.EquipMix].text = "Mix";
                UILanguageList[(int)UILanguageIndex.MyEquip].text = "My Equip";
                UILanguageList[(int)UILanguageIndex.MixExplain].text = "Select the equipment you want to merge!";
                UILanguageList[(int)UILanguageIndex.MixEquipGold].text = "Gain Gold 1 % -> 1.5 %";
                UILanguageList[(int)UILanguageIndex.EquipArrangeUpBtn].text = "Arr";
                UILanguageList[(int)UILanguageIndex.EquipArrangeDownBtn].text = "Arr";
                UILanguageList[(int)UILanguageIndex.SpecialBoxExplain].text = "5 Equipment Gacha S ~ C Rank";
                UILanguageList[(int)UILanguageIndex.NormalBoxExplain].text = "5 Equipment Gacha A ~ D Rank";
                UILanguageList[(int)UILanguageIndex.SpecialBoxTitle].text = "Special Gift Box";
                UILanguageList[(int)UILanguageIndex.NormalBoxTitle].text = "Normal Gift Box";
                UILanguageList[(int)UILanguageIndex.MixSumLevel].text = "Mix Level";
                
                
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
    
    
    public void UpdateEquipBtnText(bool isEquipped)
    {
        switch (currentLanguage)
        {
            case Language.Korean:
                UILanguageList[(int)UILanguageIndex.EquipSet].text = isEquipped ? "장착 해제" : "장착";
                break;

            case Language.English:
                UILanguageList[(int)UILanguageIndex.EquipSet].text = isEquipped ? "UnEquip" : "Equip";
                break;
        }
    }
    
    
    public string GetGoldIncrementText(float currentGoldIncrement, float nextGoldIncrement, Language currentLanguage)
    {
        switch (currentLanguage)
        {
            case Language.Korean:
                return $"골드 획득량 {currentGoldIncrement} % -> {nextGoldIncrement} %";

            case Language.English:
                return $"Gold Gain {currentGoldIncrement} % -> {nextGoldIncrement} %";

            default:
                return "";
        }
    }
    
    public string GetTotalCandyLevelText(int totalLevelSum, Language currentLanguage)
    {
        switch (currentLanguage)
        {
            case Language.Korean:
                return "캔디 총합 \n" + totalLevelSum.ToString();
            case Language.English:
                return "Mix Level \n" + totalLevelSum.ToString();
            default:
                return "";
        }
    }

    
    public Language GetCurrentLanguage()
    {
        return currentLanguage;
    }
    
}
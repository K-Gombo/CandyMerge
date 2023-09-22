using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataController : MonoBehaviour
{
    public static DataController instance;
    [SerializeField] GiftBoxController giftBoxController;
    [SerializeField] UpgradeManager upgradeManager;
    [SerializeField] UpgradeUI upgradeUI;
    [SerializeField] AutoCreateBtn autoCreateBtn;

    [SerializeField] Toggle BGMToggle;
    [SerializeField] Toggle SoundToggle;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
    }

    public void ALL_LOAD()
    {
        CandiesRemaining_Load();
        Player_Level_Load();
        Player_Experience_Load();
        Option_BGM_Load();
        Option_Sound_Load();
        Upgrade_luckyCreateLevel_Load();
        Upgrade_createSpeedLevel_Load();
        Upgrade_removeLockedLevel_Load();
        Upgrade_maxCandiesLevel_Load();
        Upgrade_candyLevel_Load();
        Upgrade_passiveAutoCreateSpeedLevel_Load();
        Upgrade_goldUpLevel_Load();
        Upgrade_luckyGoldLevel_Load();
        Upgrade_OffLineRewardBonusUpLevel_Load();
        Auto_Create_Load();
        CurrencyManager.instance.LoadCurrencies();
        EquipmentManager.instance.LoadEquipData();


        HappyLevel.instance.InitUI();
        upgradeUI.UpdateUI();
    }

    private void CandiesRemaining_DataSave()
    {
        ES3.Save("candiesRemaining", GameData.candiesRemaining);
    }
    public void CandiesRemaining_Save()
    {
        GameData.candiesRemaining = giftBoxController.candiesRemaining;
        CandiesRemaining_DataSave();
    }

    private void CandiesRemaining_DataLoad()
    {
        GameData.candiesRemaining = ES3.Load("candiesRemaining", 0);
    }
    public void CandiesRemaining_Load()
    {
        CandiesRemaining_DataLoad();
        giftBoxController.candiesRemaining = GameData.candiesRemaining;
    }


    private void Player_Level_DataSave()
    {
        ES3.Save("player_Level", GameData.player_Level);
    }
    public void Player_Level_Save()
    {
        GameData.player_Level = HappyLevel.instance.CurrentLevel;
        Player_Level_DataSave();
    }

    private void Player_Level_DataLoad()
    {
        GameData.player_Level = ES3.Load("player_Level", 1);
    }
    public void Player_Level_Load()
    {
        Player_Level_DataLoad();
        HappyLevel.instance.CurrentLevel = GameData.player_Level;
    }


    private void Player_Experience_DataSave()
    {
        ES3.Save("player_Experience", GameData.player_Experience);
    }
    public void Player_Experience_Save()
    {
        GameData.player_Experience = HappyLevel.instance.currentExperience;
        Player_Experience_DataSave();
    }

    private void Player_Experience_DataLoad()
    {
        GameData.player_Experience = ES3.Load("player_Experience", 0);
    }

    public void Player_Experience_Load()
    {
        Player_Experience_DataLoad();
        HappyLevel.instance.currentExperience = GameData.player_Experience;
    }


    private void Option_BGM_DataSave()
    {
        ES3.Save("option_BGM", GameData.option_BGM);
    }

    public void Option_BGM_Save()
    {
        GameData.option_BGM = BGMToggle.isOn;
        Option_BGM_DataSave();
    }

    private void Option_BGM_DataLoad()
    {
         GameData.option_BGM = ES3.Load("option_BGM", true);
    }
    public void Option_BGM_Load()
    {
        Option_BGM_DataLoad();
        BGMToggle.isOn = GameData.option_BGM;

        BGMToggle.GetComponent<OptionToggle>().SwitchToggle(GameData.option_BGM);
    }


    private void Option_Sound_DataSave()
    {
        ES3.Save("option_Sound", GameData.option_Sound);
    }

    public void Option_Sound_Save()
    {
        GameData.option_Sound = SoundToggle.isOn;
        Option_Sound_DataSave();
    }

    private void Option_Sound_DataLoad()
    {
         GameData.option_Sound = ES3.Load("option_Sound", true);
    }
    public void Option_Sound_Load()
    {
        Option_Sound_DataLoad();
        SoundToggle.isOn = GameData.option_Sound;

        SoundToggle.GetComponent<OptionToggle>().SwitchToggle(GameData.option_Sound);
    }


    // Upgrade
    private void Upgrade_luckyCreateLevel_DataSave()
    {
        ES3.Save("upgrade_luckyCreateLevel", GameData.luckyCreateLevel);
    }
    public void Upgrade_luckyCreateLevel_Save()
    {
        GameData.luckyCreateLevel = upgradeManager.luckyCreateLevel;
        Upgrade_luckyCreateLevel_DataSave();
    }

    private void Upgrade_luckyCreateLevel_DataLoad()
    {
        GameData.luckyCreateLevel = ES3.Load("upgrade_luckyCreateLevel", 0);
    }
    public void Upgrade_luckyCreateLevel_Load()
    {
        Upgrade_luckyCreateLevel_DataLoad();
        upgradeManager.luckyCreateLevel = GameData.luckyCreateLevel;

        upgradeManager.LuckyCreateUp(GameData.luckyCreateLevel);
    }


    private void Upgrade_createSpeedLevel_DataSave()
    {
        ES3.Save("upgrade_createSpeedLevel", GameData.createSpeedLevel);
    }
    public void Upgrade_createSpeedLevel_Save()
    {
        GameData.createSpeedLevel = upgradeManager.createSpeedLevel;
        Upgrade_createSpeedLevel_DataSave();
    }

    private void Upgrade_createSpeedLevel_DataLoad()
    {
        GameData.createSpeedLevel = ES3.Load("upgrade_createSpeedLevel", 0);
    }
    public void Upgrade_createSpeedLevel_Load()
    {
        Upgrade_createSpeedLevel_DataLoad();
        upgradeManager.createSpeedLevel = GameData.createSpeedLevel;

        upgradeManager.CreateSpeedUp(GameData.createSpeedLevel);
    }


    private void Upgrade_removeLockedLevel_DataSave()
    {
        ES3.Save("upgrade_removeLockedLevel", GameData.removeLockedLevel);
    }
    public void Upgrade_removeLockedLevel_Save()
    {
        GameData.removeLockedLevel = upgradeManager.removeLockedLevel;
        Upgrade_removeLockedLevel_DataSave();
    }

    private void Upgrade_removeLockedLevel_DataLoad()
    {
        GameData.removeLockedLevel = ES3.Load("upgrade_removeLockedLevel", 0);
    }
    public void Upgrade_removeLockedLevel_Load()
    {
        Upgrade_removeLockedLevel_DataLoad();
        upgradeManager.removeLockedLevel = GameData.removeLockedLevel;



        upgradeManager.RemoveLocked(GameData.removeLockedLevel);
        
    }


    private void Upgrade_maxCandiesLevel_DataSave()
    {
        ES3.Save("upgrade_maxCandiesLevel", GameData.maxCandiesLevel);
    }
    public void Upgrade_maxCandiesLevel_Save()
    {
        GameData.maxCandiesLevel = upgradeManager.maxCandiesLevel;
        Upgrade_maxCandiesLevel_DataSave();
    }

    private void Upgrade_maxCandiesLevel_DataLoad()
    {
        GameData.maxCandiesLevel = ES3.Load("upgrade_maxCandiesLevel", 0);
    }
    public void Upgrade_maxCandiesLevel_Load()
    {
        Upgrade_maxCandiesLevel_DataLoad();
        upgradeManager.maxCandiesLevel = GameData.maxCandiesLevel;


        upgradeManager.MaxCandiesUp(GameData.maxCandiesLevel);
    }


    private void Upgrade_candyLevel_DataSave()
    {
        ES3.Save("upgrade_candyLevel", GameData.candyLevel);
    }
    public void Upgrade_candyLevel_Save()
    {
        GameData.candyLevel = upgradeManager.candyLevel;
        Upgrade_candyLevel_DataSave();
    }

    private void Upgrade_candyLevel_DataLoad()
    {
        GameData.candyLevel = ES3.Load("upgrade_candyLevel", 0);
    }
    public void Upgrade_candyLevel_Load()
    {
        Upgrade_candyLevel_DataLoad();
        upgradeManager.candyLevel = GameData.candyLevel;

        upgradeManager.CandyLevelUp(GameData.candyLevel);
    }


    private void Upgrade_passiveAutoCreateSpeedLevel_DataSave()
    {
        ES3.Save("upgrade_passiveAutoCreateSpeedLevel", GameData.passiveAutoCreateSpeedLevel);
    }
    public void Upgrade_passiveAutoCreateSpeedLevel_Save()
    {
        GameData.passiveAutoCreateSpeedLevel = upgradeManager.passiveAutoCreateSpeedLevel;
        Upgrade_passiveAutoCreateSpeedLevel_DataSave();
    }

    private void Upgrade_passiveAutoCreateSpeedLevel_DataLoad()
    {
        GameData.passiveAutoCreateSpeedLevel = ES3.Load("upgrade_passiveAutoCreateSpeedLevel", 0);
    }
    public void Upgrade_passiveAutoCreateSpeedLevel_Load()
    {
        Upgrade_passiveAutoCreateSpeedLevel_DataLoad();
        upgradeManager.passiveAutoCreateSpeedLevel = GameData.passiveAutoCreateSpeedLevel;


        upgradeManager.PassiveAutoCreateSpeedUp(GameData.passiveAutoCreateSpeedLevel);
    }


    private void Upgrade_goldUpLevel_DataSave()
    {
        ES3.Save("upgrade_goldUpLevel", GameData.goldUpLevel);
    }
    public void Upgrade_goldUpLevel_Save()
    {
        GameData.goldUpLevel = upgradeManager.goldUpLevel;
        Upgrade_goldUpLevel_DataSave();
    }

    private void Upgrade_goldUpLevel_DataLoad()
    {
        GameData.goldUpLevel = ES3.Load("upgrade_goldUpLevel", 0);
    }
    public void Upgrade_goldUpLevel_Load()
    {
        Upgrade_goldUpLevel_DataLoad();
        upgradeManager.goldUpLevel = GameData.goldUpLevel;

        upgradeManager.GoldUp(GameData.goldUpLevel);
    }


    private void Upgrade_luckyGoldLevel_DataSave()
    {
        ES3.Save("upgrade_luckyGoldLevel", GameData.luckyGoldLevel);
    }
    public void Upgrade_luckyGoldLevel_Save()
    {
        GameData.luckyGoldLevel = upgradeManager.luckyGoldLevel;
        Upgrade_luckyGoldLevel_DataSave();
    }

    private void Upgrade_luckyGoldLevel_DataLoad()
    {
        GameData.luckyGoldLevel = ES3.Load("upgrade_luckyGoldLevel", 0);
    }
    public void Upgrade_luckyGoldLevel_Load()
    {
        Upgrade_luckyGoldLevel_DataLoad();
        upgradeManager.luckyGoldLevel = GameData.luckyGoldLevel;

        upgradeManager.LuckyGoldUp(GameData.luckyGoldLevel);
    }

    private void Upgrade_OffLineRewardBonusUpLevel_DataSave()
    {
        ES3.Save("upgrade_OffLineRewardBonusUpLevel", GameData.offLineRewardBonusUpLevel);
    }
    public void Upgrade_OffLineRewardBonusUpLevel_Save()
    {
        GameData.offLineRewardBonusUpLevel = upgradeManager.offLineRewardBonusLevel;
        Upgrade_OffLineRewardBonusUpLevel_DataSave();
    }

    private void Upgrade_OffLineRewardBonusUpLevel_DataLoad()
    {
        GameData.offLineRewardBonusUpLevel = ES3.Load("upgrade_OffLineRewardBonusUpLevel", 0);
    }

    public void Upgrade_OffLineRewardBonusUpLevel_Load()
    {
        Upgrade_OffLineRewardBonusUpLevel_DataLoad();
        upgradeManager.offLineRewardBonusLevel = GameData.offLineRewardBonusUpLevel;

        upgradeManager.OffLineRewardBonusUp(GameData.offLineRewardBonusUpLevel);
    }


    private void Auto_Create_DataSave()
    {
        ES3.Save("auto_Create", GameData.autoCreateIsOn);
    }
    public void Auto_Create_Save()
    {
        GameData.autoCreateIsOn = autoCreateBtn.isOn;
        Auto_Create_DataSave();
    }

    private void Auto_Create_DataLoad()
    {
        GameData.autoCreateIsOn = ES3.Load("auto_Create", false);
    }

    public void Auto_Create_Load()
    {
        Auto_Create_DataLoad();
        autoCreateBtn.isOn = GameData.autoCreateIsOn;

        if (GameData.autoCreateIsOn)
        {
            autoCreateBtn.OnACOffBtnClick();
        }
        else
        {
            autoCreateBtn.OnACOnBtnClick();
        }
    }
}

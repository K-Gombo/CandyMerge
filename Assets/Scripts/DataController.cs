using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour
{
    public static DataController instance;
    [SerializeField] GiftBoxController giftBoxController;
    [SerializeField] UpgradeManager upgradeManager;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ALL_LOAD();

    }

    private void ALL_LOAD()
    {
        CandiesRemaining_Load();
        Player_Level_Load();
        Player_Experience_Load();
        Upgrade_luckyCreateLevel_Load();
        Upgrade_createSpeedLevel_Load();
        Upgrade_removeLockedLevel_Load();
        Upgrade_maxCandiesLevel_Load();
        Upgrade_candyLevel_Load();
        Upgrade_passiveAutoCreateSpeedLevel_Load();
        Upgrade_goldUpLevel_Load();
        Upgrade_luckyGoldLevel_Load();


        HappyLevel.instance.InitUI();
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
        GameData.luckyCreateLevel = ES3.Load("upgrade_luckyCreateLevel", 1);
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
        GameData.createSpeedLevel = ES3.Load("upgrade_createSpeedLevel", 1);
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
        GameData.removeLockedLevel = ES3.Load("upgrade_removeLockedLevel", 1);
    }
    public void Upgrade_removeLockedLevel_Load()
    {
        Upgrade_removeLockedLevel_DataLoad();
        upgradeManager.removeLockedLevel = GameData.removeLockedLevel;



        upgradeManager.RemoveLocked(GameData.removeLockedLevel - 1);
        
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
        GameData.maxCandiesLevel = ES3.Load("upgrade_maxCandiesLevel", 1);
    }
    public void Upgrade_maxCandiesLevel_Load()
    {
        Upgrade_maxCandiesLevel_DataLoad();
        upgradeManager.maxCandiesLevel = GameData.maxCandiesLevel;


        upgradeManager.MaxCandiesUp(GameData.maxCandiesLevel - 1);
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
        GameData.candyLevel = ES3.Load("upgrade_candyLevel", 1);
    }
    public void Upgrade_candyLevel_Load()
    {
        Upgrade_candyLevel_DataLoad();
        upgradeManager.candyLevel = GameData.candyLevel;

        upgradeManager.CandyLevelUp(GameData.candyLevel - 1);
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
        GameData.passiveAutoCreateSpeedLevel = ES3.Load("upgrade_passiveAutoCreateSpeedLevel", 1);
    }
    public void Upgrade_passiveAutoCreateSpeedLevel_Load()
    {
        Upgrade_passiveAutoCreateSpeedLevel_DataLoad();
        upgradeManager.passiveAutoCreateSpeedLevel = GameData.passiveAutoCreateSpeedLevel;


        upgradeManager.PassiveAutoCreateSpeedUp(GameData.passiveAutoCreateSpeedLevel - 1);
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
        GameData.goldUpLevel = ES3.Load("upgrade_goldUpLevel", 1);
    }
    public void Upgrade_goldUpLevel_Load()
    {
        Upgrade_goldUpLevel_DataLoad();
        upgradeManager.goldUpLevel = GameData.goldUpLevel;

        upgradeManager.GoldUp(GameData.goldUpLevel - 1);
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
        GameData.luckyGoldLevel = ES3.Load("upgrade_luckyGoldLevel", 1);
    }
    public void Upgrade_luckyGoldLevel_Load()
    {
        Upgrade_luckyGoldLevel_DataLoad();
        upgradeManager.luckyGoldLevel = GameData.luckyGoldLevel;

        upgradeManager.LuckyGoldUp(GameData.luckyGoldLevel - 1);
    }
}

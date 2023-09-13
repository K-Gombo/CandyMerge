using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour
{
    public static DataController instance;
    [SerializeField] GiftBoxController giftBoxController;


    private void Awake()
    {
        instance = this;
        ALL_LOAD();
    }

    private void ALL_LOAD()
    {
        CandiesRemaining_Load();
        Player_Level_Load();
        Player_Experience_Load();

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
}

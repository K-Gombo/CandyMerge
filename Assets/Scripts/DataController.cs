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


}

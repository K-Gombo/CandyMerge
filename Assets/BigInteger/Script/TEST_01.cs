using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TEST_01 : MonoBehaviour {
    public string BigDatastr; // 입력 저장 string
    public Text MyText;

    public InputField inputTarget;

    // Use this for initialization
    void Start()
    {
        //LoadMoney();

        MyText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(BigDatastr);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            BtnPlus();
        }

        //BigDatastr = inputTarget.text;
        //MyText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(BigDatastr);

    }
    public void BtnPlus()
    {
        //SaveMoney();
        //LoadMoney();
    }
    public void BtnMinus()
    {
        //SaveMoney();
        //LoadMoney();
    }
    public void SaveMoney()
    {
        string moneystr;
        moneystr = BigDatastr;
        PlayerPrefs.SetString("SAVE_GET_MONEY", moneystr);

        Debug.Log("Save");
    }
    public void LoadMoney()
    {
        string moneystr;
        moneystr = PlayerPrefs.GetString("SAVE_GET_MONEY");
        BigDatastr = moneystr;

        MyText.text = BigIntegerCtrl_global.bigInteger.ChangeMoney(BigDatastr);
        Debug.Log("Load");
    }
}

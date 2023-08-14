using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Keiwando.BigInteger;
using System;

public class BigIntegerCtrl_kr : MonoBehaviour {
    public long BigData;
    public Text MyText;


    static public string ToStringKR(BigInteger bigInteger)
    {
        string output = "";
        string outputA = ""; //경
        string outputB = ""; //조
        string outputC = ""; //억
        string outputD = ""; //만
        string outputE = ""; //원

        if (BigInteger.Compare(bigInteger, new BigInteger("10000")) == -1)
        {
            output = bigInteger.ToString() + "원";
        }
        // 설명
        // 문자형 -> 정수형(앞에 0001 일경우 앞자리0 제거용) -> 문자형 + 화폐단위(경조억만원) -> if로 빈값필터링 빈값인경우 표기안함
        else if (BigInteger.Compare(bigInteger, new BigInteger("100000000")) == -1)
        {
            string strTmp = bigInteger.ToString();

            outputD = strTmp.Substring(0, strTmp.Length - 1 - 3);
            outputE = strTmp.Substring(strTmp.Length - 1 - 3);
            int dd = int.Parse(outputD);
            int ee = int.Parse(outputE);

            outputD = dd.ToString() + "만";
            outputE = ee.ToString() + "원";

            if (ee == 0)
                outputE = "";

            output = outputD + " " + outputE;
        }
        else if (BigInteger.Compare(bigInteger, new BigInteger("1000000000000")) == -1)
        {
            string strTmp = bigInteger.ToString();

            outputC = strTmp.Substring(0, strTmp.Length - 8);
            outputD = strTmp.Substring(strTmp.Length - 8, 4); //- 7, 4
            outputE = strTmp.Substring(strTmp.Length - 1 - 3);
            int cc = int.Parse(outputC);
            int dd = int.Parse(outputD);
            int ee = int.Parse(outputE);

            outputC = cc.ToString() + "억";
            outputD = dd.ToString() + "만";
            outputE = ee.ToString() + "원";

            if (dd == 0)
                outputD = "";
            if (ee == 0)
                outputE = "";

            output = outputC + " " + outputD + " " + outputE;
        }
        else if (BigInteger.Compare(bigInteger, new BigInteger("10000000000000000")) == -1)
        {
            string strTmp = bigInteger.ToString();

            outputB = strTmp.Substring(0, strTmp.Length - 12);
            outputC = strTmp.Substring(strTmp.Length - 12, 4);
            outputD = strTmp.Substring(strTmp.Length - 8, 4);
            outputE = strTmp.Substring(strTmp.Length - 1 - 3);
            int bb = int.Parse(outputB);
            int cc = int.Parse(outputC);
            int dd = int.Parse(outputD);
            int ee = int.Parse(outputE);

            outputB = bb.ToString() + "조";
            outputC = cc.ToString() + "억";
            outputD = dd.ToString() + "만";
            outputE = ee.ToString() + "원";

            if (cc == 0)
                outputC = "";
            if (dd == 0)
                outputD = "";
            if (ee == 0)
                outputE = "";

            output = outputB + " " + outputC + " " + outputD + " " + outputE;
        }
        else if (BigInteger.Compare(bigInteger, new BigInteger("100000000000000000000")) == -1)
        {
            string strTmp = bigInteger.ToString();

            outputA = strTmp.Substring(0, strTmp.Length - 16);
            outputB = strTmp.Substring(strTmp.Length - 16, 4);
            outputC = strTmp.Substring(strTmp.Length - 12, 4);
            outputD = strTmp.Substring(strTmp.Length - 1 - 7, 4);
            outputE = strTmp.Substring(strTmp.Length - 1 - 3);
            int aa = int.Parse(outputA);
            int bb = int.Parse(outputB);
            int cc = int.Parse(outputC);
            int dd = int.Parse(outputD);
            int ee = int.Parse(outputE);

            outputA = aa.ToString() + "경";
            outputB = bb.ToString() + "조";
            outputC = cc.ToString() + "억";
            outputD = dd.ToString() + "만";
            outputE = ee.ToString() + "원";

            if (bb == 0)
                outputB = "";
            if (cc == 0)
                outputC = "";
            if (dd == 0)
                outputD = "";
            if (ee == 0)
                outputE = "";

            output = outputA + " " + outputB + " " + outputC + " " + outputD + " " + outputE;
        }

        return output;

    }

    // Use this for initialization
    void Start(){
        //LoadMoney();
        MyText.text = ToStringKR(BigData);
    }

    void Update(){
        if (Input.GetKey(KeyCode.S))
        {
            BtnPlus();
        }
    }
    public void BtnPlus () {
        BigData += 100000000;
        MyText.text = ToStringKR(BigData);
        SaveMoney();
        LoadMoney();
    }
    public void BtnMinus()
    {
        BigData -= 100000000;
        MyText.text = ToStringKR(BigData);
        SaveMoney();
        LoadMoney();
    }
    void SaveMoney()
    {
        string moneystr;
        moneystr = BigData.ToString();
        PlayerPrefs.SetString("SAVE_GET_MONEY", moneystr);
    }
    void LoadMoney()
    {
        string moneystr;
        moneystr = PlayerPrefs.GetString("SAVE_GET_MONEY");
        BigData = long.Parse(moneystr);
    }

}

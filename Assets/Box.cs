using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    private CandyStatus myCandy;


    public void SetCandy(CandyStatus candyStatus)
    {
        myCandy = candyStatus;
        ES3.Save(gameObject.name, myCandy);
    }

    public CandyStatus GetCandy()
    {
        return myCandy;
    }
}
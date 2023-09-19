using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public UnityEvent DownImage = new UnityEvent();


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SoundManager.Instance.PlayBackgroundMusic();
    }
}

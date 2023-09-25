using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public UnityEvent DownImage = new UnityEvent();

    public GameObject LoadingCanvas;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SoundManager.Instance.PlayBackgroundMusic();

        DataController.instance.ALL_LOAD();
        
        LoadingCanvas.SetActive(true);
    }
    
   
}

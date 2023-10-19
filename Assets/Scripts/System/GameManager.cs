using System;
using System.Collections;
using System.Collections.Generic;
using TapGame;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //------------------------------------Unity Functions--------------------------------
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
            
        Application.targetFrameRate = 60;
    }

    private IEnumerator Start()
    {
        // Config init
        ConfigManager.Instance.InitConfigData();
        yield return new WaitForEndOfFrame();

        // Map init
        MapController.Instance.Init();
        yield return new WaitForEndOfFrame();
        
        // Bot init
        BotController.Instance.Init();
        
        // UI Init
        UIManager.Instance.Init();
    }
}

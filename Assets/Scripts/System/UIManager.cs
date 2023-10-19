using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Button")] 
    [SerializeField] private Button spawnButton_20;
    [SerializeField] private Button spawnButton_50;
    [SerializeField] private Button spawnButton_100;

    [Header("Loading")] 
    [SerializeField] private Image loadingImage;

    [Header("FPS")] 
    [SerializeField] private Text fpsText;
    private float fps;

    [SerializeField] private Text message;
    
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
        }
        
        InvokeRepeating("GetFPS", 0.5f, 0.5f);
    }

    //------------------------------------UI Manager Functions--------------------------------
    public void Init()
    {
        loadingImage.gameObject.SetActive(true);
        
        spawnButton_20.gameObject.SetActive(false);
        spawnButton_50.gameObject.SetActive(false);
        spawnButton_100.gameObject.SetActive(false);
        
        Invoke("ActiveButton", 4.5f);
    }

    private void ActiveButton()
    {
        spawnButton_20.gameObject.SetActive(true);
        spawnButton_50.gameObject.SetActive(true);
        spawnButton_100.gameObject.SetActive(true);
        
        loadingImage.gameObject.SetActive(false);
    }
    
    public void SpawnBot(int number)
    {
        BotController.Instance.CreateBot(number);
    }

    public void GetFPS()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        fpsText.text = fps + "fps";
    }

    public void SetMessage(string content)
    {
        message.text = content;
    }
}

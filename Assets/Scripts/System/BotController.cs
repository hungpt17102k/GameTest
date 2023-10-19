using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAI.PathFinding;
using TapGame;
using Random = UnityEngine.Random;
using EZ_Pooling;

public class BotController : MonoBehaviour
{
    public static BotController Instance { get; private set; }

    [SerializeField] private float speed = 5f;
    public float Speed => speed;

    [Header("List Bot")] 
    [SerializeField] private List<GameObject> listBot;

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
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         CreateBot(5);
    //     }
    // }

    //------------------------------------Bot Controller Functions--------------------------------
    public void Init()
    {
        
    }

    public void CreateBot(int number)
    {
        StartCoroutine(CreateBotIEnum());
        
        IEnumerator CreateBotIEnum()
        {
            for (int i = 0; i < number; i++)
            {
                var randBot = Random.Range(0, listBot.Count);
                var randWayPoint = Random.Range(0, ConfigManager.Instance.MapConfig.CellStartList.Count);
                var path = MapController.Instance.dictPath[randWayPoint];
                Vector3 pos = path[0].transform.position;
                var delayTime = Random.Range(0, 0.2f);
                
                yield return new WaitForSeconds(delayTime);

                var bot = EZ_PoolManager.Spawn(listBot[randBot].transform, pos, Quaternion.identity);
                bot.GetComponent<BotMovement>().Move(path);
            }
        }
    }

    public List<Transform> CreatePath()
    {
        var randWayPoint = Random.Range(0, ConfigManager.Instance.MapConfig.CellStartList.Count);
        var path = MapController.Instance.dictPath[randWayPoint];

        return path;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TapGame;
using UnityEngine;
using GameAI.PathFinding;

public class MapController : MonoBehaviour
{
    public static MapController Instance { get; private set; }

    [Header("Map Component")] 
    [SerializeField] private RectGrid_Viz grid;

    public Dictionary<int, List<Transform>> dictPath = new Dictionary<int, List<Transform>>(); 
    private Dictionary<int, PathFinder<Vector2Int>> dictPathFinder = new Dictionary<int, PathFinder<Vector2Int>>();
    private Dictionary<int, Queue<Vector2>> dictWayPoint = new Dictionary<int, Queue<Vector2>>();

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
    
    //------------------------------------Map Controller Functions--------------------------------
    public void Init()
    {
        // Create grid
        grid.Init();

        // Get data from config
        var listCellNotWalkable = ConfigManager.Instance.MapConfig.CellNotWalkableList;
        
        // Set data to grid
        foreach (var c in listCellNotWalkable)
        {
            RectGridCell cell = grid.GetRectGridCell(c.x, c.y);
            cell.IsWalkable = false;
        }
        
        grid.ResetCellColours();
        
        // Create path
        CreatePath();
    }
    
    [ContextMenu("Save Map Config")]
    public void SaveConfig()
    {
        ConfigManager.Instance.SaveMapConfig();
    }
    
    // Sáng mai test
    private void CreatePath()
    {
        dictPath.Clear();
        dictPathFinder.Clear();
        dictWayPoint.Clear();
        
        // Get data config
        var listStartCell = ConfigManager.Instance.MapConfig.CellStartList;
        var listEndCell = ConfigManager.Instance.MapConfig.CellDesList;

        // Create dict path and waypoint
        for (int i = 0; i < listStartCell.Count; i++)
        {
            dictPathFinder.Add(i, new AStarPathFinder<Vector2Int>());
            var key = i;
            dictPathFinder[i].onSuccess = () => OnSuccessPathFinding(key);
            dictPathFinder[i].onFailure = OnFailurePathFinding;
            dictPathFinder[i].HeuristicCost = RectGrid_Viz.GetManhattanCost;
            dictPathFinder[i].NodeTraversalCost = RectGrid_Viz.GetEuclideanCost;
            dictWayPoint.Add(key, new Queue<Vector2>());
            dictPath.Add(key, new List<Transform>());
            
            // Init
            RectGridCell start = grid.GetRectGridCell(listStartCell[i].x, listStartCell[i].y);
            var rand = Random.Range(0, listEndCell.Count);
            RectGridCell end = grid.GetRectGridCell(listEndCell[rand].x, listEndCell[rand].y);
            dictPathFinder[i].Initialize(start, end);
            StartCoroutine(Coroutine_FindPathSteps(key));
        }
    }
    
    IEnumerator Coroutine_FindPathSteps(int key)
    {
        while(dictPathFinder[key].Status == PathFinderStatus.RUNNING)
        {
            dictPathFinder[key].Step();
            yield return null;
        }
    }
    
    private void AddWayPoint(int key, Vector2 pt)
    {
        dictWayPoint[key].Enqueue(pt);
        dictPath[key].Add(grid.GetCellTrans((int)pt.x, (int)pt.y));
    }
    
    void OnFailurePathFinding()
    {
        Debug.Log("Error: Cannot find path");
    }
    
    void OnSuccessPathFinding(int key)
    {
        PathFinder<Vector2Int>.PathFinderNode node = dictPathFinder[key].CurrentNode;
        List<Vector2Int> reverse_indices = new List<Vector2Int>();
        while(node != null)
        {
            reverse_indices.Add(node.Location.Value);
            node = node.Parent;
        }
        for(int i = reverse_indices.Count -1; i >= 0; i--)
        {
            AddWayPoint(key, new Vector2(reverse_indices[i].x, reverse_indices[i].y));
        }
        
        print(dictPath[key].Count);
    }
}

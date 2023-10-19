using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TapGame.ConfigData.MapConfig;

namespace TapGame
{
    public class ConfigManager : MonoBehaviour
    {
        public static ConfigManager Instance { get; private set; }
        
        //------------------------------------Config Data--------------------------------
        private ConfigData _configData;
        public ConfigData.MapConfig MapConfig => _configData.mapConfig;

        private List<string> _listFileConfig;
        
        private readonly string PROP_MAP = "Map";

        //------------------------------------Unity Functions--------------------------------
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Found more than one Config Manager in the scene");
            }

            Instance = this;
        }
        
        //------------------------------------ConfigManager Functions--------------------------------
        public void InitConfigData()
        {
            // Init config data
            _configData = new ConfigData();
            
            // Set config data will appear
            _listFileConfig = new List<string>()
            {
                PROP_MAP,
            };

            // Load config data
            this._configData = new ConfigData();
            
            if (_listFileConfig.Contains(PROP_MAP))
            {
                _configData.mapConfig = LoadResouceConfig<ConfigData.MapConfig>(PROP_MAP);
            }
        }
        
        private T LoadResouceConfig<T>(string fileName) where T : new()
        {
            var dataLoad = Resources.Load<TextAsset>("Config/Map");
            var data = JsonUtility.FromJson<T>(dataLoad.text);
            
            // If no data can be loaded, initialize to a new game
            if (data == null)
            {
                Debug.Log("No data was found. Initializing data to defaults.");
                UIManager.Instance.SetMessage("No data was found");
                // Create new data
                data = new T();
            }
            else
            {
                UIManager.Instance.SetMessage("Data was found");
            }

            return data;
        }
        
        [ContextMenu("Test")]
        public void SaveConfig()
        {
            string path = "Assets/Resources/Config/";
            var dataHandler = new FileDataHandler(path, PROP_MAP);
            var testConfigData = new ConfigData();
            testConfigData.mapConfig = new ConfigData.MapConfig();
            
            dataHandler.Save(testConfigData.mapConfig);
        }

        public void SaveMapConfig()
        {
            string path = "Assets/Resources/Config/";
            var dataHandler = new FileDataHandler(path, PROP_MAP);
            dataHandler.Save(MapConfig);
        }

        public void AddToMapConfig(int x, int y)
        {
            var cell = new CellNotWalkable(x, y);
            MapConfig.CellNotWalkableList.Add(cell);
        }
    }
}
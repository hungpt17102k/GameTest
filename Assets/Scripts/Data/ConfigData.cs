using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TapGame
{
    // This data for loading from config to game
    
    [Serializable]
    public class ConfigData
    {
        public MapConfig mapConfig;

        // The value defined in this constructor will be the default values
        // The game starts with when there's no data to load
        public ConfigData()
        {
            mapConfig = new MapConfig();
        }

        //------------------------------------Map Config--------------------------------
        [Serializable]
        public class MapConfig
        {
            public List<CellNotWalkable> CellNotWalkableList;
            public List<CellStart> CellStartList;
            public List<CellDes> CellDesList;

            public MapConfig()
            {
                CellNotWalkableList = new List<CellNotWalkable>();
                CellStartList = new List<CellStart>();
                CellDesList = new List<CellDes>();
            }

            [Serializable]
            public class CellNotWalkable
            {
                public int x;
                public int y;

                public CellNotWalkable(int x, int y)
                {
                    this.x = x;
                    this.y = y;
                }
            }
            
            [Serializable]
            public class CellStart
            {
                public int x;
                public int y;
                
                public CellStart(int x, int y)
                {
                    this.x = x;
                    this.y = y;
                }
            }

            [Serializable]
            public class CellDes
            {
                public int x;
                public int y;
                
                public CellDes(int x, int y)
                {
                    this.x = x;
                    this.y = y;
                }
            }
        }
    }
}
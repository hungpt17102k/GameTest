using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TapGame
{
    [CreateAssetMenu(fileName = "ConfigSO", menuName = "DataSO/ConfigSO")]
    public class ConfigDataSO : ScriptableObject
    {
        public ConfigData ConfigData;
    }
}
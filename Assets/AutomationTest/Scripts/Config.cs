using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Khite.AutomationTest
{
    public class Config : ScriptableObject
    {
        [SerializeField] SceneAsset scene;

        public SceneAsset Scene => scene;
        public List<TestUnit> Units { get; set; } = new List<TestUnit>();


        [System.Serializable]
        public class TestUnit
        {
            public string typeStr;
        }


        //defines
        public const char SPLIT = '-';
        public const string SUCCESS = "success";
        public static string TestLog => Application.persistentDataPath + "/testlog.txt";
    }
}

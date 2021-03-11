using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using System;

namespace Khite.AutomationTest
{
    //[ExecuteInEditMode]
    public class AutomationTestMenu
    {
        [MenuItem("Window/Khite/Test Menu")]
        public static void ShowMenu()
        {
            string path = "Assets/AutomationTest/Resources/Test_Config.asset";
            var config = AssetDatabase.LoadAssetAtPath(path, typeof(Config));
            if(config == null)
            {
                config = ScriptableObject.CreateInstance<Config>();
                AssetDatabase.CreateAsset(config, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            EditorApplication.ExecuteMenuItem("Window/General/Inspector");
            AssetDatabase.OpenAsset(config);
        }


        void OnGUI()
        {
        }
    }
}

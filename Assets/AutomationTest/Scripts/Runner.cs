using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Khite.AutomationTest;
using System;
using System.IO;
using System.Linq;
using UnityEditor;

namespace Khite.AutomationTest
{
    public class Runner : MonoBehaviour
    {
        void Awake()
        {
            var list = Resources.Load<Config>("Test_Config").Units;
            StartCoroutine(RunAllTest(list));
        }

        IEnumerator RunAllTest(List<Config.TestUnit> list)
        {
            string logFix = "";
            foreach(var e in list)
            {
                var componentType = Type.GetType(e.typeStr);
                var component = new GameObject("Test").AddComponent(componentType);
                var testScript = component.GetComponent<TestScript>();

                yield return testScript.EnumratorRun();

                string color = testScript.IsSuccess ? "lime" : "red";
                string result = testScript.IsSuccess ? "Success" : "Fail";
                string log = testScript.IsSuccess ? "" : "\n" + testScript.ErrorMessage;

                Debug.Log($"<color=yellow>{e.typeStr}</color> has result <color={color}>{result}</color>{log}");

                logFix += testScript.LogToFix + '\n';
            }

            //write result
            string logPath = Config.TestLog;
            StreamWriter writer = null;
            if (!File.Exists(logPath))
                writer = new StreamWriter(File.Create(logPath));
            else
                writer = new StreamWriter(logPath, false);

            writer.Write(logFix);
            writer.Close();


            //end test
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }
    }
}

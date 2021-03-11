using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Khite.AutomationTest;
using static Khite.AutomationTest.Config;
using UnityEditor.SceneManagement;
using System;
using System.Linq;
using System.IO;

[CustomEditor(typeof(Config))]
public class ConfigEditor : Editor
{
    List<TestUnit> listUnits = new List<TestUnit>();
    List<string> logFailed = new List<string>();
    public override void OnInspectorGUI()
    {
        GUILayout.Label($"Test Cases", new GUIStyle { fontSize = 20, fontStyle = FontStyle.BoldAndItalic, alignment = TextAnchor.UpperCenter, normal = new GUIStyleState { textColor = Color.green } });
        if(logFailed.Count != listUnits.Count)
            EditorGUILayout.HelpBox("You need to run all test or new test once to be View/Fix available", MessageType.Info, true);

        foreach (var unit in listUnits)
        {
            TestUnit cache = unit;

            GUILayout.BeginHorizontal();

            Color labelColor = Color.white;
            GUILayout.Label($"<color=yellow>{cache.typeStr}</color>", new GUIStyle { alignment = TextAnchor.MiddleLeft, normal = new GUIStyleState { textColor = labelColor } });
            if (GUILayout.Button("Run"))
            {
                Run(new List<TestUnit>() { cache });
            }

            var inLog = logFailed.Find(e => Path.GetFileNameWithoutExtension(e.Split(SPLIT)[0]) == cache.typeStr);
            if (inLog != null && !inLog.Contains(SUCCESS))
            {
                if (GUILayout.Button("Fix"))
                {
                    var split = inLog.Split(SPLIT);
                    UnityEditorInternal.InternalEditorUtility.TryOpenErrorFileFromConsole(split[0], int.Parse(split[1]));
                }
            }
            else if(inLog != null)
            {
                if (GUILayout.Button("View"))
                {
                    var split = inLog.Split(SPLIT);
                    UnityEditorInternal.InternalEditorUtility.TryOpenErrorFileFromConsole(split[0], 0);
                }
            }

            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Run all"))
        {
            Run(listUnits);
        }

        if (GUILayout.Button("Update"))
        {
            UpdateUnits();
        }

        DrawDefaultInspector();
    }

    private void OnEnable()
    {
        UpdateUnits();
    }

    bool Run(List<TestUnit> list)
    {
        UpdateUnits();
        var config = target as Config;
        config.Units = list;

        if (EditorSceneManager.GetActiveScene().name == config.Scene.name)
        {
            EditorApplication.isPlaying = true;
            return true;
        }
        else
        {
            LogE("Active scene is not automatic scene!");
            return false;
        }
    }

    void UpdateUnits()
    {
        var type = typeof(TestScript);
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .Select(x => x.GetTypes())
            .SelectMany(x => x)
            .Where(x =>
            type.IsAssignableFrom(x) && x != type);

        listUnits.Clear();
        foreach (var e in types)
        {
            listUnits.Add(new TestUnit { typeStr = e.Name});
        }

        logFailed.Clear();
        if (File.Exists(TestLog))
        {
            var logs = File.ReadAllLines(TestLog);
            logFailed = new List<string>(logs);
        }

    }

    public static void LogE(string log) => Debug.LogError("[AutomationTest] " + log);
    public static void LogW(string log) => Debug.LogWarning("[AutomationTest] " + log);
}

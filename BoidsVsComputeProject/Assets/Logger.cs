using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using JetBrains.Annotations;
using OpenCover.Framework.Model;
using Palmmedia.ReportGenerator.Core.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

[Flags]
public enum LogLevel
{
    none = 0,
    fps = 2,
    objectsInScene = 4
    
}
public class Logger : MonoBehaviour
{
    private static Logger _instance;
    public static Logger instance { get { return _instance; } }
    
    public LogLevel logAmount;
    [SerializeField] private string path = String.Empty;
    private string log = string.Empty;
    private int objectsInScene;
    private long frameCount = 0;
    [SerializeField]private string version = "0.0.1";
    private void Awake()
    {
        if(_instance != null) Destroy(gameObject);
        _instance = this;
        EditorApplication.playModeStateChanged += change => ModeChanged();
        objectsInScene = GameObject.FindObjectsOfType(typeof(MonoBehaviour)).Length;
    }

    private void LateUpdate()
    {
        frameCount++;
        log += new LogData(frameCount, Time.deltaTime, objectsInScene, logAmount).ConvertToJson() + "\n";
    }

    void ModeChanged ()
    {
        if (!EditorApplication.isPlayingOrWillChangePlaymode &&
            EditorApplication.isPlaying )
        {
            DateTime rightNow = DateTime.Now; 
            string dateString = $"{rightNow.Day},{rightNow.Month},{rightNow.Year} {rightNow.Hour} {rightNow.Minute} {rightNow.Second}";
            string newLog = $"Log of: {dateString} Version: {version}\n{log}";
            JsonUtility.ToJson(newLog, true);
            if (path == String.Empty || path == "default" || !(AssetDatabase.IsValidFolder(path)))
            {
                System.IO.File.WriteAllText($"{Application.persistentDataPath}/log_{dateString}_{version}.json",
                    newLog);
        
                Process.Start(Application.persistentDataPath);
            }
            else
            {
                    System.IO.File.WriteAllText(path + "/log: " + dateString + " "+version+".json",
                        newLog);          
            }
        }
    }
    
}

[Serializable]
public struct LogData
{
    public float frameTime;
    public int actorsInScene;
    public long frame;
    public LogData(long pFrameCount, float pFrameTime, int pActorsInScene, LogLevel level)
    {
        frame = pFrameCount;
        frameTime = -1;
        actorsInScene = -1;

        if(level.HasFlag(LogLevel.fps))
        {
            frameTime = pFrameTime;
        }
        if(level.HasFlag(LogLevel.objectsInScene))
        {
            actorsInScene = pActorsInScene;
        }


    }
    public string ConvertToJson()
    {
        string jsonString = JsonUtility.ToJson(this,true);
            
        return jsonString;
    }
}

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
using DefaultNamespace;
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

    public static Logger instance
    {
        get { return _instance; }
    }

    public LogLevel logAmount;
    [SerializeField] private string path = String.Empty;
    private LogList log = new LogList();
    private int boidsInScene;
    private int obstaclesInScene;
    private long frameCount = 0;
    [SerializeField] private string version = "0.0.1";
    [SerializeField] private bool oop;

    private void Awake()
    {
        if (_instance != null) Destroy(gameObject);
        _instance = this;
        EditorApplication.playModeStateChanged += change => ModeChanged();
        boidsInScene = GameObject.FindObjectsOfType(oop ? typeof(OOP.Boid) : typeof(Boid)).Length;
        obstaclesInScene = GameObject.FindGameObjectsWithTag("Obstacle").Length;
    }

    private void LateUpdate()
    {
        frameCount++;
        log.lD.Add(new LogData(frameCount, Time.deltaTime, boidsInScene, obstaclesInScene, BoidHelper.directions.Length, logAmount));
    }

    void ModeChanged()
    {
        if (!EditorApplication.isPlayingOrWillChangePlaymode &&
            EditorApplication.isPlaying)
        {
            var s = JsonUtility.ToJson(log);
                System.IO.File.WriteAllText(
                    $"{Application.persistentDataPath}/{(oop ? "OOP" : "Compute")} {boidsInScene} boids {BoidHelper.directions.Length}.json", s);
        }

    }

    [Serializable]
    public struct LogData
    {

        public float frameTime;
        public int boidsInScene;
        public int obstaclesInScene;
        public int viewDirections;
        public long frame;

        public LogData(long pFrameCount, float pFrameTime, int pBoidsInScene, int pObstaclesInScene, int pViewDirections, LogLevel level)
        {
            frame = pFrameCount;
            frameTime = -1;
            boidsInScene = -1;
            obstaclesInScene = pObstaclesInScene;
            viewDirections = pViewDirections;
            if (level.HasFlag(LogLevel.fps))
            {
                frameTime = pFrameTime;
            }

            if (level.HasFlag(LogLevel.objectsInScene))
            {
                boidsInScene = pBoidsInScene;
            }


        }

        public string ConvertToJson()
        {
            string jsonString = JsonUtility.ToJson(this, true);

            return jsonString;
        }
    }
}

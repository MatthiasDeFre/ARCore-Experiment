using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Linq;
using UnityEngine.Profiling;
using UnityEditor;

public class ExperimentProfiler : MonoBehaviour
{
    private float frequency = 1.0f;
    private string fileName;
    void Start()
    {
        StringBuilder sb = new StringBuilder();
        long milliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        fileName = Application.persistentDataPath + "/stats" + milliseconds + ".csv"; 
        System.IO.File.WriteAllText(fileName, "Time;Fps;ReservedMemory;AllocatedMemory;GameObjectCount;activeGameObjectCount;visibleGameObjectCount\n");
        StartCoroutine(Stats());
    }

    private IEnumerator Stats()
    {
        for (; ; )
        {
            // FPS
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(frequency);
            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;

            // Memory
            long reserved = Profiler.GetTotalReservedMemoryLong();
            long allocated = Profiler.GetTotalAllocatedMemoryLong();

            // Gameobjects
            var gameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            int activeGameObjectCount = gameObjects.Where(g => g?.activeSelf == true).Count();
            int visibleGameObjectCount = gameObjects.Where(g => {
                var renderer = g.GetComponent<Renderer>();
                return renderer && renderer.isVisible;
            }).Count();
            int gameObjectCount = gameObjects.Length;

            // Write to csv
            StringBuilder sb = new StringBuilder();
            sb.Append(Time.time).Append(";").Append(Mathf.RoundToInt(frameCount / timeSpan))
                .Append(';').Append(reserved).Append(';').Append(allocated)
                .Append(";").Append(gameObjectCount).Append(";").Append(activeGameObjectCount)
                .Append(";").Append(visibleGameObjectCount).Append('\n');
            System.IO.File.AppendAllText(fileName, sb.ToString());
        }
    }

}

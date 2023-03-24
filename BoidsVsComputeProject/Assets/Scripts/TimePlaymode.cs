using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePlaymode : MonoBehaviour
{
    [SerializeField] private float secondsOfPlay = 10;
    [SerializeField] private bool timePlay = false;
    private void Start()
    {
        if(timePlay)
        StartCoroutine(TimeTillCancel());
    }

    private IEnumerator  TimeTillCancel()
    {
        Debug.Log("Hi");
        yield return new WaitForSeconds(secondsOfPlay);
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        yield break;
    }

}

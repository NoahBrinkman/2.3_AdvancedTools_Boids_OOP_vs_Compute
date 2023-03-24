using UnityEngine;

namespace DefaultNamespace
{
    public class EditorDebug
    {
        public static void Print(object message)
        {
            #if UNITY_EDITOR
            Debug.Log(message);
            #endif
        }
        public static void Print(object message, Object context)
        {
            #if UNITY_EDITOR
                Debug.Log(message, context);
            #endif
        }
        public static void PrintWarning(object message)
        {
            #if UNITY_EDITOR
                Debug.LogWarning(message);
            #endif
        }
        public static void PrintWarning(object message, Object context)
        {
            #if UNITY_EDITOR
                Debug.LogWarning(message, context);
            #endif
        }
        public static void PrintError(object message)
        {
            #if UNITY_EDITOR
            Debug.LogError(message);
            #endif
        }
        public static void PrintError(object message, Object context)
        {
            #if UNITY_EDITOR
                Debug.LogError(message, context);
            #endif
        }
    }
}
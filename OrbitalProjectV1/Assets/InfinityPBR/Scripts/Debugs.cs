using UnityEngine;

namespace InfinityPBR
{
    public static class Debugs
    {
        public static bool showDebugs = true;
        // TODO Update this so that showDebugs controls whether or not we actually do any debugs
        
        public static void Log(string message, string color) => Debug.Log($"<color={color}>{message}</color>"); // ("<color=" + color + " ...

        public static void Log(string message, Color color) =>
            Log(message, "#"+ColorUtility.ToHtmlStringRGBA(color));
        
        public static void Log(this MonoBehaviour m,string message,Color color)=>
            Log(m,message, "#"+ColorUtility.ToHtmlStringRGBA(color));
        
        public static void Log(this MonoBehaviour m,string message,string color)=>
            Debug.Log($"<color={color}>{message}</color>",m);
        
        public static void Log(this MonoBehaviour m,string message)=>
            Debug.Log(message,m);
        
        public static void RequiredBy<T>(this T t,MonoBehaviour item) where T : Component
        {
            if (t == null)
                Debug.LogWarning($"<color=yellow>{typeof(T).Name}</color> required by <color=blue>{item.name}</color>",
                    item);
        }
        
        public static void RequiredBy<T>(this T t,MonoBehaviour item, bool signatureDifference = true) where T : UnityEngine.Object
        {
            
            if (t == null)
                Debug.LogWarning($"<color=yellow>{typeof(T).Name}</color> required by <color=blue>{item.name}</color>",
                    item);
        }
    }
}
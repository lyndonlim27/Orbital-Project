using UnityEditor;
using UnityEngine;

namespace InfinityPBR
{
    
    public abstract class CustomEditorScriptableObject<T> : CustomEditor<T> where T : ScriptableObject
    {
        protected T Script;

        protected virtual void OnEnable()
        {
            Script = (T) target;
            Setup();
        }
        
        protected abstract void Setup();

        protected abstract void Draw();

        public override void OnInspectorGUI()
        {
            // April 23, 2021 -- I'm pretty sure the serializedObject bits here do nothing at all.
            serializedObject.Update();
            Draw();
            serializedObject.ApplyModifiedProperties();
        }

        // April 23, 2021 -- the nasty bug was just me being silly, and setting dirty "Item" instead of "obj".
        public void SetDirty(Object obj = null)
        {
            if (obj == null)
                obj = Script;
            EditorUtility.SetDirty(obj);
        }

        public void BeginChangeCheck()
        {
            EditorGUI.BeginChangeCheck();
        }

        public void EndChangeCheck(bool setDirty = true)
        {
            if (EditorGUI.EndChangeCheck())
            {
                if (!setDirty) return;
                SetDirty();
            }
        }

    }

}

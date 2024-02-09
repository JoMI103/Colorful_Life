using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace jomi.utils {
#if UNITY_EDITOR
    public static class EditorUtils
    {
        public static int MakePopupOfResources<T>(int currentSelection, SerializedProperty serializedProperty, string path, string label = "Choose: ") where T : UnityEngine.Object
        {
            T[] ObjectList = Resources.LoadAll<T>(path);
            currentSelection = EditorGUILayout.Popup(label, currentSelection, Array.ConvertAll(ObjectList, Input => Input.name));
            if (ObjectList.Length <= currentSelection) return 0;
            serializedProperty.boxedValue = ObjectList[currentSelection];
            return currentSelection;
        }

        

    }
#endif
    public static class Jaux
    {

        public static bool exists<T>(T elementT, List<T> listT, string pro) where T : struct
        {
            foreach (T ele in listT)
                if (ele.GetType().GetField(pro).GetValue(ele).ToString() ==
                        elementT.GetType().GetField(pro).GetValue(elementT).ToString()) return true;
            return false;
        }

        /// <summary> (Make a description) </summary> 
        public static Vector3 Dontknow(Vector3 origin, Vector3 direction, float y) => origin - direction * ((origin.y - y) / direction.y);
      

        /// <summary> (Make a description) </summary>
        public static Vector3 GetcurrentMousePosition(Camera camera, float targetY)  {     
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            return Dontknow(ray.origin, ray.direction, targetY);    
        }

    }
}

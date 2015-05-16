#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using InControl.ReorderableList;

namespace InControl{
    [CustomEditor(typeof (InControlManager))]
    public class InControlManagerEditor : Editor{
        private SerializedProperty logDebugInfo;
        private SerializedProperty invertYAxis;
        private SerializedProperty enableXInput;
        private SerializedProperty useFixedUpdate;
        private SerializedProperty dontDestroyOnLoad;
        private SerializedProperty customProfiles;
        private Texture headerTexture;

        private void OnEnable(){
            logDebugInfo = serializedObject.FindProperty("logDebugInfo");
            invertYAxis = serializedObject.FindProperty("invertYAxis");
            enableXInput = serializedObject.FindProperty("enableXInput");
            useFixedUpdate = serializedObject.FindProperty("useFixedUpdate");
            dontDestroyOnLoad = serializedObject.FindProperty("dontDestroyOnLoad");
            customProfiles = serializedObject.FindProperty("customProfiles");

            var path = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this));
            headerTexture = AssetDatabase.LoadAssetAtPath<Texture>(Path.GetDirectoryName(path) + "/Images/InControlHeader.png");
        }

        public override void OnInspectorGUI(){
            serializedObject.Update();

            GUILayout.Space(5.0f);

            var headerRect = GUILayoutUtility.GetRect(0.0f, 5.0f);
            headerRect.width = headerTexture.width;
            headerRect.height = headerTexture.height;
            GUILayout.Space(headerRect.height);
            GUI.DrawTexture(headerRect, headerTexture);

            logDebugInfo.boolValue = EditorGUILayout.ToggleLeft("Log Debug Info", logDebugInfo.boolValue);
            invertYAxis.boolValue = EditorGUILayout.ToggleLeft("Invert Y Axis", invertYAxis.boolValue);
            enableXInput.boolValue = EditorGUILayout.ToggleLeft("Enable XInput (Windows)", enableXInput.boolValue);
            useFixedUpdate.boolValue = EditorGUILayout.ToggleLeft("Use Fixed Update", useFixedUpdate.boolValue);
            dontDestroyOnLoad.boolValue = EditorGUILayout.ToggleLeft("Don't Destroy On Load", dontDestroyOnLoad.boolValue);

            ReorderableListGUI.Title("Custom Profiles");
            ReorderableListGUI.ListField(customProfiles);

            ValidInputProfiles();

            GUILayout.Space(3.0f);

            serializedObject.ApplyModifiedProperties();
        }

        private void ValidInputProfiles(){
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var list = new List<Type>();
            foreach (var assembly in assemblies){
                foreach (var type in assembly.GetTypes()){
                    if (type.IsSubclassOf(typeof (UnityInputDeviceProfile))) { list.Add(type); }
                }
            }
            var types = list.ToArray();

            if (types.Any()){
                var selected = EditorGUILayout.Popup(-1, types.Select(t => t.Name).ToArray());
                if (selected != -1){
                    var t = (InControlManager) target;
                    var selectedName = types.ElementAt(selected).Name;
                    t.customProfiles.Add(selectedName);
                }
            }
        }
    }
}

#endif
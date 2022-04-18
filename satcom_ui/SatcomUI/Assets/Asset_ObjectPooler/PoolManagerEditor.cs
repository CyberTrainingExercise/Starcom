using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(PoolManager))]
public class PoolManagerEditor : Editor
{
    SerializedProperty poolManagerID;

    void OnEnable()
    {
        // Setup the SerializedProperties
        poolManagerID = serializedObject.FindProperty("poolManagerID");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        PoolManager poolManager = (PoolManager)target;
        EditorGUILayout.PropertyField(poolManagerID, new GUIContent("Pool Manager ID")); //add a property field for [poolManagerID]
        GUI.color = Color.green;
        if (GUILayout.Button("Add Pool", GUILayout.Height(50)))
        {
            Pool newPoolData = poolManager.gameObject.AddComponent<Pool>();
            newPoolData.PoolManager = poolManager;
        }
        GUI.color = Color.white;
        EditorGUILayout.BeginHorizontal();
        if (poolManager.poolUIType == PoolUIType.fullView)
        {
            GUI.color = Color.gray;
        }
        else
        {
            GUI.color = Color.white;
        }
        if (GUILayout.Button("Full View", GUILayout.Height(25)))
        {
            poolManager.poolUIType = PoolUIType.fullView;
        }
        if (poolManager.poolUIType == PoolUIType.compactView)
        {
            GUI.color = Color.gray;
        }
        else
        {
            GUI.color = Color.white;
        }
        if (GUILayout.Button("Compact View", GUILayout.Height(25)))
        {
            poolManager.poolUIType = PoolUIType.compactView;
        }
        EditorGUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();
    }
}
#endif

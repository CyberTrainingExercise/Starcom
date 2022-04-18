using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Pool))]
public class PoolEditor : Editor
{

    //list of properties to display in inspector
    SerializedProperty id;
    SerializedProperty sourcePrefab;
    SerializedProperty initialObjects;
    SerializedProperty maxObjects;
    SerializedProperty noLimit;
    SerializedProperty recycleObjects;
    SerializedProperty recycleVisible;
    SerializedProperty havePoolParent;
    SerializedProperty poolParent;

    void OnEnable()
    {
        // Setup the SerializedProperties
        id = serializedObject.FindProperty("id");
        sourcePrefab = serializedObject.FindProperty("sourcePrefab");
        initialObjects = serializedObject.FindProperty("initialObjects");
        maxObjects = serializedObject.FindProperty("maxObjects");
        noLimit = serializedObject.FindProperty("noLimit");
        recycleObjects = serializedObject.FindProperty("recycleObjects");
        recycleVisible = serializedObject.FindProperty("recycleVisible");
        havePoolParent = serializedObject.FindProperty("havePoolParent");
        poolParent = serializedObject.FindProperty("poolParent");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update(); //update the object
        Pool pool = (Pool)target; //get my PoolData script
        if (pool.PoolManager == null) //if the pool manager doesn't exist, try again
        {
            pool.PoolManager = pool.GetComponent<PoolManager>();
            if (pool.PoolManager == null)
            {
                Errors.LogError("PoolNotOnManager");
            }
        }
        EditorGUILayout.PropertyField(id, new GUIContent("Id")); //add a property field for [id]
        if (pool.PoolManager.poolUIType == PoolUIType.fullView) //checks what view we are in
        {
            EditorGUILayout.BeginHorizontal(); //begins displaying the GUI horizontally
            GUILayout.Label("Object Preview:", GUILayout.Width(150)); //adds some text that says "Object Preview"
            Texture2D poolTexture = AssetPreview.GetAssetPreview(pool.SourcePrefab); //gets the preview texture of the sourcePrefab
            GUILayout.Box(poolTexture); //makes a box with the preview texture of the sourcePrefab
            EditorGUILayout.EndHorizontal(); //ends displaying the GUI hroizontally
        }
        EditorGUILayout.PropertyField(sourcePrefab, new GUIContent("Source Prefab"));
        if (pool.PoolManager.poolUIType == PoolUIType.fullView)
        {
            EditorGUILayout.PropertyField(initialObjects, new GUIContent("Initial Amount"));
        }
        EditorGUILayout.PropertyField(noLimit, new GUIContent("No Limit"));
        if (!pool.NoLimit)
        {
            EditorGUILayout.PropertyField(maxObjects, new GUIContent("Max Objects"));
        }
        if (pool.PoolManager.poolUIType == PoolUIType.fullView)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(recycleObjects, new GUIContent("Recycle when full"));
            EditorGUILayout.PropertyField(recycleVisible, new GUIContent("Recycle Visible"));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(havePoolParent, new GUIContent("Have Pool Parent"));
            if (pool.HavePoolParent)
            {
                EditorGUILayout.PropertyField(poolParent, new GUIContent("Pool Parent"));
            }
        }
        GUI.color = Color.red;
        if (GUILayout.Button("Remove", GUILayout.Width(100)))
        {
            DestroyImmediate(pool);
            return;
        }
        GUI.color = Color.white;
        serializedObject.ApplyModifiedProperties();
    }
}
#endif

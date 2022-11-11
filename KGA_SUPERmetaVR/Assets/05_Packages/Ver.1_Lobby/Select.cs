using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.SceneManagement; //3

public class Select : Editor
{
    [MenuItem("smilejsu/Remove All Missing Script Components")]
    private static void RemoveAllMissingScriptComponents()
    {

        Object[] deepSelectedObjects = EditorUtility.CollectDeepHierarchy(Selection.gameObjects);

        Debug.Log(deepSelectedObjects.Length);

        int componentCount = 0;
        int gameObjectCount = 0;

        foreach (Object obj in deepSelectedObjects)
        {
            if (obj is GameObject go)
            {
                int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);

                //Debug.LogFormat("<color=cyan>{0}</color>", count);

                if (count > 0)
                {
                    Undo.RegisterCompleteObjectUndo(go, "Remove Missing Scripts");

                    GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);

                    componentCount += count;
                    gameObjectCount++;
                }

            }
        }

    }
}
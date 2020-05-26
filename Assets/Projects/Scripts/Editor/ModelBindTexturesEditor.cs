using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(ModelBindTextures))]
public class ModelBindTexturesEditor : Editor
{
    ModelBindTextures mbTexture;
    void OnEnable()
    {
        mbTexture = (ModelBindTextures) target;
    }

    public override void OnInspectorGUI ( )
    {
        base.OnInspectorGUI();
        EditorGUILayout.BeginVertical();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("该脚本是用于替换掉模型上的材质");
        if (GUILayout.Button("获取MeshRender"))
        {
            mbTexture.GetMeshRender();
        }
        if(GUILayout.Button("更换材质"))
        {
            mbTexture.ReplaceMaterial();
        }
        EditorGUILayout.EndVertical();

        
    }
}

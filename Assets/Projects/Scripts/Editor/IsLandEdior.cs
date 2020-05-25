using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IsLand))]
public class IsLandEdior : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        IsLand isLand = (IsLand)target;
        if(GUILayout.Button("替换材质"))
        {
            isLand.MaterialChange();
        }
    }
}

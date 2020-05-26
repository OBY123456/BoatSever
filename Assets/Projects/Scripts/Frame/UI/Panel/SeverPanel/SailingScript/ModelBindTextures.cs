using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



public class ModelBindTextures : MonoBehaviour
{
    public List<MeshRenderer> curMeshRenderList;
    
    public Material[] replaceMaterial;

    public void Start()
    {   
        //ReplaceMaterial();        
    }

    public void GetMeshRender()
    {
        curMeshRenderList = new List<MeshRenderer>(this.GetComponentsInChildren<MeshRenderer>());
        replaceMaterial = Resources.LoadAll<Material>("Boat");
    }


    public void ReplaceMaterial()
    {
        if(replaceMaterial.Length<=0)
        {
            Debug.LogError("请添加要替换掉的材质");
        }

        for(int i=0 ;i<=curMeshRenderList.Count-1 ;i++)
        {
            Material[] mats = curMeshRenderList[i].sharedMaterials;//有的Gameobject可能会有多个Mats
            Material newMat = null;
            bool check = false;
            for (int j = 0 ; j <= mats.Length - 1 ; j++)
            {

                string compareName = mats[j].name;
               GetMaterial(compareName,out newMat,out check);
                
                if (check != null)
                {
                    //mats[j] = newMat;
                    mats[j].CopyPropertiesFromMaterial(newMat);

                    Debug.Log("Replace Successfull");
                }
                else
                {
                   //Debug.Log("Replace Error");
                }
            }
           
        }
    }

    //动态数组生成的材质无法赋值给Mesh Render
    public void GetMaterial(string name,out Material mat,out bool check)
    {
        
        foreach (Material item in replaceMaterial)
        {
            if (name.Contains(item.name))
            {
                mat = item;
                check = true ;
                return;
            }
        }
        check = false;
        mat = null;
    }


    
}
 
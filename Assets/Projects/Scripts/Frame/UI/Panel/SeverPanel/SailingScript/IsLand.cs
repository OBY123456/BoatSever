using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class IsLand : MonoBehaviour
{
    private Transform[] allchild;
    // Start is called before the first frame update

    public void MaterialChange()
    {
        allchild = GetComponentsInChildren<Transform>();
        foreach (Transform item in allchild)
        {
            if (item.GetComponent<LODGroup>())
            {
                item.GetComponent<LODGroup>().enabled = true;
            }

            if (item.GetComponent<MeshRenderer>())
            {
                //Debug.Log(item.GetComponent<MeshRenderer>().material.mainTexture.name);
                Debug.Log(item.GetComponent<MeshRenderer>().sharedMaterial.name);
                //item.GetComponent<MeshRenderer>().sharedMaterial.EnableKeyword("_EMISSION");
                //item.GetComponent<MeshRenderer>().sharedMaterial.SetFloat("Emission", 1);

                if(item.GetComponent<MeshRenderer>().sharedMaterial.mainTexture!=null)
                item.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_EmissionMap", item.GetComponent<MeshRenderer>().sharedMaterial.mainTexture);
                item.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_EmissionColor", Color.black);
            }
        }
    }

}

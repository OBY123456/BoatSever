using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Boat")
        {
            other.gameObject.GetComponentInParent<AutoDrive>().MainCameraRotate();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraRotate : MonoBehaviour
{
    public Transform Target;

    [Range(0, 360)]
    public float RotateY = 0;

    // Start is called before the first frame update
    void Start()
    {
        Target = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
       // SetTargetRotate(RotateY);
    }

    public void SetTargetRotate(float value)
    {
        Target.localEulerAngles = -Vector3.up * value;
    }
}


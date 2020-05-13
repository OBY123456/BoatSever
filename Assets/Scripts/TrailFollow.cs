using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TrailFollow : MonoBehaviour
{
    public Transform target;
    public float YOffset = 0.0f;
    public float XOffset = 0.0f;
    public float ZOffset = 0.0f;
    public float distance = 1.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = target.transform.TransformPoint(new Vector3(0,0,1)*distance);
        this.transform.position = new Vector3(targetPosition.x+XOffset , YOffset , targetPosition.z+ZOffset);
    }
}

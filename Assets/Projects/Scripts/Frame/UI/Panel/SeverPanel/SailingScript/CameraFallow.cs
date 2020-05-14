using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraFallow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    private Vector3 offset2;

    public float Sensity = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        offset2 = player.position - transform.position;
    }

    private void LateUpdate()
    {
        Vector3 vector3 = player.position + player.TransformDirection(offset);
        transform.position = Vector3.Lerp(transform.position, new Vector3(vector3.x, transform.position.y, vector3.z), Time.deltaTime * Sensity);
        transform.LookAt(player.position);
    }
}

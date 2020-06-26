using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cargo : MonoBehaviour
{
    public Transform Rope;

    /// <summary>
    /// 初始位置
    /// </summary>
    public Vector3 InitialPosition;
    private Vector3 InitialRotation = new Vector3(-90, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        InitialPosition = this.transform.localPosition;
    }

    private void RopeOpen()
    {
        Rope.gameObject.SetActive(true);
    }

    private void RopeHide()
    {
        Rope.gameObject.SetActive(false);
    }

    public void Reset()
    {
        RopeHide();
        this.transform.localPosition = InitialPosition;
        this.transform.localEulerAngles = InitialRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}

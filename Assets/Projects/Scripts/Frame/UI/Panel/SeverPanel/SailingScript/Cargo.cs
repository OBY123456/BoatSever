using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cargo : MonoBehaviour
{
    public Transform Rope;

    /// <summary>
    /// 需要跟随的目标
    /// </summary>
    public Transform Traget;

    /// <summary>
    /// 是否开启跟随
    /// </summary>
    public bool IsFllow;

    /// <summary>
    /// 偏移量
    /// </summary>
    public Vector3 Offset;

    /// <summary>
    /// 初始位置
    /// </summary>
    public Vector3 InitialPosition;
    private Vector3 InitialRotation = new Vector3(-90, 0, 0);

    public RopeControl ropeControl;

    private BoxCollider[] boxColliders;

    public BoatAnimationControl animationControl;

    // Start is called before the first frame update
    void Start()
    {
        InitialPosition = this.transform.localPosition;
        boxColliders = transform.GetComponents<BoxCollider>();
    }

    private void Update()
    {
        if(IsFllow)
        {
            transform.position = Traget.position - Offset;
            if(!Rope.gameObject.activeSelf)
            {
                RopeOpen();
            }
        }
    }

    private void RopeOpen()
    {
        Rope.gameObject.SetActive(true);
        boxColliders[0].enabled = false;
        ropeControl.IsRayOpen = true;
        ropeControl.floorCollider.ColliderOpen();
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
        if(boxColliders!=null)
        boxColliders[0].enabled = true;
        IsFllow = false;
        Traget = null;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        
    }

    public void PutDown()
    {
        IsFllow = false;
        Traget = null;
        if(animationControl.TurnTable.localEulerAngles.z < 345 )
        {
            if(animationControl.TurnTable.localEulerAngles.z!=0)
            {
                boxColliders[0].enabled = true;
            }
            
        }
        RopeHide();
        ropeControl.IsRayOpen = false;
        ropeControl.floorCollider.ColliderOpen();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.transform.tag.Contains("Floor") || (other.transform.tag.Contains("Cargo")))
    //    {
    //        //IsFllow = false;
    //        //Traget = null;
    //        //RopeHide();
    //        //Debug.Log("Cargo Enter");
    //        if (Traget != null)
    //        {
    //            ropeControl.IsElongate = false;
    //            Debug.Log("Cargo Enter");
    //        }
    //    }
    //}

    //private void OnTriggerStay(Collider other)
    //{
       
    //    if (other.transform.tag.Contains("Floor") || (other.transform.tag.Contains("Cargo")))
    //    {
    //        if(Traget!=null)
    //        {
    //            ropeControl.IsElongate = false;
    //            Debug.Log("Cargo Stay");
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.transform.tag.Contains("Floor") || (other.transform.tag.Contains("Cargo") /*&& !other.transform.name.Contains(transform.name)*/))
    //    {
    //        if (Traget != null)
    //        {
    //            if(!ropeControl.IsShorten)
    //            {
    //                ropeControl.IsElongate = true;
    //                Debug.Log("Cargo Exit");
    //            }
    //        }
    //    }
    //}
}

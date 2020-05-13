using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipelineTrigger : MonoBehaviour
{
    static public PipelineTrigger instance;
    [SerializeField]
    private bool active = true;

    void Awake()
    {
        if(instance ==null)
        {
            instance = this;
        }
    }
    public bool InsTrigger = true;
    void OnTriggerEnter(Collider enter)
    {
        
        if(enter.gameObject.tag=="PipelineHead"&&active)
        {
            if (InsTrigger)
            {
                PipelineManager.instance.GetNewPipeline();
            }
            else
            {
                Debug.Log(enter.gameObject.transform.parent);
                enter.transform.parent.GetComponent<Pipeline>().SetIdleState(true);
                PipelineManager.instance.SetDefaultPipeline(enter.transform.parent.gameObject);
            }

            if(enter.transform.parent.GetComponent<Pipeline>().GetDestroyParament())
            {
                PipelineManager.instance.DestroyAllPipeline();
            }
        }
    }

    public void SetTriggerActive (bool act) 
    {
        active = act;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipelineManager : MonoBehaviour
{

    public static PipelineManager instance;
    public GameObject pipeline;

    [Header("PipeTrigger")]
    public PipelineTrigger insTrigger;//检测生成钢管的Trigger

    public GameObject pipelineRoot;
    [Header("Ins Position")]
    public GameObject instationPosition;//生成的钢管初始位置

    [Header("New Pipeline")]
    public GameObject connectedPipeline;//新生成的钢管
    [Header("Parament")]
    public GameObject firstPipeline;//第一节
    [SerializeField]
    int firstPipelineIndex;//第一节的索引
    public List<GameObject> pipelineLists;
    public float force = 2.0f;

    public bool isPlay = false;
    public bool isStop = false;
    [SerializeField]
    int activePipeline =0;//场景中Active=true的管道


    

    void Awake ( )
    {
        if(instance==null)
        {
            instance = this;
        }

        for (int i = 0 ; i <= 9 ; i++)
        {
            GameObject G = GameObject.Instantiate(pipeline , instationPosition.transform.position , Quaternion.Euler(instationPosition.transform.eulerAngles));
            G.transform.SetParent(pipelineRoot.transform);
            G.GetComponent<Transform>().localScale = new Vector3(0.7f , 1 , 1);
            G.GetComponent<Pipeline>().SetIdleState(true);
            G.SetActive(false);
            Destroy(G.GetComponent<HingeJoint>());
            connectedPipeline = G;
            pipelineLists.Add(G);
        }
        firstPipeline = pipelineLists[0];
    }

    void Start ( )
    {
    
    }

    // Update is called once per frame
    void Update ( )
    {
        //if (Input.GetKey(KeyCode.A))
        //{
        //    if (activePipeline <= 5)
        //        firstPipeline.GetComponent<Rigidbody>().AddForce(firstPipeline.transform.right * force * activePipeline , ForceMode.VelocityChange);
        //    else
        //        firstPipeline.GetComponent<Rigidbody>().velocity = firstPipeline.transform.right*force*6;
        //}
        //if (firstPipeline)
        //    Debug.DrawRay(firstPipeline.transform.position , firstPipeline.transform.right *force, Color.red);


        if (Input.GetKeyDown(KeyCode.D))
        {          
            Play();
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            Stop();
        }

        if(isPlay)
        {
            if (activePipeline <= 3)
            {
                firstPipeline.GetComponent<Rigidbody>().AddForce(firstPipeline.transform.right * force * activePipeline , ForceMode.VelocityChange);
                //firstPipeline.transform.Translate(firstPipeline.transform.right*force*activePipeline) ;
            }
            else 
            {
                firstPipeline.GetComponent<Rigidbody>().velocity = firstPipeline.transform.right * force * 6;
            }
        }
    }

    public void Play()
    {
        //如果动画还在播放，那么不能进入

        if (isPlay)
        {
            //Debug.Log("当前动画未播放完毕或者已经处于播放状态");
            //StartCoroutine(PlayCoroutline());
            return;
        }


            insTrigger.SetTriggerActive(true);
            GetNewPipeline();
            isPlay = true;  
    }

    IEnumerator PlayCoroutline()
    {
        yield return new WaitForSeconds(2.0f);
        if(!isPlay)
        {
            insTrigger.SetTriggerActive(true);
            GetNewPipeline();
            isPlay = true;
        }     
    }


    public void Stop()
    {
        if (isStop == false && isPlay== true)
        {
            isStop = true;
            insTrigger.SetTriggerActive(false);
            firstPipeline.GetComponent<Pipeline>().SetDestroyAll(true);
            force *= 4.0f;
        }
    }

    void ConnectPipeline ( )
    {
        firstPipeline.AddComponent<HingeJoint>();
        firstPipeline.GetComponent<HingeJoint>().connectedBody = connectedPipeline.GetComponent<Rigidbody>();
        firstPipeline.GetComponent<HingeJoint>().anchor = new Vector3(-8.4f , 0 , 0);
        firstPipeline.GetComponent<HingeJoint>().axis = new Vector3(0 , 1 , 0);
        firstPipeline.GetComponent<HingeJoint>().autoConfigureConnectedAnchor = false;
        firstPipeline.GetComponent<HingeJoint>().connectedAnchor = new Vector3(8.7f , 0 , 0);
        firstPipeline.GetComponent<HingeJoint>().useLimits = true;

        JointLimits jl = new JointLimits();
        jl.min = 0;
        jl.max = 5;

        firstPipeline.GetComponent<Rigidbody>().useGravity = true;
        firstPipeline.GetComponent<Rigidbody>().isKinematic = false;
        firstPipeline.GetComponent<HingeJoint>().limits = jl;
        firstPipeline = connectedPipeline;

        firstPipeline.GetComponent<Rigidbody>().useGravity = true;
        firstPipeline.GetComponent<Rigidbody>().isKinematic = false;

    }

    public void GetNewPipeline ( )
    {
        if (activePipeline >= 10)
            activePipeline = 10;
      
            for (int i=0 ;i<=pipelineLists.Count-1 ;i++)
            {
                if(pipelineLists[i].GetComponent<Pipeline>().GetIdleState())
                {
                    connectedPipeline = pipelineLists[i];
                    connectedPipeline.gameObject.SetActive(true);
                    connectedPipeline.GetComponent<Rigidbody>().isKinematic = false;
                    connectedPipeline.GetComponent<Pipeline>().SetIdleState(false);
                    connectedPipeline.GetComponent<Pipeline>().SetDestroyAll(false);
                    connectedPipeline.transform.position = instationPosition.transform.position;
                    connectedPipeline.transform.eulerAngles = instationPosition.transform.eulerAngles;
                    activePipeline++;

                
                    break;
                }
            }


        if (activePipeline >= 2)
        {
            ConnectPipeline();
        }
        else
        {
            firstPipeline = connectedPipeline;
        }
    }

    public void SetDefaultPipeline(GameObject p)
    {
        p.SetActive(false);
        p.GetComponent<Rigidbody>().isKinematic = true;
        Destroy(p.GetComponent<HingeJoint>());
        activePipeline--;
    }

    

    public void DestroyAllPipeline()
    {
        firstPipeline = null;
        connectedPipeline = null;
        isPlay = false;
        force /= 4.0f;
        isStop = false;
        //Debug.Log("管道初始化完毕");
    }
}

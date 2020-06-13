using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using MTFrame.MTEvent;
using System;
using DG.Tweening;

public class WaitPanel : BasePanel
{
    public static WaitPanel Instance;

    public Slider slider;
    public CanvasGroup sliderCanvas;

    private bool IsComplete;

    public Animator animator;

    private WaitPanel2 panel2;

    //切换场景时设置这两个，然后SceneLoadAsync即可
    public SceneName sceneName;
    public PanelName panelName;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();
        panel2 = UIManager.GetPanel<WaitPanel2>(WindowTypeEnum.World);
    }

    public override void InitFind()
    {
        base.InitFind();
        slider = FindTool.FindChildComponent<Slider>(transform, "sliderGroup/Slider");
        sliderCanvas = FindTool.FindChildComponent<CanvasGroup>(transform, "sliderGroup");
        animator = FindTool.FindChildComponent<Animator>(transform, "StartAnima");
    }

    public override void Open()
    {
        base.Open();
        Reset();

        InvokeRepeating("StartAnima",0,7.0f);
        

    }

    private void StartAnima()
    {
        animator.gameObject.GetComponent<CanvasGroup>().alpha = 1;
        animator.SetTrigger("IsOpen");
    }

    public override void Hide()
    {
        base.Hide();
        CancelInvoke("StartAnima");
    }

    /// <summary>
    /// 设置切换的场景名称和UIPanel名称
    /// </summary>
    /// <param name="scene">场景名</param>
    /// <param name="panel">UIPanel名称</param>
    public void SetName(SceneName scene,PanelName panel)
    {
        sceneName = scene;
        panelName = panel;
    }

    public void SceneLoadAsync()
    {
        CancelInvoke("StartAnima");
        Main.Instance.MainCamera.gameObject.SetActive(true);
        sliderCanvas.alpha = 1;
        SceneManager.LoadSceneAsync(sceneName.ToString(), MTFrame.MTScene.LoadingModeType.UnityLocal,
    () => { StartCoroutine(LoadingSlide()); GC.Collect();/*StartCoroutine(panel2.LoadingSlide()); */}, null, () => { IsComplete = true; /*panel2.IsComplete = true;*/ });
    }

    private void Reset()
    {
        slider.value = 0;
        sliderCanvas.alpha = 0;
        //text.text = "0%";
        IsComplete = false;
        
    }

    IEnumerator LoadingSlide()
    {

        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            if (slider.value <= 0.98)
            {
                slider.value += 0.01f;
                //text.text = ((int)(slider.value * 100)).ToString() + "%";

            }

            if (IsComplete)
            {
                LoadingComplete();
                yield break;
            }
            else if (slider.value >= 0.98f && IsComplete)
            {
                LoadingComplete();
                yield break;
            }
        }
    }

    private void LoadingComplete()
    {
        StopCoroutine(LoadingSlide());
        //text.text = "100%";
        slider.value = 1;
        TimeTool.Instance.AddDelayed(TimeDownType.NoUnityTimeLineImpact, 2.0f, () => {
            Main.Instance.MainCamera.gameObject.SetActive(false);
            UdpSeverLink.Instance.PanelChange(panelName);
            //Debug.Log("Complete==" + panelName.ToString());
            Hide();
        });
    }
}

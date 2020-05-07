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
    public AnimationClip[] clips;

    //切换场景时设置这两个，然后切换到LoadingPanel页面即可
    public SceneName sceneName;
    public PanelName panelName;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    public override void InitFind()
    {
        base.InitFind();
        slider = FindTool.FindChildComponent<Slider>(transform, "Slider");
        sliderCanvas = FindTool.FindChildComponent<CanvasGroup>(transform, "Slider");
        animator = FindTool.FindChildComponent<Animator>(transform, "StartAnima");
        clips = animator.runtimeAnimatorController.animationClips;

        //AnimationEvent animationEvent = new AnimationEvent();
        //animationEvent.functionName = "HideAnima";
        //animationEvent.time = clips[0].length;
        //animationEvent.floatParameter = 1;
        //clips[0].AddEvent(animationEvent);
    }

    public void HideAnima(float aa)
    {
        animator.gameObject.GetComponent<CanvasGroup>().alpha = 0;
    }

    public override void Open()
    {
        base.Open();
        Reset();
        animator.SetTrigger("IsOpen");
        //animator.gameObject.GetComponent<CanvasGroup>().DOFade(1, 0.5f);

    }

    public override void Hide()
    {
        base.Hide();
        
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
        Main.Instance.MainCamera.gameObject.SetActive(true);
        sliderCanvas.alpha = 1;
        SceneManager.LoadSceneAsync(sceneName.ToString(), MTFrame.MTScene.LoadingModeType.UnityLocal,
    () => { StartCoroutine(LoadingSlide()); GC.Collect(); }, null, () => { IsComplete = true; });
    }

    private void Reset()
    {
        slider.value = 0;
        sliderCanvas.alpha = 0;
        //text.text = "0%";
        IsComplete = false;
        animator.gameObject.GetComponent<CanvasGroup>().alpha = 1;
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
                Debug.Log("读条完成");
                yield break;
            }
            else if (slider.value >= 0.98f && IsComplete)
            {
                LoadingComplete();
                Debug.Log("读条完成");
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
            Debug.Log("Complete==" + panelName.ToString());
            Hide();
        });
    }
}

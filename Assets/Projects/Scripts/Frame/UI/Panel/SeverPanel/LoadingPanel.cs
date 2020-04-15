using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using System;

public class LoadingPanel : BasePanel
{
    public static LoadingPanel Instance;

    public Slider slider;

    public Text text;

    private bool IsComplete;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    public override void InitFind()
    {
        base.InitFind();
        slider = FindTool.FindChildComponent<Slider>(transform, "Slider");
        text = FindTool.FindChildComponent<Text>(transform, "ValueText");
    }

    public override void Open()
    {
        base.Open();
        Reset();
        Main.Instance.MainCamera.gameObject.SetActive(true);
        SceneManager.LoadSceneAsync(WaitPanel.Instance.sceneName.ToString(), MTFrame.MTScene.LoadingModeType.UnityLocal,
    () => { StartCoroutine(LoadingSlide()); }, null, () => { IsComplete = true; });
    }

    private void Reset()
    {
        slider.value = 0;
        text.text = "0%";
        IsComplete = false;     
    }

    IEnumerator LoadingSlide()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            if(slider.value <= 0.98)
            {
                slider.value += 0.01f;
                text.text = ((int)(slider.value * 100)).ToString() + "%";
                
            }

            if (slider.value >= 0.98f && IsComplete)
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
        text.text = "100%";
        slider.value = 1;
        TimeTool.Instance.AddDelayed(TimeDownType.NoUnityTimeLineImpact, 2.0f, ()=> {
            Main.Instance.MainCamera.gameObject.SetActive(false);
            UdpSeverLink.Instance.PanelChange(WaitPanel.Instance.panelName);
            if(WaitPanel.Instance.panelName == PanelName.WaitPanel)
            {
                Main.Instance.MainCamera.gameObject.SetActive(true);
            }
        });
        Debug.Log("读条完成！");
    }
}

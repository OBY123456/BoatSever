using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;

public class WaitPanel : BasePanel
{
    public static WaitPanel Instance;

    public Button button;

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
        //button = FindTool.FindChildComponent<Button>(transform, "Image");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        //button.onClick.AddListener(() => {
        //    SetName(SceneName.PirateCove1, PanelName.DisplayPanel);
        //    UdpSeverLink.Instance.PanelChange(PanelName.LoadingPanel);
        //});
    }

    public override void Open()
    {
        base.Open();
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
}

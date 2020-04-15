using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;

public class DisplayPanel : BasePanel
{
    public Button button;

    public override void InitFind()
    {
        base.InitFind();
        button = FindTool.FindChildComponent<Button>(transform, "bg");
    }

    public override void InitEvent()
    {
        base.InitEvent();
        button.onClick.AddListener(() => {

            WaitPanel.Instance.SetName(SceneName.WaitScene, PanelName.WaitPanel);
            UdpSeverLink.Instance.PanelChange(PanelName.LoadingPanel);
        });
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Hide()
    {
        base.Hide();
    }
}

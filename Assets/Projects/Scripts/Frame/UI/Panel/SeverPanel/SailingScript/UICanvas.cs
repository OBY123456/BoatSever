using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using DG.Tweening;

public class UICanvas : BasePanel
{

    public static UICanvas Instance;
    //public RectTransform View1,View2;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();
        //ViewOpen();
    }

    public override void InitFind()
    {
        base.InitFind();
        //View1 = FindTool.FindChildComponent<RectTransform>(transform, "ViewGroup/View1");
        //View2 = FindTool.FindChildComponent<RectTransform>(transform, "ViewGroup/View2");

    }

    //public void ViewOpen()
    //{
    //    View1.DOSizeDelta(new Vector2(492, 270), 0.5f);
    //    View2.DOSizeDelta(new Vector2(492,270), 0.5f);
    //}

    //public void ViewHide()
    //{
    //    View1.DOSizeDelta(new Vector2(492, 0), 0.5f);
    //    View2.DOSizeDelta(new Vector2(492, 0), 0.5f);
    //}
}

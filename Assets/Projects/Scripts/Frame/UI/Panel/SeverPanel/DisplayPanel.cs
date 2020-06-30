using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using System;
using MTFrame.MTEvent;
using Newtonsoft.Json;
using DG.Tweening;
using RenderHeads.Media.AVProVideo;

public class DisplayPanel : BasePanel
{
    public MediaPlayer[] VideoGroup;
    private MediaPlayer CurrentVideo;
    //private Vector2 MediaRect = new Vector2(1333.4f, 710f);
    private Vector2 MediaRect = new Vector2(2676f, 1585f);
    private Vector2 InitialPosition;
    private Vector2 InitialSize = new Vector2(424f, 338.6666f);
    private float AnimationTime = 0.8f;

    private bool IsAnimation;
    private float ForwordTime = 1.0f;

    public Animation[] animations;

    private string[] VideoPath = {
        "Video/起重机.mp4",
        "Video/J形导管.mp4",
        "Video/S形导管.mp4",
        "Video/螺旋桨.mp4",
        "Video/发电机.mp4",
    };

    protected override void Start()
    {
        base.Start();
        Reset();
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.Display_PlayVideo.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.Display_VideoControl.ToString(), Callback);
    }

    public override void InitFind()
    {
        base.InitFind();
        VideoGroup = FindTool.FindChildNode(transform, "VideoGroup").GetComponentsInChildren<MediaPlayer>();

        animations = FindTool.FindChildNode(transform, "ImageGroup").GetComponentsInChildren<Animation>();
    }

    public override void InitEvent()
    {
        base.InitEvent();
    }

    private void Callback(EventParamete parameteData)
    {
        if(parameteData.EvendName == ParmaterCodes.Display_PlayVideo.ToString())
        {
            string index = parameteData.GetParameter<string>()[0];
            Display_PlayVideo display_PlayVideo = new Display_PlayVideo();
            display_PlayVideo = JsonConvert.DeserializeObject<Display_PlayVideo>(index);
            VideoName name = (VideoName)Enum.Parse(typeof(VideoName), display_PlayVideo.name);
            switch (name)
            {
                case VideoName.起吊系统:
                    ImageOpenAnimation(VideoGroup[0], VideoPath[0]);
                    break;
                case VideoName.J型铺管:
                    ImageOpenAnimation(VideoGroup[1], VideoPath[1]);
                    break;
                case VideoName.S型铺管:
                    ImageOpenAnimation(VideoGroup[2], VideoPath[2]);
                    break;
                case VideoName.推进器系统:
                    ImageOpenAnimation(VideoGroup[3], VideoPath[3]);
                    break;
                case VideoName.动力系统:
                    ImageOpenAnimation(VideoGroup[4], VideoPath[4]);
                    break;
                case VideoName.结束:
                    ImageHideAnimation();
                    break;
                default:
                    break;
            }
        }

        if (parameteData.EvendName == ParmaterCodes.Display_VideoControl.ToString())
        {
            string index = parameteData.GetParameter<string>()[0];
            Display_VideoControl display_PlayVideo = new Display_VideoControl();
            display_PlayVideo = JsonConvert.DeserializeObject<Display_VideoControl>(index);
            VideoControl state = (VideoControl)Enum.Parse(typeof(VideoControl), display_PlayVideo.state);
            switch (state)
            {
                case VideoControl.快进:
                    VideoSeek(ForwordTime);
                    break;
                case VideoControl.快退:
                    VideoSeek(-ForwordTime);
                    break;
                case VideoControl.暂停:
                    if(CurrentVideo != null && CurrentVideo.Control.IsPlaying())
                    {
                        CurrentVideo.Pause();
                    }
                    break;
                case VideoControl.播放:
                    if (CurrentVideo != null && !CurrentVideo.Control.IsPlaying())
                    {
                        CurrentVideo.Play();
                    }
                    break;
                case VideoControl.重播:
                    CurrentVideo.Rewind(false);
                    break;
                default:
                    break;
            }
        }

    }

    private void VideoSeek(float time)
    {
        if(CurrentVideo!=null)
        {
            float temp = CurrentVideo.Control.GetCurrentTimeMs() + time * 1000;
            if (temp > CurrentVideo.Info.GetDurationMs())
            {
                temp = 0;
            }

            if (temp < 0)
            {
                temp = 0;
            }
            CurrentVideo.Control.Seek(temp);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        TimeTool.Instance.Remove(TimeDownType.NoUnityTimeLineImpact, AnimationClose);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.Display_PlayVideo.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.Display_VideoControl.ToString(), Callback);
        if (CurrentVideo != null)
            ImageHideAnimation();
    }

    /// <summary>
    /// 图片动效
    /// </summary>
    /// <param name="image"></param>
    private void ImageOpenAnimation(MediaPlayer image,string VideoPath)
    {
        if(CurrentVideo!=null)
        {
            if(CurrentVideo.name == image.name)
            {
                //Debug.Log(CurrentVideo.name);
                return;
            }
        }

        //BoatControl.Instance.Boat.SetActive(false);
        IsAnimation = true;
        RectTransform rect = image.gameObject.GetComponent<RectTransform>();
        CanvasGroup canvas = image.gameObject.GetComponent<CanvasGroup>();
        canvas.alpha = 1;


        //第一次播
        if (CurrentVideo == null)
        {
            CurrentVideo = image;
            InitialPosition = rect.anchoredPosition;
        }
        else
        {
            //不是第一次播，需要复原上一个
            CurrentVideo.gameObject.GetComponent<RectTransform>().anchoredPosition = InitialPosition;
            CurrentVideo.gameObject.GetComponent<RectTransform>().sizeDelta = InitialSize;
            CurrentVideo.gameObject.GetComponent<CanvasGroup>().alpha = 0;
            CurrentVideo.Stop();

            CurrentVideo = image;
            InitialPosition = rect.anchoredPosition;
        }

        image.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, VideoPath);

        rect.DOAnchorPos(Vector2.zero, AnimationTime).SetEase(Ease.InExpo);
        rect.DOSizeDelta(MediaRect, AnimationTime).SetEase(Ease.InExpo).OnComplete(()=> {
            
            IsAnimation = false;
        });

    }

    private void ImageHideAnimation()
    {
        IsAnimation = true;

        //BoatControl.Instance.Boat.SetActive(true);

        if (CurrentVideo == null)
        {
            IsAnimation = false;
            return;
        }
        else
        {
            CurrentVideo.Stop();

            RectTransform rect = CurrentVideo.gameObject.GetComponent<RectTransform>();
            CanvasGroup canvas = CurrentVideo.gameObject.GetComponent<CanvasGroup>();

            canvas.DOFade(0, AnimationTime);

            rect.DOAnchorPos(InitialPosition, AnimationTime);
            rect.DOSizeDelta(InitialSize, AnimationTime).OnComplete(() => {

                CurrentVideo = null;
                InitialPosition = Vector2.zero;
                IsAnimation = false;
            });
        }
    }

    private void AnimationOpen()
    {
        foreach (Animation item in animations)
        {
            item.Play();
        }
    }

    private void AnimationClose()
    {
        foreach (Animation item in animations)
        {
            item.Stop();
            item.gameObject.GetComponent<Image>().color = Color.white;
        }
    }

    private void Reset()
    {
        AnimationOpen();
        TimeTool.Instance.Remove(TimeDownType.NoUnityTimeLineImpact, AnimationClose);
        TimeTool.Instance.AddDelayed(TimeDownType.NoUnityTimeLineImpact, 5.0f, AnimationClose);

        CurrentVideo = null;
        InitialPosition = Vector2.zero;
        
    }
}

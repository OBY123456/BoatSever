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
    public MediaPlayer mediaPlayer;
    public Image[] ImageGroup;
    private Image CurrentImage;
    public RectTransform MediaRectTransform;
    public CanvasGroup MediaCanvasGroup;

    private Vector2 InitialPosition;
    private Vector2 InitialSize = new Vector2(206, 164);
    private float AnimationTime = 0.5f;

    private bool IsAnimation;

    public float VideoLenth;
    public float VideoCurrentTime;
    private float ForwordTime = 1.0f;

    public override void InitFind()
    {
        base.InitFind();
        mediaPlayer = FindTool.FindChildComponent<MediaPlayer>(transform, "VideoPlayer");
        ImageGroup = FindTool.FindChildNode(transform, "ImageGroup").GetComponentsInChildren<Image>();
        MediaRectTransform = mediaPlayer.gameObject.GetComponent<RectTransform>();
        MediaCanvasGroup = mediaPlayer.gameObject.GetComponent<CanvasGroup>();
    }

    public override void InitEvent()
    {
        base.InitEvent();
    }

    public override void Open()
    {
        base.Open();
        Reset();
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.Display_PlayVideo.ToString(), Callback);
        EventManager.AddListener(GenericEventEnumType.Message, ParmaterCodes.Display_VideoControl.ToString(), Callback);
        EventManager.AddUpdateListener(UpdateEventEnumType.Update, "DisplayUpdate", DisplayUpdate);
    }

    private void DisplayUpdate(float timeProcess)
    {
        VideoLenth = (int)(mediaPlayer.Info.GetDurationMs() / 1000);
        VideoCurrentTime = (int)(mediaPlayer.Control.GetCurrentTimeMs() / 1000);
    }

    private void Callback(EventParamete parameteData)
    {
        Debug.Log("222");
        if(parameteData.EvendName == ParmaterCodes.Display_PlayVideo.ToString())
        {
            Debug.Log("000");
            string index = parameteData.GetParameter<string>()[0];
            Display_PlayVideo display_PlayVideo = new Display_PlayVideo();
            display_PlayVideo = JsonConvert.DeserializeObject<Display_PlayVideo>(index);
            VideoName name = (VideoName)Enum.Parse(typeof(VideoName), display_PlayVideo.name);
            switch (name)
            {
                case VideoName.起吊系统:
                    ImageOpenAnimation(ImageGroup[0]);
                    break;
                case VideoName.J型铺管:
                    ImageOpenAnimation(ImageGroup[1]);
                    break;
                case VideoName.S型铺管:
                    ImageOpenAnimation(ImageGroup[2]);
                    break;
                case VideoName.推进器系统:
                    ImageOpenAnimation(ImageGroup[3]);
                    break;
                case VideoName.动力系统:
                    ImageOpenAnimation(ImageGroup[4]);
                    break;
                case VideoName.结束:
                    
                    ImageHideAnimation();
                    break;
                default:
                    break;
            }
            Debug.Log("1111");
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
                    if(mediaPlayer.Control.IsPlaying() && MediaCanvasGroup.alpha ==1)
                    {
                        mediaPlayer.Pause();
                    }
                    break;
                case VideoControl.播放:
                    if (!mediaPlayer.Control.IsPlaying() && MediaCanvasGroup.alpha == 1)
                    {
                        mediaPlayer.Play();
                    }
                    break;
                default:
                    break;
            }
        }

    }

    private void VideoSeek(float time)
    {
        if(MediaCanvasGroup.alpha > 0)
        {
            float temp = mediaPlayer.Control.GetCurrentTimeMs() + time * 1000;
            if (temp > mediaPlayer.Info.GetDurationMs())
            {
                temp = mediaPlayer.Info.GetDurationMs();
            }

            if (temp < 0)
            {
                temp = 0;
            }
            mediaPlayer.Control.Seek(temp);
        }
    }

    public override void Hide()
    {
        base.Hide();
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.Display_PlayVideo.ToString(), Callback);
        EventManager.RemoveListener(GenericEventEnumType.Message, ParmaterCodes.Display_VideoControl.ToString(), Callback);
        EventManager.RemoveUpdateListener(UpdateEventEnumType.Update, "DisplayUpdate", DisplayUpdate);
        mediaPlayer.Stop();
    }

    /// <summary>
    /// 图片动效
    /// </summary>
    /// <param name="image"></param>
    private void ImageOpenAnimation(Image image)
    {
        IsAnimation = true;
        RectTransform rect = image.gameObject.GetComponent<RectTransform>();
        CanvasGroup canvas = image.gameObject.GetComponent<CanvasGroup>();

        if(mediaPlayer.Control.IsPlaying())
        {
            mediaPlayer.Stop();
            MediaCanvasGroup.alpha = 0;
        }

        //第一次播
        if(CurrentImage == null)
        {
            CurrentImage = image;
            InitialPosition = rect.anchoredPosition;
        }
        else
        {
            //不是第一次播，需要复原上一个
            CurrentImage.gameObject.GetComponent<RectTransform>().anchoredPosition = InitialPosition;
            CurrentImage.gameObject.GetComponent<RectTransform>().sizeDelta = InitialSize;


            CurrentImage = image;
            InitialPosition = rect.anchoredPosition;
        }

        canvas.alpha = 1;
        canvas.DOFade(0.3f,0.15f* AnimationTime).OnComplete(()=> {
            canvas.DOFade(1.0f, 0.25f * AnimationTime).OnComplete(() => {
                canvas.DOFade(0.5f, 0.15f * AnimationTime).OnComplete(() => {
                    canvas.DOFade(1.0f, 0.1f * AnimationTime).OnComplete(() => {
                        canvas.DOFade(1, 0.35f * AnimationTime).OnComplete(() =>
                        {
                            canvas.DOFade(0, 0.3f);
                        });
                    });
                });
            });
        });
        rect.DOAnchorPos(Vector2.zero, AnimationTime);
        rect.DOSizeDelta(MediaRectTransform.sizeDelta, AnimationTime).OnComplete(()=> {
            mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, "AVProVideoSamples/BigBuckBunny_720p30.mp4");
            MediaCanvasGroup.alpha = 1;
            IsAnimation = false;
        });

    }

    private void ImageHideAnimation()
    {
        IsAnimation = true;
        mediaPlayer.Stop();
        MediaCanvasGroup.alpha = 0;

        if (mediaPlayer.Control.IsPlaying())
        {
            mediaPlayer.Stop();
            MediaCanvasGroup.alpha = 0;
        }

        if (CurrentImage == null)
        {
            IsAnimation = false;
            return;
        }
        else
        {
            RectTransform rect = CurrentImage.gameObject.GetComponent<RectTransform>();
            CanvasGroup canvas = CurrentImage.gameObject.GetComponent<CanvasGroup>();
            canvas.alpha = 1;
            
            rect.DOAnchorPos(InitialPosition, AnimationTime);
            rect.DOSizeDelta(InitialSize, AnimationTime).OnComplete(() => {
                canvas.alpha = 0;
                CurrentImage = null;
                InitialPosition = Vector2.zero;
                IsAnimation = false;
            });
        }
    }

    private void Reset()
    {
        if(CurrentImage != null)
        {
            RectTransform rect = CurrentImage.gameObject.GetComponent<RectTransform>();
            rect.anchoredPosition = InitialPosition;
            rect.sizeDelta = InitialSize;
        }

        CurrentImage = null;
        InitialPosition = Vector2.zero;
        MediaCanvasGroup.alpha = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using System;
using MTFrame.MTEvent;
using Newtonsoft.Json;
using DG.Tweening;

public class DisplayPanel : BasePanel
{
    public GameObject Boat;

    private BoatRotateY rotateData = new BoatRotateY();

    protected override void Start()
    {
        base.Start();
        EventManager.AddListener(MTFrame.MTEvent.GenericEventEnumType.Message, ParmaterCodes.BoatRotateY.ToString(), Callback);
        EventManager.AddListener(MTFrame.MTEvent.GenericEventEnumType.Message, ParmaterCodes.BoatRotate.ToString(), Callback);
    }

    public override void InitFind()
    {
        base.InitFind();
    }

    public override void InitEvent()
    {
        base.InitEvent();
    }

    private void Callback(EventParamete parameteData)
    {
        if(parameteData.EvendName == ParmaterCodes.BoatRotateY.ToString())
        {
            rotateData = JsonConvert.DeserializeObject<BoatRotateY>(parameteData.GetParameter<string>()[0]);
            //Vector3 temp = new Vector3(0, rotateData.Y, 0);
            Boat.transform.localEulerAngles = new Vector3(-90, rotateData.y, 0);
        }

        if (parameteData.EvendName == ParmaterCodes.BoatRotate.ToString())
        {
            BoatRotate rotate = JsonConvert.DeserializeObject<BoatRotate>(parameteData.GetParameter<string>()[0]);
            Vector3 temp = new Vector3(rotate.X, rotate.Y, rotate.Z);
            //Boat.transform.localEulerAngles = Vector3.Lerp(Boat.transform.localEulerAngles, temp, 0.5f);
            Boat.transform.localEulerAngles = temp;
        }
    }



    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.RemoveListener(MTFrame.MTEvent.GenericEventEnumType.Message, ParmaterCodes.BoatRotateY.ToString(), Callback);
        EventManager.RemoveListener(MTFrame.MTEvent.GenericEventEnumType.Message, ParmaterCodes.BoatRotate.ToString(), Callback);
    }
}

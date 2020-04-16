using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;
using UnityEngine.UI;
using System;
using MTFrame.MTEvent;
using Newtonsoft.Json;

public class DisplayPanel : BasePanel
{
    public GameObject Cube;

    private BoatRotateX rotateDataX = new BoatRotateX();
    private BoatRotateY rotateDataY = new BoatRotateY();
    private BoatRotateZ rotateDataZ = new BoatRotateZ();

    public override void InitFind()
    {
        base.InitFind();
    }

    public override void InitEvent()
    {
        base.InitEvent();
    }

    public override void Open()
    {
        base.Open();
        EventManager.AddListener(MTFrame.MTEvent.GenericEventEnumType.Message, ParmaterCodes.BoatRotateX.ToString(), Callback);
    }

    private void Callback(EventParamete parameteData)
    {
        if(parameteData.EvendName == ParmaterCodes.BoatRotateX.ToString())
        {
            rotateDataX = JsonConvert.DeserializeObject<BoatRotateX>(parameteData.GetParameter<string>()[0]);
            Cube.transform.localEulerAngles = new Vector3(rotateDataX.X, Cube.transform.localEulerAngles.y, Cube.transform.localEulerAngles.z);
        }

        if (parameteData.EvendName == ParmaterCodes.BoatRotateY.ToString())
        {
            rotateDataY = JsonConvert.DeserializeObject<BoatRotateY>(parameteData.GetParameter<string>()[0]);
            Cube.transform.localEulerAngles = new Vector3(Cube.transform.localEulerAngles.x, rotateDataY.Y, Cube.transform.localEulerAngles.z);
        }

        if (parameteData.EvendName == ParmaterCodes.BoatRotateZ.ToString())
        {
            rotateDataZ = JsonConvert.DeserializeObject<BoatRotateZ>(parameteData.GetParameter<string>()[0]);
            Cube.transform.localEulerAngles = new Vector3(Cube.transform.localEulerAngles.x, Cube.transform.localEulerAngles.y, rotateDataZ.Z);
        }
    }

    public override void Hide()
    {
        base.Hide();
        EventManager.RemoveListener(MTFrame.MTEvent.GenericEventEnumType.Message, ParmaterCodes.BoatRotateX.ToString(), Callback);
    }
}

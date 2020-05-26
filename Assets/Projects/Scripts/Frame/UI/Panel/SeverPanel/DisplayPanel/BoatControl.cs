using MTFrame;
using MTFrame.MTEvent;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatControl : MonoBehaviour
{
    public static BoatControl Instance;
    public GameObject Boat;

    [Range(0, 360)]
    public float RotateY = 0;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddListener(MTFrame.MTEvent.GenericEventEnumType.Message, ParmaterCodes.BoatRotate.ToString(), Callback);
    }

    // Update is called once per frame
    void Update()
    {
       // SetTargetRotate(RotateY);
    }

    private void Callback(EventParamete parameteData)
    {
        if (parameteData.EvendName == ParmaterCodes.BoatRotate.ToString())
        {
            BoatRotate rotate = JsonConvert.DeserializeObject<BoatRotate>(parameteData.GetParameter<string>()[0]);
            //Vector3 temp = new Vector3(rotate.X, rotate.Y, rotate.Z);
            //Boat.transform.localEulerAngles = Vector3.Lerp(Boat.transform.localEulerAngles, temp, 0.5f);
            SetTargetRotate(rotate.Y);
        }
    }

    public void SetTargetRotate(float value)
    {
        Boat.transform.localEulerAngles = Vector3.up * value;
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(MTFrame.MTEvent.GenericEventEnumType.Message, ParmaterCodes.BoatRotate.ToString(), Callback);
    }
}

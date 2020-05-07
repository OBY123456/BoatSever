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
        
    }

    private void Callback(EventParamete parameteData)
    {
        if (parameteData.EvendName == ParmaterCodes.BoatRotate.ToString())
        {
            BoatRotate rotate = JsonConvert.DeserializeObject<BoatRotate>(parameteData.GetParameter<string>()[0]);
            Vector3 temp = new Vector3(rotate.X, rotate.Y, rotate.Z);
            //Boat.transform.localEulerAngles = Vector3.Lerp(Boat.transform.localEulerAngles, temp, 0.5f);
            Boat.transform.localEulerAngles = temp;
        }
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(MTFrame.MTEvent.GenericEventEnumType.Message, ParmaterCodes.BoatRotate.ToString(), Callback);
    }
}

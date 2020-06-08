using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public Display6FPPanel[] display6FPPanels;
    private Rigidbody BoatRb;
    private Transform Boat;
    // Start is called before the first frame update
    void Start()
    {
        BoatRb = SailingSceneManage.Instance.boatProbes.gameObject.GetComponent<Rigidbody>();
        Boat = BoatRb.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(display6FPPanels != null && BoatRb!=null)
        {
            display6FPPanels[0].SetRateOfTurnValue(BoatRb.angularVelocity.y * 1000);
            display6FPPanels[0].SetGyroValue(Boat.eulerAngles.y);
            display6FPPanels[0].MagnText.text = (Boat.eulerAngles.y + Random.Range(-2f, 2f)).ToString("#0.0");
            display6FPPanels[0].SetRPMValue(80 + Random.Range(-2, 2));
            display6FPPanels[0].SetDepthValue(4500 + Boat.transform.position.y);
            //display6FPPanels[0].SetRelWindValue(WeatherManager.Instance.Wind_Rotate);
            display6FPPanels[0].SetRollValue((Boat.eulerAngles.y - 360)/360 * 20);
            display6FPPanels[0].SetLogValue(BoatRb.angularVelocity.x *1000, BoatRb.angularVelocity.y * 1000, BoatRb.angularVelocity.x *1000);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{

    public RectTransform map;
    public RectTransform map_Boat, map_Target;

    public Transform Boat;
    public Transform RouteImageGroup;

    //海域范围
    public float Height;
    public float Weight;

    private GameObject Target;

    public Image RouteImage;


    // Start is called before the first frame update
    void Start()
    {
        RouteImage = Resources.Load<Image>("Sailing/RouteImage");
        Target = SailingSceneManage.Instance.Target;
    }

    // Update is called once per frame
    void Update()
    {
        Set_MapBoat_Position();
        Set_MapTarget_Position();
    }

    private void Set_MapBoat_Position()
    {
        Vector2 temp = new Vector2(Boat.position.x / Weight * map.rect.width, Boat.position.z / Height * map.rect.height);
        map_Boat.anchoredPosition = temp;
    }

    private void Set_MapTarget_Position()
    {
        Vector2 temp = new Vector2(Target.transform.position.x / Weight * map.rect.width, Target.transform.position.z /Height * map.rect.height);
        map_Target.anchoredPosition = temp;
    }

    public void Creat_RouteImage(Vector2 vector2)
    {
        Image img = Instantiate(RouteImage);

        img.transform.SetParent(RouteImageGroup);
        img.GetComponent<RectTransform>().anchoredPosition = vector2;
    }
}

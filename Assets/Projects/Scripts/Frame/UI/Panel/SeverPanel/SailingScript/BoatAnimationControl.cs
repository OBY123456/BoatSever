﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoatAnimationControl : MonoBehaviour
{
    public static BoatAnimationControl Instance;
    //升降杆
    //public GameObject Engine_Liftinglever;

    //旋转轴
    public GameObject[] Engine_Shaft;

    //螺旋桨
    public GameObject[] Engine_Propeller;

    //public ParticleSystem SmokeParticle;

    //public Material[] SmokeMaterials;
    [HideInInspector]
    public bool IsRotate;
    private float RotateSpeed_Propeller = 50;

    [Range(0,360)]
    public float RotateSpeed_Engine = 0;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //EngineRotate(RotateSpeed_Engine);

        //if (RotateSpeed_Propeller == 0)
        //{
        //    return;
        //}
        //else
        //{
        //    PropellerRotate();
        //}
        if(IsRotate)
        {
            PropellerRotate();
        }
    }

    private void PropellerRotate()
    {
        Engine_Propeller[0].transform.Rotate(Vector3.right * RotateSpeed_Propeller);
        Engine_Propeller[1].transform.Rotate(Vector3.right * RotateSpeed_Propeller);
        Engine_Propeller[2].transform.Rotate(Vector3.right * RotateSpeed_Propeller);
        Engine_Propeller[3].transform.Rotate(Vector3.right * RotateSpeed_Propeller);
        Engine_Propeller[4].transform.Rotate(Vector3.right * RotateSpeed_Propeller);
        Engine_Propeller[5].transform.Rotate(Vector3.right * RotateSpeed_Propeller);
        Engine_Propeller[6].transform.Rotate(Vector3.right * RotateSpeed_Propeller);
        Engine_Propeller[7].transform.Rotate(Vector3.right * RotateSpeed_Propeller);
    }


    //public void EngineUp()
    //{
    //    Engine_Liftinglever.transform.DOLocalMoveZ(0f, 3.0f);
    //}

    //public void EngineDown()
    //{
    //    Engine_Liftinglever.transform.DOLocalMoveZ(-50.2f, 3.0f);
    //}

    //public void EngineRotate(float value)
    //{
    //    Engine_Shaft.transform.DOLocalRotate(value*Vector3.forward, 3.0f);
    //}

    //private void PropellerRotate()
    //{
    //    Engine_Propeller.transform.Rotate(Vector3.right * RotateSpeed_Propeller);
    //}

    //public void Set_Day_Smoke()
    //{
    //    // SmokeParticle.GetComponent<Renderer>().material = SmokeMaterials[0];
    //    SmokeParticle.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(191 / 255f, 191 / 255f, 191 / 255f, 10 / 255f));
    //}

    //public void Set_Night_Smoke()
    //{
    //    //SmokeParticle.GetComponent<Renderer>().material = SmokeMaterials[1];
    //    SmokeParticle.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(50 / 255f, 50 / 255f, 50 / 255f, 10 / 255f));
    //}
}

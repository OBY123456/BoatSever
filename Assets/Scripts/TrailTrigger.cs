using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailTrigger : MonoBehaviour
{
    ParticleSystem ps;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();//进入粒子
    List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();//离开粒子

    
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleTrigger ( )
    {
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter , enter);
        int numExit = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit , exit);
        ////进入触发器的粒子改变颜色为红色
        for (int i = 0 ; i < numEnter ; i++)
        {
            ParticleSystem.Particle p = enter[i];
            p.startLifetime = 0.0f;
           // p.startColor = new Color32(255 , 0 , 0 , 255);
            enter[i] = p;
        }
        ////退出触发器的粒子改变颜色为绿色
        //for (int i = 0 ; i < numExit ; i++)
        //{
        //    ParticleSystem.Particle p = exit[i];
        //    p.startColor = new Color32(0 , 255 , 0 , 255);
        //    exit[i] = p;
        //}
        ////需要重新为粒子系统设置改版后的粒子
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter , enter);
        //ps.SetTriggerParticles(ParticleSystemTriggerEventType.Exit , exit);
    }
}

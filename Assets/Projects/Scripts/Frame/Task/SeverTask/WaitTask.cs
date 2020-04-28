using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MTFrame;

public class WaitTask : BaseTask
{
    public WaitTask(BaseState state) : base(state)
    {
    }

    public override void Enter()
    {
        base.Enter();
        UIManager.CreatePanel<WaitPanel>(WindowTypeEnum.Screen);
    }

    public override void Exit()
    {
        base.Exit();
        UIManager.ChangePanelState<WaitPanel>(WindowTypeEnum.Screen, UIPanelStateEnum.Hide);
    }
}

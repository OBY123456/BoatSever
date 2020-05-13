using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipeline : MonoBehaviour
{
    [SerializeField]
    bool state = true;

    [SerializeField]
    bool destroyAll = false;
    // Start is called before the first frame update
    
    public bool GetIdleState ( ) { return state; }
    public void SetIdleState(bool newState)
    {
        state = newState;
    }
    public void SetDestroyAll(bool newState)
    {
        destroyAll = newState;
    }

    public bool GetDestroyParament ( ) { return destroyAll; }
}

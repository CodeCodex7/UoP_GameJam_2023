using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FsmState : ScriptableObject, IFsmState
{
    public FSM FsmEngine;
    public GameObject Object;

    public FsmState NextState;
    public FsmState EnteredState;

    public virtual void InState()
    {
        //throw new System.NotImplementedException();
    }

    public virtual void OnEnter()
    {
        //throw new System.NotImplementedException();
    }

    public virtual void OnExit()
    {
        
    }

}

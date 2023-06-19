using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{

    public List<FsmState> fsmStates = new List<FsmState>();
    public List<FsmState> CurrentStates = new List<FsmState>();

    public bool Playing = true;

    // Update is called once per frame
    public virtual void Update()
    {
        if (Playing)
        {
            foreach (var fsm in CurrentStates)
            {
                fsm.InState();
            }
        }
    }

    private void MoveState(FsmState CState,FsmState NState)
    {
        FsmState NextState = fsmStates.Find(delegate (FsmState S) { return S == NState; });

        CurrentStates[0].OnExit();

        for(int i = 0; i < CurrentStates.Count; i++)
        {
            if(CurrentStates[i].Equals(CState))
            {
                CurrentStates.Remove(CState);
                CurrentStates.Add(NextState);
            }
        }

        CurrentStates[0].OnEnter();
    }

    public void Next()
    {
        MoveState(CurrentStates[0], CurrentStates[0].NextState);
    }

}

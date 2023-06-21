using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;
using TMPro;

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
        MoveState(CurrentStates[0], CurrentStates[0].NextStates[0]);
    }

    public void NextState<T> ()
    {
        Type typeParameterType = typeof(T);

        FsmState state = fsmStates.Find(delegate (FsmState S) { if (S.GetType() == typeParameterType) { return S; } else { return false; }; ; });

        try
        {
            //state = fsmStates.Find((x) => x.Equals(typeParameterType));
            MoveState(state, state.NextStates[0]);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        
    }

    public void AddState<T>()
    {
        Type typeParameterType = typeof(T);

        
        try
        {
            //state = fsmStates.Find((x) => x == (typeParameterType));
            FsmState state = fsmStates.Find(delegate (FsmState S) { if (S.GetType() == typeParameterType) { return S; } else { return false; }; ; });
            CurrentStates.Add(state);
            CurrentStates.Find(delegate (FsmState S) { if (S.GetType() == typeParameterType) { return S; } else { return false; }; ; }).OnEnter();

        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

    }

    public void RemoveStates(params FsmState[] state )
    {
        for (int i = 0; i < state.Length; i++)
        {
            CurrentStates.Remove(state[i]);
        }
    }

}

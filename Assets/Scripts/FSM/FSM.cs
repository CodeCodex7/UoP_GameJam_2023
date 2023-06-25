using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using TMPro;

public class FSM : MonoBehaviour
{

    public List<FsmState> fsmStates = new List<FsmState>();
    public List<FsmState> CurrentStates = new List<FsmState>();

    public bool Playing = true;
    public bool Reset = false;

    Action StateChange;

    // Update is called once per frame
    public virtual void Update()
    {
        if (Playing)
        {
            for(int i = 0; i < CurrentStates.Count; i++) {

                CurrentStates[i].InState();  
               
            }

            StateChange();
            //Action.RemoveAll(StateChange, null);
            StateChange = () => { };
        }
    }


    private void MoveState(FsmState CState, FsmState NState)
    {
        FsmState NextState = fsmStates.Find(delegate (FsmState S) { return S == NState; });

        CurrentStates[0].OnExit();

        for (int i = 0; i < CurrentStates.Count; i++)
        {
            if (CurrentStates[i].Equals(CState))
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

    public void NextState<T>()
    {
        Type typeParameterType = typeof(T);

        FsmState state = fsmStates.Find(delegate (FsmState S) { if (S.GetType() == typeParameterType) { return S; } else { return false; }; ; });

        try
        {
            //state = fsmStates.Find((x) => x.Equals(typeParameterType));
            StateChange += () => { MoveState(state, state.NextStates[0]); };

        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        Reset = true;

    }

    public void AddState<T>()
    {
        Type typeParameterType = typeof(T);


        try
        {

            StateChange += () => {
                
                //state = fsmStates.Find((x) => x == (typeParameterType));
                FsmState state = fsmStates.Find(delegate (FsmState S) { if (S.GetType() == typeParameterType) { return S; } else { return false; }; ; });
                CurrentStates.Add(state);
                CurrentStates.Find(delegate (FsmState S) { if (S.GetType() == typeParameterType) { return S; } else { return false; }; ; }).OnEnter();
            };
            

        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        Reset = true;
    }

    public void ExitState<T>()
    {
        Type typeParameterType = typeof(T);

        FsmState state = fsmStates.Find(delegate (FsmState S) { if (S.GetType() == typeParameterType) { return S; } else { return false; }; ; });

        try
        {
            //state = fsmStates.Find((x) => x.Equals(typeParameterType));
            StateChange += state.OnExit;
            StateChange += () => { CurrentStates.Remove(state); };
            
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        Reset = true;
    }

    public void RemoveStates(params FsmState[] state )
    {
        for (int i = 0; i < state.Length; i++)
        {
            StateChange += () => { CurrentStates.Remove(state[i]); };
        }

        Reset = true;
    }

}

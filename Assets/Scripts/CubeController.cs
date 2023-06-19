using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : FSM
{
    public bool Test;

    private void Start()
    {
        Setup();
    }

    public override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.N))
        {
            Next();
        }
    }

    public void Setup()
    {
        CubeLeftRight C1 = new CubeLeftRight(this.gameObject);
        CubeUpDown C2 = new CubeUpDown(this.gameObject);

        C1.NextState = C2;
        C2.NextState = C1;

        fsmStates.Add(C1);
        fsmStates.Add(C2);


        CurrentStates.Add(C1);

    }

    class CubeLeftRight : FsmState
    {
        public CubeLeftRight(GameObject obj)
        {
            Object = obj;
        }

        float Counter = 0;
        bool Direction = false;

        public override void InState()
        {
            base.InState();
            if (Counter > 5 || Counter < -5)
            {
                Direction = !Direction;
            }

            if (Direction)
            {
                Counter += 1 * Time.deltaTime;
                Object.transform.position += Vector3.left * Time.deltaTime;
            }
            else
            {
                Counter -= 1 * Time.deltaTime;
                Object.transform.position += (Vector3.left * Time.deltaTime) * -1;
            }
        }
    }

    class CubeUpDown : FsmState
    {

        public CubeUpDown(GameObject obj)
        {
            Object = obj;
        }

        float Counter = 0;
        bool Direction = false;

        public override void InState()
        {
            base.InState();


            if (Counter > 5 || Counter < -5)
            {
                Direction = !Direction;
            }

            if (Direction)
            {
                Counter += 1 * Time.deltaTime;

                Object.transform.position += Vector3.up * Time.deltaTime;
            }
            else
            {
                Counter -= 1 * Time.deltaTime;

                Object.transform.position += (Vector3.up * Time.deltaTime) * -1;
            }
        }
    }



}
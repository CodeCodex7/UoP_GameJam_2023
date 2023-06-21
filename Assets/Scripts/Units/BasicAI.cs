using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : FSM
{

    public bool Paused;
    public bool Hold;

    public float Speed = 0.5f;
    Vector2Int PosTraker;
    GridCell<PathfindingInfo> Cell;

    public Vector3 TargetPos;

    public BasicUnit Self;
    public BasicUnit Target;

    public AiOrders Orders = new AiOrders();

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    public override void Update()
    {
        base.Update();
        //Move();
    }

    private void Setup()
    {
        BasicAiSeek B1 = new BasicAiSeek(this);
        BasicAiAttack B2 = new BasicAiAttack(this);
        BasicAiHold B3 = new BasicAiHold(this);


        B1.NextStates.Add(B2);
        B1.NextStates.Add(B3);

        B2.NextStates.Add(B1);
        B2.NextStates.Add(B3);

        B3.NextStates.Add(B2);
        B3.NextStates.Add(B1);

        fsmStates.Add(B1);
        fsmStates.Add(B2);
        fsmStates.Add(B3);

        AddState<BasicAiSeek>();

        Orders.AttackUnit += () => { NextState<BasicAiAttack>(); };
        Orders.MoveHere += () => { AddState<BasicAiSeek>(); };
        Orders.Hold += () => { NextState<BasicAiHold>(); };
    }

    void Move()
    {

        Debug.Log(Vector3.Distance(transform.position, TargetPos));
         
        if (Vector3.Distance(transform.position,TargetPos) < 0.01f)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, TargetPos, Speed * Time.deltaTime);
        }

    }

    public IEnumerator FreeSeek(string Key, Vector2Int StartPos)
    {
        var GC = Services.Resolve<GridController>();

        var Grid = GC.GetFromStorage<GridCell<PathfindingInfo>[,]>(Key);
        var BattleInfo = GC.GetFromStorage<GridCell<UnitMapData>[,]>("BattleInfo");

        this.transform.position = Grid[StartPos.x, StartPos.y].Contents.LinkedObject.gameObject.transform.position;
        PosTraker.Set(StartPos.x, StartPos.y);
        TargetPos = Grid[StartPos.x, StartPos.y].Contents.LinkedObject.transform.position;
        
        BattleInfo[PosTraker.x, PosTraker.y].Contents.Taken = true;
        BattleInfo[PosTraker.x, PosTraker.y].Contents.Unit = Self;

        PathfindingInfo Target = Grid[StartPos.x, StartPos.y].Contents;
        float Cost = Grid[StartPos.x, StartPos.y].Contents.Weight;

        Speed = Random.Range(Speed, Speed + 2);

        while (true)
        {
            Cost = Grid[PosTraker.x, PosTraker.y].Contents.BestCost;

            yield return new WaitWhile( () => { if (!Paused || !Hold) { return false; } else { return true;} });

            for (int i = 0; i < Grid[PosTraker.x, PosTraker.y].NeighboursList.Count; i++)
            {
                if (Grid[PosTraker.x, PosTraker.y].NeighboursList[i].Contents.BestCost < Cost)
                {
                    if (this.transform.position == TargetPos)
                    {
                        Cost = Grid[PosTraker.x, PosTraker.y].NeighboursList[i].Contents.BestCost;
                        Target = Grid[PosTraker.x, PosTraker.y].NeighboursList[i].Contents;
                    }
                }

            }

            BattleInfo[PosTraker.x, PosTraker.y].Contents.Taken = false;
            BattleInfo[PosTraker.x, PosTraker.y].Contents.Unit = null;
            PosTraker = Target.GridPostion;
            //this.transform.position = Target.LinkedObject .transform.position;
            TargetPos = Target.LinkedObject.transform.position;

            BattleInfo[PosTraker.x, PosTraker.y].Contents.Unit = Self;
            BattleInfo[PosTraker.x, PosTraker.y].Contents.Taken = true;
        }
    }

    public class BasicAiAttack : FsmState
    {
        private BasicAI AI;

        public BasicAiAttack( BasicAI ai )
        {
            AI = ai;
        }

        public override void InState()
        {
            base.InState();
        }
    }

    public class BasicAiSeek : FsmState
    {
        private BasicAI AI;

        public BasicAiSeek(BasicAI ai)
        {
            AI = ai;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            AI.Hold = false;
            AI.StartCoroutine(AI.FreeSeek("Battle", new Vector2Int(0, 0)));
        }

        public override void InState()
        {
            base.InState();
            AI.Move();
        }

        public override void OnExit()
        {
            base.OnExit();
            //AI.Hold = false;
        }
    }

    public class BasicAiHold : FsmState
    {
        private BasicAI AI;

        public BasicAiHold(BasicAI ai)
        {
            AI = ai;
        }

        public override void InState()
        {
            base.InState();
        }

    }

}

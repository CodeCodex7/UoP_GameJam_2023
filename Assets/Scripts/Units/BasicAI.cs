using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasicAI : FSM
{

    public bool Paused;
    public bool Hold;

    public string MapKey;

    public float Speed = 0.5f;
    public Vector2Int PosTraker = new Vector2Int(0, 0);
    public Vector2Int SeekTarget = new Vector2Int(0, 0);
    GridCell<PathfindingInfo> Cell;

    public Vector3 TargetPos;

    public bool InRange;

    public bool UpdateText = false;

    public BasicUnit Self;
    public BasicUnit EnemyTarget;

    public AiOrders Orders = new AiOrders();

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    public override void Update()
    {
        base.Update();

        if (Vector3.Distance(transform.position, TargetPos) > 0.01f)
        {

            //this.gameObject.transform.position = Vector3.Lerp(this.transform.position, TargetPos, Speed * Time.deltaTime);
            this.gameObject.transform.position = Vector3.MoveTowards(this.transform.position, TargetPos, Speed * Time.deltaTime);
        }
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

        AddState<BasicAiHold>();

        Self = GetComponent<BasicUnit>();



        Orders.AttackUnit += () => { NextState<BasicAiAttack>(); };
        Orders.MoveHere += () => { AddState<BasicAiSeek>(); };
        Orders.Hold += () => { NextState<BasicAiHold>(); };
    }



    GridCell<PathfindingInfo>[,] Grid;
    GridCell<UnitMapData>[,] BattleInfo;
    GridController GC;

    PathfindingInfo Target;
    float Cost;

    bool StuckResolver = false;


    int loopCounter = 0;
    float Timer = 2;
    void Move()
    {
        if (Timer >= 0.01f)
        {
            GC = Services.Resolve<GridController>();
            BattleInfo = GC.GetFromStorage<GridCell<UnitMapData>[,]>("BattleInfo");

            if (GC.GridExists(MapKey))
            {
                //great!
            }
            else
            {
                var Batlle = GC.GetFromStorage<GridCell<PathfindingInfo>[,]>("Battle");

                GC.CreateGrid<PathfindingInfo>(new Vector2Int(Services.Resolve<GameManager>().MapSize.x, Services.Resolve<GameManager>().MapSize.y), MapKey);
                var Cells = GC.GetFromStorage<GridCell<PathfindingInfo>[,]>(MapKey);

                for (int i = 0; i < Batlle.GetLength(0); i++)
                {
                    for (int b = 0; b < Batlle.GetLength(1); b++)
                    {
                        //Cells[i, b] = new GridCell<PathfindingInfo>(new PathfindingInfo());

                        Cells[i, b].Contents.Cost = Batlle[i, b].Contents.Cost;
                        Cells[i, b].Contents.BestCost = Batlle[i, b].Contents.BestCost;
                        Cells[i, b].Contents.Weight = Batlle[i, b].Contents.Weight;

                        Cells[i, b].Contents.WorldPostion = Batlle[i, b].Contents.WorldPostion;
                        Cells[i, b].Contents.GridPostion = Batlle[i, b].Contents.GridPostion;
                        Cells[i, b].Contents.LinkedObject = Batlle[i, b].Contents.LinkedObject;
                    }
                }

                Services.Resolve<FlowField>().GenerateCostField(SeekTarget, MapKey);
                Services.Resolve<FlowField>().GenerateIntergationField(SeekTarget, MapKey, UpdateText);

            }

            if (SeekTarget != EnemyTarget.Postion)
            {
                SeekTarget = EnemyTarget.Postion;

                //if (Vector3.Distance(transform.position, TargetPos) < 5f)
                //{
                Services.Resolve<FlowField>().GenerateCostField(SeekTarget, MapKey);
                Services.Resolve<FlowField>().GenerateIntergationField(SeekTarget, MapKey, UpdateText);
                Timer = 0;
                //}
                //Timer += 1 * Time.deltaTime;
            }

            Grid = GC.GetFromStorage<GridCell<PathfindingInfo>[,]>(MapKey);
            BattleInfo = GC.GetFromStorage<GridCell<UnitMapData>[,]>("BattleInfo");

            Services.Resolve<FlowField>().GenerateCostField(SeekTarget, MapKey);
            Services.Resolve<FlowField>().GenerateIntergationField(SeekTarget, MapKey, UpdateText);

            //this.transform.position = Grid[PosTraker.x, PosTraker.y].Contents.LinkedObject.gameObject.transform.position;
            PosTraker.Set(PosTraker.x, PosTraker.y);
            //TargetPos = Grid[PosTraker.x, PosTraker.y].Contents.LinkedObject.transform.position;

            BattleInfo[PosTraker.x, PosTraker.y].Contents.Taken = true;
            BattleInfo[PosTraker.x, PosTraker.y].Contents.Unit = Self;

            Target = Grid[PosTraker.x, PosTraker.y].Contents;
            Cost = Grid[PosTraker.x, PosTraker.y].Contents.Weight;

            //StuckResolver = false;

            //loopCounter = 0;

            Cost = Grid[PosTraker.x, PosTraker.y].Contents.BestCost;

            for (int i = 0; i < Grid[PosTraker.x, PosTraker.y].NeighboursList.Count; i++)
            {
                if (CostCalu(Grid[PosTraker.x, PosTraker.y].NeighboursList[i].Contents.BestCost, Cost))
                {
                    if (BattleInfo[Grid[PosTraker.x, PosTraker.y].NeighboursList[i].Contents.GridPostion.x, Grid[PosTraker.x, PosTraker.y].NeighboursList[i].Contents.GridPostion.y].Contents.Taken)
                    {
                        Grid[Grid[PosTraker.x, PosTraker.y].NeighboursList[i].Contents.GridPostion.x, Grid[PosTraker.x, PosTraker.y].NeighboursList[i].Contents.GridPostion.y].Contents.Cost = 255;

                        Services.Resolve<FlowField>().GenerateCostField(SeekTarget, MapKey);
                        Services.Resolve<FlowField>().GenerateIntergationField(SeekTarget, MapKey, false);

                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, TargetPos) < 0.01f)
                        {
                            Cost = Grid[PosTraker.x, PosTraker.y].NeighboursList[i].Contents.BestCost;
                            Target = Grid[PosTraker.x, PosTraker.y].NeighboursList[i].Contents;
                        }
                    }
                    loopCounter = 0;
                }


                //if (!(Vector3.Distance(transform.position, TargetPos) < 0.5f))
                //{

                BattleInfo[PosTraker.x, PosTraker.y].Contents.Taken = false;
                BattleInfo[PosTraker.x, PosTraker.y].Contents.Unit = null;

                PosTraker = Target.GridPostion;
                Self.Postion = Target.GridPostion;
                //this.transform.position = Target.LinkedObject .transform.position;
                TargetPos = Target.LinkedObject.transform.position;

                BattleInfo[PosTraker.x, PosTraker.y].Contents.Unit = Self;
                BattleInfo[PosTraker.x, PosTraker.y].Contents.Taken = true;

                //}

                bool CostCalu(float C1, float C2)
                {
                    if (StuckResolver)
                    {
                        if (C1 != 255)
                        {
                            StuckResolver = false;
                            //return true;
                        }
                    }

                    if (C1 < C2)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                Timer = 0;
            }


        }
        Timer += 1 * Time.deltaTime;



    }


    public class BasicAiAttack : FsmState
    {
        private BasicAI AI;
        private float AttackCoolDown;

        public BasicAiAttack(BasicAI ai)
        {
            AI = ai;
        }

        public override void InState()
        {
            base.InState();
            if (!AI.EnemyTarget.Dead)
            {
                if (AI.EnemyTarget == null)
                {
                    AI.ExitState<BasicAiAttack>();
                    AI.AddState<BasicAiHold>();

                }

                if (Vector3.Distance(AI.Self.UnitTransform.position, AI.EnemyTarget.UnitTransform.position) < AI.Self.Range)
                {
                    AI.InRange = true;

                    if (Services.Resolve<BattleController>().DeadUnits.Contains(AI.EnemyTarget))
                    {
                        AI.EnemyTarget = null;
                    }

                    if (AttackCoolDown <= 0)
                    {

                        AI.EnemyTarget.TakeDamage(AI.Self.Damage);
                        AttackCoolDown = AI.Self.AttackSpeed;
                    }
                    else
                    {
                        AttackCoolDown += -1 * Time.deltaTime;
                    }

                }
                else
                {
                    AI.InRange = false;
                }


                if (AI.InRange)
                {
                    AI.ExitState<BasicAiSeek>();
                }
                else
                {
                    AI.ExitState<BasicAiAttack>();
                    //AI.AddState<BasicAiSeek>();
                    AI.AddState<BasicAiHold>();
                }
            }
            else
            {
                AI.ExitState<BasicAiAttack>();
                AI.AddState<BasicAiHold>();
            }
        }
    }

    public class BasicAiSeek : FsmState
    {
        private BasicAI AI;
        public float RecheckMovements;
        public BasicAiSeek(BasicAI ai)
        {
            AI = ai;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            AI.Hold = false;
            RecheckMovements = 0;
            //AI.StartCoroutine(AI.FreeSeek("Battle"));
        }

        public override void InState()
        {
            base.InState();

            AI.Move();

            RecheckMovements += (1f * Time.deltaTime);
            if (RecheckMovements > 3)
            {
                AI.ExitState<BasicAiSeek>();
                AI.AddState<BasicAiHold>();
                RecheckMovements = 0;
            }



            if (Vector3.Distance(AI.Self.UnitTransform.position, AI.EnemyTarget.UnitTransform.position) <= AI.Self.Range)
            {
                AI.ExitState<BasicAiSeek>();
                AI.AddState<BasicAiAttack>();
            }


        }

        public override void OnExit()
        {
            base.OnExit();
            AI.StopAllCoroutines();
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

        public override void OnEnter()
        {
            base.OnEnter();
            AI.EnemyTarget = null;
        }

        public override void InState()
        {
            base.InState();

            if (AI.EnemyTarget == null)
            {
                var GC = Services.Resolve<GridController>();
                var BattleGrid = GC.GetFromStorage<GridCell<UnitMapData>[,]>("BattleInfo");

                List<BasicUnit> TargetList = new List<BasicUnit>();

                foreach (var item in BattleGrid)
                {
                    if (item.Contents.Unit != null && item.Contents.Unit.Team != AI.Self.Team)
                    {
                        if (!item.Contents.Unit.Dead)
                        {
                            TargetList.Add(item.Contents.Unit);
                        }
                    }
                }

                BasicUnit Target = null;

                foreach (var item in TargetList)
                {
                    if (Target == null)
                    {
                        Target = item;
                    }
                    else if (Vector3.Distance(AI.Self.UnitTransform.position, item.UnitTransform.position) < Vector3.Distance(AI.Self.UnitTransform.position, Target.UnitTransform.position))
                    {
                        Target = item;
                    }

                }

                if (Target != null)
                {

                    AI.EnemyTarget = Target;

                    AI.SeekTarget = Target.Postion;
                    AI.ExitState<BasicAiHold>();
                    AI.AddState<BasicAiSeek>();

                }

                //for (int i = 0; i < BattleGrid.; i++)
                //{
                //    for (int b = 0; b < BattleGrid.; b++)
                //    {

                //    }
                //}
            }


        }

    }

}

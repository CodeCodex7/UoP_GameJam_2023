using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasicUnit : MonoBehaviour, IBasicUnit , IDamage
{
    public float HP;
    public float Speed;
    public float Damage;
    public float Range =1;
    public float AttackSpeed = 1;

    public bool Dead;

    public Transform UnitTransform;
    public Vector2Int Postion;
    //Team 1 is Player team 2 is AI
    public int Team;


    public Action AttackEffects;

    public Action<BasicUnit> OnDeath;

    public virtual void TakeDamage(float Damage)
    {
        HP -= Damage;
        if(HP <=0 )
        {
            Death();
        }
    }

    public void Death()
    {
        var BattleGrid = Services.Resolve<GridController>().GetFromStorage<GridCell<UnitMapData>[,]>("BattleInfo");
        BattleGrid[Postion.x, Postion.y].Contents.Taken = false;
        BattleGrid[Postion.x, Postion.y].Contents.Unit = null;

        Dead = true;
        //OnDeath.Invoke();
        Services.Resolve<BattleController>().UnitDeath(this);
        gameObject.SetActive(false);
        //Destroy(this.gameObject);
    }


}

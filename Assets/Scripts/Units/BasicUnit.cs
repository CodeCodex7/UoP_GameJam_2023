using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasicUnit : IBasicUnit , IDamage
{
    public float HP;
    public float Speed;
    public float Damage;

    
    //Team 1 is Player team 2 is AI
    public int Team;


    public Action OnDeath;

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
        OnDeath.Invoke();
    }


}

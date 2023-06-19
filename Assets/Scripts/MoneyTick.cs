using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyTick : Tickable
{
    public float MoneyToGive = 0;

    public override void Tick()
    {
        GiveMoney();
    }

    void GiveMoney()
    {
        Services.Resolve<ResourcesManager>().NumericResources["Money"] += MoneyToGive;
        Debug.Log(string.Format("Money Given {0} from {1}", MoneyToGive.ToString(), this.gameObject.name));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tickable : MonoBehaviour, ITickable
{

    public virtual void Start()
    {
        Services.Resolve<TickManager>().Tickable.Add(this);
    }

    public abstract void Tick();
}

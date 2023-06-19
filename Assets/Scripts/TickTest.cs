using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickTest : Tickable
{
    public override void Tick()
    {
        Debug.Log(string.Format("I {0} am TickTested", this.name));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

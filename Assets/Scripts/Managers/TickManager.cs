using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickManager : MonoService<TickManager>
{

    public List<Tickable> Tickable = new List<Tickable>();

    // We must register the service in Awake or Start
    private void Awake()
    {
        RegisterService(); ///handles Registering the service and accidental Duplicates 
    }


    //If not unregistered elsewhere we want to unregister the service on the Object deletion
    private void OnDestroy()
    {
        UnregisterService();// handles Un-registering the service
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            TickAll();
        }
    }

    public void TickAll()
    {
        for (int i = 0; i < Tickable.Count; i++)
        {
            Tickable[i].Tick();
        }

        Debug.Log(string.Format("Ticked {0} Objects", Tickable.Count));
    }


}

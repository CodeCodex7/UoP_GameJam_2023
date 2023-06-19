using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoService<GameManager> 
{


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


    private void Reset()
    {
        
    }

    void StartGame()
    {
        
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

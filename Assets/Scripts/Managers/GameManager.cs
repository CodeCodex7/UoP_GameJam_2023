using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoService<GameManager> 
{
    public Vector2Int MapSize = new Vector2Int(50, 50);
    public int Unitlimit = 20;
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

    public void TimeSet(float I)
    {
        Time.timeScale = I;
    }

    public void ChangeMapSize(int I)
    {
        MapSize = new Vector2Int(100, 100);
    }

    public void NoUnitLimit()
    {
        Unitlimit = 1000;
    }

    public void GameReset()
    {
        Services.Resolve<GridController>().Clear();
        Services.Resolve<TerrainGenerator>().DestoryTerrain();
        Services.Resolve<BattleController>().Clear();


    }

    public void StartGame()
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

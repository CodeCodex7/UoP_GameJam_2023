using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public enum UnitTypes { Soilder,Archer,Giant };
public enum MapSize { Small, Medium, Large };

public class BattleSettings : MonoService<BattleSettings>
{

    public int UnitLimit = 20;

    public List<UnitTypes> UnitList = new List<UnitTypes>();
    public MapSize Msize = MapSize.Small;


    public void RemoveLast()
    {
        if (UnitList.Count > 0)
        {
            UnitList.RemoveAt(0);
        }
    }

    public List<string> UnitNames()
    {
        List<string> Names = new List<string>();

        foreach (var item in UnitList)
        {
            switch (item)
            {
                case UnitTypes.Soilder:
                    {
                        Names.Add(UnitTypes.Soilder.ToString());
                    }
                    break;

                case UnitTypes.Archer:
                    {
                        Names.Add(UnitTypes.Archer.ToString());
                    }
                    break;

                case UnitTypes.Giant:
                    {
                        Names.Add(UnitTypes.Giant.ToString());
                    }
                    break;
                default:
                    Names.Add(UnitTypes.Soilder.ToString());
                    break;
            }
        }

        return Names;
    }

    public void AddUnit(int unit)
    {
        if (UnitList.Count < Services.Resolve<GameManager>().Unitlimit)
        {
            switch (unit)
            {
                case 1:
                    {
                        UnitList.Add(UnitTypes.Soilder);
                    }
                    break;

                case 2:
                    {
                        UnitList.Add(UnitTypes.Archer);
                    }
                    break;

                case 3:
                    {
                        UnitList.Add(UnitTypes.Giant);
                    }
                    break;
                default:
                    UnitList.Add(UnitTypes.Soilder);
                    break;
            }
        }
    }

    public void BeginBattle()
    {
        Services.Resolve<BattleController>().SetupBattle(UnitList);
    }
    private void Awake()
    {
        RegisterService();
    }

    private void OnDestroy()
    {
        UnregisterService();
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

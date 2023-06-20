using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TechTier { Tier0,Tier1,Tier2,Tier3}
public class ResourcesManager : MonoService<ResourcesManager>
{

    public Dictionary<string, float> NumericResources = new Dictionary<string, float>();
    public Dictionary<string, float> AiNumericResources = new Dictionary<string, float>();

    public Dictionary<string, Dictionary<string, string>> Unlocks = new Dictionary<string, Dictionary<string, string>>();

    private void Awake()
    {
        RegisterService();

        Unlocks.Add("Unlocks", new Dictionary<string, string>());
        Unlocks.Add("AiUnlocks", new Dictionary<string, string>());
        Unlocks["Unlocks"].Add("Tier0", "basic");
        Unlocks["AiUnlocks"].Add("Tier0", "basic");


        NumericResources.Add("Money", 0);
    }

    private void OnDestroy()
    {
        UnregisterService();
    }

    private void Reset()
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

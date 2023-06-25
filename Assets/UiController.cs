using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class UiController : MonoService<UiController> 
{

    public Dictionary<string, GameObject> UIWindows = new  Dictionary<string, GameObject>();
    public List<GameObject> UIWindowsList = new List<GameObject>();

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
        foreach (GameObject go in UIWindowsList)
        {
            UIWindows.Add(go.name, go);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ActiveUIOnly(string key)
    {
        foreach (KeyValuePair<string, GameObject> entry in UIWindows)
        {
            entry.Value.SetActive(false);
        }

        UIWindows[key].SetActive(true);
    }

    public void ActiveUI(string key)
    {
        UIWindows[key].SetActive(true);
    }

    



}

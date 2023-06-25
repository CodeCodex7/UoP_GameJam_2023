using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevPanel : MonoBehaviour
{

    public GameObject D;
    bool Toggle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) 
        {
            Toggle = !Toggle;
            D.SetActive(Toggle);
        }
    }
}

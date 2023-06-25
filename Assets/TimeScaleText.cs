using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeScaleText : MonoBehaviour
{

    public TextMeshProUGUI Text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Text.text = string.Format("Time sacle is {0}", Time.timeScale);
    }
}

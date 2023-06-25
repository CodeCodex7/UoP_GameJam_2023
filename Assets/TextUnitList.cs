using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextUnitList : MonoBehaviour
{

    public TextMeshProUGUI Text;
    public TextMeshProUGUI limit;

    public void UpdateLimitsText()
    {
        limit.text = string.Format("{0} / {1}", Services.Resolve<BattleSettings>().UnitNames().Count, Services.Resolve<GameManager>().Unitlimit);
          
    }

    public void UpdateText()
    {
        Text.text = "";
        var A = Services.Resolve<BattleSettings>().UnitNames();
        foreach (var item in A)
        {
            Text.text += string.Format("{0} \n", item);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
        UpdateLimitsText();
    }
}

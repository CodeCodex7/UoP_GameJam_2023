using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextUnitCounter : MonoBehaviour
{

    TextMeshProUGUI text;
    public int Team;
    // Start is called before the first frame update
    void Start()
    {
        text  = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (Team)
        {
            case 0:
                text.text = string.Format("Red Alive = {0}", Services.Resolve<BattleController>().CountTeamAlive(0));
                break;

            case 1:
                text.text = string.Format("Blue Alive = {0}", Services.Resolve<BattleController>().CountTeamAlive(1));
                break;
        }

    }


}

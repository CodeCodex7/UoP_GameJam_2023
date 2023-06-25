using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitWorldText : MonoBehaviour
{

    TextMeshPro text;
    BasicUnit BU;
    float MaxHP;
    // Start is called before the first frame update
    void Start()
    {
        text= GetComponent<TextMeshPro>();
        BU= GetComponentInParent<BasicUnit>();
        MaxHP = BU.HP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        text.text = string.Format("{0} {1}/{2}", BU.name, BU.HP, MaxHP);
    }
}

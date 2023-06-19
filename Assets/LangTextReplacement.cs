using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LangTextReplacement : MonoBehaviour
{
    TextMeshProUGUI Text;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        Text = GetComponent<TextMeshProUGUI>();
        ScanText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ScanText()
    {
        //var WordArray = Text.text.ToCharArray();

        //string f = "";

        //f.

        //for (int i = 0; i < WordArray.Length; i++)
        //{
        //    if(WordArray[i] == '[')
        //    {

        //    }
        //}

        string ReplacmentText = "";

        var words = Text.text.Split(' ');
        foreach (var w in words)
        {
            if(w =="")
            {
                continue;
            }

            if (w[0] == '[' && w[w.Length-1] == ']')
            {
                ReplacmentText += string.Format(" {0}", Services.Resolve<LanguageManager>().GetWord(w));
            }
            else
            {
                ReplacmentText += string.Format(" {0}", w);
            }
        }

        Text.text = ReplacmentText;

    }

}

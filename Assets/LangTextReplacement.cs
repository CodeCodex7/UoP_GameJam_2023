using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LangTextReplacement : MonoBehaviour
{
    TextMeshProUGUI Text;
    string BaseText;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        Text = GetComponent<TextMeshProUGUI>();
        BaseText = Text.text;
        ScanText();
        Services.Resolve<LanguageManager>().LanguageChange += TextReset;
    }

    private void TextReset()
    {
        Text.text = BaseText;
        ScanText();
    }

    private void OnEnable()
    {
         
    }

    private void OnDestroy()
    {
        Services.Resolve<LanguageManager>().LanguageChange -= TextReset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ScanText()
    {
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
                ReplacmentText += string.Format("{0}", Services.Resolve<LanguageManager>().GetWord(w));
            }
            else
            {
                ReplacmentText += string.Format("{0}", w);
            }
        }

        Text.text = ReplacmentText;
    }

}

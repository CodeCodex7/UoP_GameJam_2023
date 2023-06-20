using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class LanguageManager : MonoService<LanguageManager>
{
    public LanguageFiles Langauges;
    public SuppportedLangs CurrentLang = SuppportedLangs.En;
    public Dictionary<string, Dictionary<string, string>> LangDict = new Dictionary<string, Dictionary<string, string>>();

    public Action LanguageChange;

    private void Awake()
    {
        RegisterService();
        SortLang();
    }

    private void Start()
    {
        
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            ChangeLang();
        }
    }

    public void ChangeLang()
    {
        LanguageChange.Invoke();
    }

    private void OnDestroy()
    {
        UnregisterService();
    }

    public string GetWord(string Key)
    {
        if(LangDict[CurrentLang.ToString()].ContainsKey(Key))
        {
            return LangDict[CurrentLang.ToString()][Key];
        }
        else
        {
            return "[Error key not found]";
        }
        
    }

    void SortLang()
    {
        foreach (var LangFile in Langauges.Supported)
        {
            LangDict.Add(LangFile.lang.ToString(),new Dictionary<string, string>());
            var lang = LangFile.LangFile.text.Split("\n");

            for (int i = 0; i < lang.Length-1; i++)
            {
                var words = lang[i].Split(",");
                LangDict[LangFile.lang.ToString()].Add(words[0], words[1]);
            }

        }
    }



}

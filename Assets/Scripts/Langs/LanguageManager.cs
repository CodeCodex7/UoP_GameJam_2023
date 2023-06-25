using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

[System.Serializable]
public class LanguageManager : MonoService<LanguageManager>
{
    public LanguageFiles Langauges;
    public SuppportedLangs CurrentLang = SuppportedLangs.En;
    public Dictionary<string, Dictionary<string, string>> LangDict = new Dictionary<string, Dictionary<string, string>>();


    public TMPro.TMP_Dropdown DP;

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
        if(Input.GetKeyDown(KeyCode.Keypad0))
        {
            ChangeLang(1);
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            ChangeLang(2);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            ChangeLang(3);
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            ChangeLang(4);
        }
    }

    public void ChangeLang()
    {
        ChangeLang(DP.value);       
    }

    public void ChangeLang(string code)
    {
        switch (code)
        {
            case "En":
                {
                    CurrentLang = SuppportedLangs.En;
                }
                break;
            case "Es":
                {
                    CurrentLang = SuppportedLangs.Es;
                }
                break;
            case "De":
                {
                    CurrentLang = SuppportedLangs.De;
                }
                break;
            case "Bn":
                {
                    CurrentLang = SuppportedLangs.Bn;
                }
                break;
            default:
                break;
        }

        LanguageChange.Invoke();
    }

    public void ChangeLang(int I)
    {
        switch (I)
        {
            case 0:
                {
                    CurrentLang = SuppportedLangs.En;
                }
                break;
            case 1:
                {
                    CurrentLang = SuppportedLangs.Es;
                }
                break;
            case 2:
                {
                    CurrentLang = SuppportedLangs.De;
                }
                break;
            case 3:
                {
                    CurrentLang = SuppportedLangs.Bn;
                }
                break;
            default:
                break;
        }

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

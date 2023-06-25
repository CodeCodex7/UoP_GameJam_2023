using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SuppportedLangs { En, Es, De ,Bn };

[CreateAssetMenu(fileName = "LangFiles", menuName = "LanguagesFiles", order = 1)]
public class LanguageFiles : ScriptableObject
{
    
    public List<SupportedLang> Supported = new List<SupportedLang>();

    [System.Serializable]
    public class SupportedLang
    {
        [SerializeField]
        public SuppportedLangs lang;
        [SerializeField]
        public TextAsset LangFile;
    }
}


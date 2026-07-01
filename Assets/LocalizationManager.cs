using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class LocalizationManager : MonoBehaviour
{
    Dictionary<string, string> dict = new Dictionary<string, string>();

    void Awake()
    {
        TextAsset jsonAsset = Resources.Load<TextAsset>("localization");
        JObject jroot = JObject.Parse(jsonAsset.text);

        string langPref;
        if (PlayerPrefs.HasKey("LanguagePreference"))
            langPref = PlayerPrefs.GetString("LanguagePreference", "eng");
        else
        {
            SystemLanguage sysLang = Application.systemLanguage;
            switch (sysLang)
            {
                
                case SystemLanguage.English:
                    langPref = "eng";
                    break;
                case SystemLanguage.Italian:
                    langPref = "ita";
                    break;
                case SystemLanguage.French:
                    langPref = "fra";
                    break;
                case SystemLanguage.Spanish:
                    langPref = "esp";
                    break;
                case SystemLanguage.German:
                    langPref = "deu";
                    break;
                default:
                    langPref = "eng";
                    break;
            }
            Debug.Log("Language autoselected from system language: " + langPref);
            PlayerPrefs.SetString("LanguagePreference", langPref);
        }

        JToken jTokenLangRoot = jroot[langPref];

        GoDeeper(jTokenLangRoot, "");
    }

    void GoDeeper(JToken jToken, string keys)
    {
        if (jToken is JObject obj)
        {
            foreach (var prop in obj.Properties())
            {
                string compKeys = keys + "." + prop.Name;
                GoDeeper(prop.Value, compKeys);
            }
        }
        else if (jToken is JValue value)
        {
            dict.Add(keys[1..], value.ToString());
        }
    }

    public string Get(string key)
    {
        if (dict.ContainsKey(key))
            return dict[key];
        else
            return null;
    }
}

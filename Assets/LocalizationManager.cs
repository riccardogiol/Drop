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

        string langPref = PlayerPrefs.GetString("LanguagePreference", "eng");
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

    void Start()
    {
        foreach (var kvp in dict)
        {
            Debug.Log(kvp.Key + ": " + kvp.Value);
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

using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace SOLocalisation
{
    [CreateAssetMenu(fileName = "LocalisationText", menuName = "Localisation Text")]
    public class SOLocalisationTextAsset : SerializedScriptableObject
    {
        public Dictionary<Language, string> text = new Dictionary<Language, string>
        {
            { Language.en, "" },
            { Language.ru, "" }
        };

        [Title("Controls")]
        private void AddLanguage(Language language, string translation = "")
        {
            if (!text.ContainsKey(language))
                text.Add(language, translation);
        }

        [Button]
        public void AddAllLanguageKeys()
        {
            foreach (Language language in Enum.GetValues(typeof(Language)))
            {
                AddLanguage(language);
            }
        }

        /// <summary>
        /// Returns currently selected language
        /// </summary>
        /// <returns>Current language</returns>
        private Language GetCurrentLanguage()
        {
            return SOLocalisationManager.Instance.CurrentLanguage;
        }

        /// <summary>
        /// Returns text value for the current language
        /// </summary>
        /// <returns>Text in current language</returns>
        public string Get()
        {
            return text[GetCurrentLanguage()];
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace SOLocalisation
{
    [CreateAssetMenu(fileName = "LocalisationText", menuName = "Localisation Text")]
    public class SOLocalisationTextAsset : SerializedScriptableObject
    {
        /// <summary>
        /// Dictionary with text values for different languages
        /// </summary>
        public Dictionary<Language, string> text = new Dictionary<Language, string>();

        /// <summary>
        /// Add a new language to the asset if it was not added before
        /// </summary>
        /// <param name="language">Language enum value to add</param>
        /// <param name="translation">Asset value in the new language</param>
        [Title("Controls")]
        private void AddLanguage(Language language, string translation = "")
        {
            if (!text.ContainsKey(language))
                text.Add(language, translation);
        }

        /// <summary>
        /// Add all currently existing languages to this asset without any text values
        /// </summary>
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
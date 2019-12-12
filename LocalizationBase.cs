using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace SOLocalisation
{
    public abstract class LocalizationBase : SerializedScriptableObject
    {
        /// <summary>
        /// Add all currently existing languages to this asset without any text values
        /// </summary>
        [Button]
        public void AddAllLanguagesKeys()
        {
            foreach (Language language in Enum.GetValues(typeof(Language)))
                AddLanguage(language);
        }

        /// <summary>
        /// Generates and returns the list of dictionary entries
        /// containing Language to string pairs
        /// </summary>
        /// <returns></returns>
        public abstract List<Dictionary<Language, string>> GetLocalizedStrings();

        /// <summary>
        /// Provided the list of dictionary entries containing language to string pairs
        /// should assign values in the same order, as they were gotten from the
        /// GetLocalizedStrings() method.
        /// </summary>
        /// <param name="localizedStrings">List of dictionaries to list</param>
        public abstract void SetLocalizedStrings (List<Dictionary<Language, string>> localizedStrings);

        /// <summary>
        /// Add a new language to the asset if it was not added before
        /// </summary>
        /// <param name="language">Language enum value to add</param>
        /// <param name="translation">Asset value in the new language</param>
        [Title("Controls")]
        protected abstract void AddLanguage(Language language, string translation = "");

        /// <summary>
        /// Get the full path to file
        /// </summary>
        /// <returns>string containing full path to localization file</returns>
        public abstract string PathToLocalizationFile();
    }
}
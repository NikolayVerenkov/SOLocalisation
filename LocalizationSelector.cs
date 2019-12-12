using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SOLocalisation
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class LocalizationSelector : MonoBehaviour
    {
        private TMP_Dropdown dropdown;

        private void Start()
        {
            //Cache dropdown UI component
            dropdown = GetComponent<TMP_Dropdown>();

            //Generate a list of all languages
            List<string> languages = new List<string>(Enum.GetNames(typeof(Language)));

            //Populate dropdown with the list of all languages
            dropdown.AddOptions(languages);
        }

        /// <summary>
        /// Try to change the current language and if successfull raise a language change event
        /// </summary>
        /// <param name="languageIndex">Language index in the enum</param>
        public void ChangeLanguageWithIndex(int languageIndex = 0)
        {
            LocalizationManager.Instance.ChangeLanguageWithIndex(languageIndex);
        }

    }
}


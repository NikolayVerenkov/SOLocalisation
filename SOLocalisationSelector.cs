using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SOLocalisation
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class SOLocalisationSelector : MonoBehaviour
    {
        private TMP_Dropdown dropdown;

        private void Start()
        {
            //Cache dropdown UI component
            dropdown = GetComponent<TMP_Dropdown>();

            List<string> languages = new List<string>(Enum.GetNames(typeof(Language)));

            dropdown.AddOptions(languages);
        }

        public void ChangeLanguageWithIndex(int languageIndex = 0)
        {
            SOLocalisationManager.Instance.ChangeLanguageWithIndex(languageIndex);
        }

    }
}


using System;
using UnityEngine;
using Sirenix.OdinInspector;


public enum Language { en, ru }

namespace SOLocalisation
{
    public class SOLocalisationManager : MonoBehaviour
    {
        private static SOLocalisationManager _instance;
        private static SOLocalisationManager Instance => _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
                Destroy(gameObject);
            else
                _instance = this;
        }

        [field: SerializeField,
                ReadOnly,
                Tooltip("Current language")]
        private Language currentLanguage = Language.en;
        public Language CurrentLanguage => currentLanguage;

        [field: SerializeField,
                AssetList(AutoPopulate = true),
                Tooltip("All text localisation scriptable objects")]
        private SOLocalisationTextAsset[] localisationTextAssets;

        /// <summary>
        /// Add all currently available language keys to all registered localisation text scriptable objects
        /// </summary>
        [Title("Controls")]
        [Button]
        private void AddAllLanguageKeys()
        {
            foreach (SOLocalisationTextAsset localisationAsset in localisationTextAssets)
            {
                localisationAsset.AddAllLanguageKeys();
            }
        }

        /// <summary>
        /// Try to change the current language and if successfull raise a language change event
        /// </summary>
        /// <param name="language">Language to set</param>
        /// <returns>true if the language was different or false if it was already set to this value</returns>
        [Button]
        public bool ChangeLanguage(Language language = Language.en)
        {
            if (currentLanguage != language)
            {
                currentLanguage = language;
                return true;
            }
            else
            {
                return false;
            }
        }

        [ButtonGroup("Export and Import")]
        private void Export()
        {
            var sheet = new ES3Spreadsheet();
            // Add data to cells in the spreadsheet.
            for (int col = 0; col <= Enum.GetNames(typeof(Language)).Length; col++)
                for (int row = 0; row <= localisationTextAssets.Length; row++)
                    if (col == 0 && row == 0) //Top left corner
                        sheet.SetCell<string>(col, row, "Localisation");
                    else if (row == 0) //Headers
                        sheet.SetCell<string>(col, row, GetLanguageStringByIndex(col - 1));
                    else if (col == 0) //Keys
                        sheet.SetCell<string>(col, row, localisationTextAssets[row - 1].name);
                    else //Translations
                        sheet.SetCell<string>(col, row, localisationTextAssets[row - 1].text[GetLanguageByIndex(col - 1)]);
            sheet.Save("localisation.csv");
        }

        [ButtonGroup("Export and Import")]
        private void Import()
        {

        }

        [Button]
        private void TestCSV()
        {
            var sheet = new ES3Spreadsheet();
            // Add data to cells in the spreadsheet.
            for (int col = 0; col < 10; col++)
                for (int row = 0; row < 8; row++)
                    sheet.SetCell<string>(col, row, "someData");
            sheet.Save("mySheet.csv");
        }

        private String GetLanguageStringByIndex(int index)
        {
            if (Enum.IsDefined(typeof(Language), index))
                return ((Language)index).ToString();
            else
                return "Invalid Value";
        }

        private Language GetLanguageByIndex(int index)
        {
            if (Enum.IsDefined(typeof(Language), index))
                return ((Language)index);
            else
                return Language.en;
        }
    }
}

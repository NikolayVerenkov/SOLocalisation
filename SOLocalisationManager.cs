using System;
using UnityEngine;
using Sirenix.OdinInspector;

public enum Language { en, ru }

namespace SOLocalisation
{
    public class SOLocalisationManager : MonoBehaviour
    {
        private static SOLocalisationManager _instance;
        public static SOLocalisationManager Instance => _instance;

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

        [field: SerializeField,
                ReadOnly,
                Tooltip("Name for the localisation csv")]
        private string localisationFileName = "localisation";

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
                        sheet.SetCell<string>(col, row, GetLanguageStringWithIndex(col - 1));
                    else if (col == 0) //Keys
                        sheet.SetCell<string>(col, row, localisationTextAssets[row - 1].name);
                    else //Translations
                        sheet.SetCell<string>(col, row, localisationTextAssets[row - 1].text[GetLanguageWithIndex(col - 1)]);
            sheet.Save(localisationFileName + ".csv");
            Debug.Log("Localisation export finished. Exported " + (localisationTextAssets.Length) + " lines in " + (Enum.GetNames(typeof(Language)).Length) + " languages");
        }

        [ButtonGroup("Export and Import")]
        private void Import()
        {
            // Create a blank ES3Spreadsheet.
            var sheet = new ES3Spreadsheet();
            sheet.Load(localisationFileName + ".csv");
            for (int row = 1; row < localisationTextAssets.Length + 1; row++)
            {
                SOLocalisationTextAsset textAsset = localisationTextAssets[row - 1];
                if (sheet.GetCell<string>(0, row) != textAsset.name.ToString())
                {
                    //Catch error if the name in the CSV file is not equal to the name of the scriptable object
                    Debug.LogError("Row: " + row + ". Name from CSV " + sheet.GetCell<string>(0, row) + " != " + textAsset.name.ToString() + "name from scriptable object. Check row order");

                }
                else
                {
                    for (int col = 1; col < sheet.ColumnCount; col++)
                    {
                        textAsset.text[GetLanguageWithIndex(col - 1)] = sheet.GetCell<string>(col, row);
                    }
                }
            }
            Debug.Log("Localisation import finished. Imported " + (localisationTextAssets.Length) + " lines in " + (sheet.ColumnCount - 1) + " languages");       
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

        private String GetLanguageStringWithIndex(int index)
        {
            if (Enum.IsDefined(typeof(Language), index))
                return ((Language)index).ToString();
            else
                return "Invalid Value";
        }

        private Language GetLanguageWithIndex(int index)
        {
            if (Enum.IsDefined(typeof(Language), index))
                return ((Language)index);
            else
                return Language.en;
        }
    }
}

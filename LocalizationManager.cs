using System;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace SOLocalisation
{
    public class LocalizationManager : MonoBehaviour
    {
        #region Singleton
        private static LocalizationManager _instance;
        public static LocalizationManager Instance => _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
                Destroy(gameObject);
            else
                _instance = this;
        }
        #endregion

        #region UnityVariables
        
        /// <summary>
        /// Current language
        /// </summary>
        [field: SerializeField,
                ReadOnly,
                Tooltip("Current language")]
        private Language currentLanguage = Language.EnUs;
        public Language CurrentLanguage => currentLanguage;

        /// <summary>
        /// All text localisation scriptable objects
        /// </summary>
        [field: SerializeField,
                AssetList(AutoPopulate = true),
                Tooltip("All text localisation scriptable objects")]
        private LocalizationBase[] localisationTextAssets;

        /// <summary>
        /// Name for the localisation csv file
        /// </summary>
        [field: SerializeField,
                ReadOnly,
                Tooltip("Name for the localisation csv file")]
        private string localisationFileName = "localisation";

//        /// <summary>
//        /// All text controllers active in the scene
//        /// </summary>
//        [field: SerializeField,
//                ReadOnly,
//                Tooltip("All text controllers active in the scene")]
//        private List<LocalizationTextController> textControllers = new List<LocalizationTextController>();
        
        #endregion
        
       

        /// <summary>
        /// Add all currently available language keys to all registered localisation text scriptable objects
        /// </summary>
        [method: Title("Controls"), Button]
        private void AddAllLanguageKeysEverywhere()
        {
            foreach (LocalizationBase localisationAsset in localisationTextAssets)
                localisationAsset.AddAllLanguagesKeys();
        }

        /// <summary>
        /// Try to change the current language and if successfull raise a language change event
        /// </summary>
        /// <param name="language">Language to set</param>
        /// <returns>true if the language was different or false if it was already set to this value</returns>
        [method: Button]
        public bool ChangeLanguage(Language language = Language.EnUs)
        {
            if (currentLanguage == language)
                return false;
            
            currentLanguage = language;
            //RefreshTextControllers();
            return true;
        }

        /// <summary>
        /// Try to change the current language and if successfull raise a language change event
        /// </summary>
        /// <param name="languageIndex">Language index in the enum</param>
        [method: Button]
        public void ChangeLanguageWithIndex(int languageIndex = 0)
        {
            Language newLanguage = LanguageUtil.LanguageAt(languageIndex);
            if (currentLanguage != newLanguage)
            {
                currentLanguage = newLanguage;
                //RefreshTextControllers();
            }
        }

        /// <summary>
        /// Export all language data to the csv file
        /// </summary>
        [method: ButtonGroup("Export and Import")]
        private void Export()
        {
            int languagesCount = Enum.GetNames(typeof(Language)).Length;

            //gather and count all unique types we will be exporting 
            var spreadsheets = new Dictionary<string, ES3Spreadsheet>();
            for (int i = 0; i < localisationTextAssets.Length; i++)
            {
                string typeName = localisationTextAssets[i].PathToLocalizationFile();
                if (!spreadsheets.ContainsKey(typeName))
                    spreadsheets.Add(typeName, new ES3Spreadsheet());
            }

            //save our localization assets to proper files
            for (int i = 0; i < localisationTextAssets.Length; i++)
            {
                string rowName = localisationTextAssets[i].name;
                string key = localisationTextAssets[i].PathToLocalizationFile();
                ES3Spreadsheet sheet = spreadsheets[key];
                int startRow = sheet.RowCount;
                List<Dictionary<Language, string>> locStrings = localisationTextAssets[i].GetLocalizedStrings();
                int rowsToAdd = locStrings.Count + (sheet.ColumnCount == 0 ? 1 : 0);

                //TODO: Possibly, fill first row with the headers, so we can get rid of redundant checks
                for (int col = 0; col <= languagesCount; col++)
                for (int row = startRow; row < startRow + rowsToAdd; row++)
                    if (col == row && col == 0) //Top left corner
                        sheet.SetCell<string>(col, row, "Localization");
                    else if (col == 0)          //Key
                        sheet.SetCell<string>(col, row, rowName);
                    else if (row == 0)          //Header
                        sheet.SetCell<string>(
                            col, row, LanguageUtil.StringAt(col - 1));
                    else                        //Translation
                    {
                        int mod = startRow == 0 ? -1 : 0; // *Checks like this* can be avoided
                        var lDict = locStrings[row - startRow + mod]; //dictionary<language, string> from list of dicts
                        var lDString = lDict[LanguageUtil.LanguageAt(col - 1)];
                        sheet.SetCell<string>(col, row, lDString);
                    }
            }

            foreach (KeyValuePair<string,ES3Spreadsheet> es3Spreadsheet in spreadsheets)
                es3Spreadsheet.Value.Save(es3Spreadsheet.Key);

            Debug.Log("Localization export finished. Exported " + localisationTextAssets.Length + " lines in " + Enum.GetNames(typeof(Language)).Length + " languages");
        }

        /// <summary>
        /// Import all language data from the previously exported CSV
        /// </summary>
        [method: ButtonGroup("Export and Import")]
        private void Import()
        {
            string locFolderPath = Application.streamingAssetsPath + "/Localization";
            string[] files = Directory.GetFiles(locFolderPath);
            files = files.Where(x => x.EndsWith(".csv")).ToArray();

            for (int i = 0; i < files.Length; i++)
            {
                var sheet = new ES3Spreadsheet();
                sheet.Load(files[i]);
                int rowCount = sheet.RowCount;

                int rowDepth = 0, checkRow = 1;
                var compareAgainst = sheet.GetCell<string>(0, checkRow);
                
                
                while (true)
                {
                    checkRow++;
                    rowDepth++;

                    if (checkRow > rowCount)
                    {
                        Debug.Log("Break cuz to big: " + checkRow);
                        rowDepth--;
                        break;
                    }

                    if (sheet.GetCell<string>(0, checkRow) != compareAgainst)
                    {
                        Debug.Log("Break cuz found target");
                        break;
                    }
                }

                Debug.Log("Row Depth: " + rowDepth);
                
                
                for (int row = 1; row < rowCount - 1; row += rowDepth)
                {
                    Debug.Log("Current Row: " + row);
                    List<Dictionary<Language, string>> dictEntry = 
                        new List<Dictionary<Language, string>>();

                    for (int subRow = row; subRow < row + rowDepth; subRow++)
                    {
                        var dict = new Dictionary<Language, string>();
                        for (int col = 1; col < sheet.ColumnCount; col++)
                            dict.Add(
                                LanguageUtil.LanguageAt(col - 1), 
                                sheet.GetCell<string>(col, subRow)
                                );
                        dictEntry.Add(dict);
                        Debug.Log("sRow: " + subRow);
                    }

                    var locBase = GetLocalizationTarget(sheet.GetCell<string>(0, row));
                    locBase.SetLocalizedStrings(dictEntry);
                    
                    
                    Debug.Log($"Finished importing to the {locBase.name} ScriptableObject");  
                }
                
            }
            
             
            Debug.Log("Localization import finished. Imported " + localisationTextAssets.Length);
        }

        #region CustomMethods

        private LocalizationBase GetLocalizationTarget(string name)
        {
            for (int i = 0; i < localisationTextAssets.Length; i++)
                if (localisationTextAssets[i].name == name)
                    return localisationTextAssets[i];

            throw new Exception("Unknown object found in localization strings." +
                                $"Object name: {name}");
        }
        
        #endregion
        
        #region TextController
        
//        /// <summary>
//        /// Register a text controller with the manager
//        /// </summary>
//        /// <param name="textController">Controller to register</param>
//        /// <returns>true if the registration was successfull, false if this controller has already been registered</returns>
//        public bool RegisterTextController(LocalizationTextController textController)
//        {
//            if (!textControllers.Contains(textController))
//            {
//                textControllers.Add(textController);
//                return true;
//            }
//
//            return false;
//        }
//
//        /// <summary>
//        /// Refresh text displayed on all text controllers
//        /// </summary>
//        public void RefreshTextControllers()
//        {
//            foreach (LocalizationTextController controller in textControllers)
//                controller.Refresh();
//        }

        
        #endregion
        
       
        
    }
}

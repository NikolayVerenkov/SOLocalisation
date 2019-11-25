using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace SOLocalisation
{
    public class SOLocalisationTextController : MonoBehaviour
    {
        /// <summary>
        /// Standard Unity UI Text reference
        /// </summary>
        private Text textComponent;

        /// <summary>
        /// TextMeshPro UI text component reference 
        /// </summary>
        private TextMeshProUGUI textMeshProUGUIComponent;

        /// <summary>
        /// TextMeshPro worldspace text component reference 
        /// </summary>
        private TextMeshPro textMeshProComponent;

        /// <summary>
        /// Reference to the text asset scriptable object
        /// </summary>
        [field: SerializeField,
                Required]
        private SOLocalisationTextAsset textAsset;

        private void Start()
        {
            //Caching needed components
            textComponent = GetComponent<Text>();
            textMeshProUGUIComponent = GetComponent<TextMeshProUGUI>();
            textMeshProComponent = GetComponent<TextMeshPro>();

            //Register with the localisation manager
            Register();

            //Refreshing text
            Refresh();
        }

        /// <summary>
        /// Refresh text in the attached text component
        /// </summary>
        [Button]
        public void Refresh()
        {
            if(textAsset != null)
            {
                if (textComponent != null)
                    textComponent.text = textAsset.Get();
                if (textMeshProUGUIComponent != null)
                    textMeshProUGUIComponent.text = textAsset.Get();
                if (textMeshProComponent != null)
                    textMeshProComponent.text = textAsset.Get();
            }
            else
            {
                Debug.LogError("No text asset selected!");
            }
        }

        /// <summary>
        /// Register this text controller with the SOLocalisationManager
        /// </summary>
        private void Register()
        {
            SOLocalisationManager.Instance.RegisterTextController(this);
        }

    }
}

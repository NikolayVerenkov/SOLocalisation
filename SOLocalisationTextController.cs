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
        private Text textComponent;
        private TextMeshProUGUI textMeshProUGUIComponent;
        private TextMeshPro textMeshProComponent;

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

        private void Register()
        {
            SOLocalisationManager.Instance.RegisterTextController(this);
        }

    }
}

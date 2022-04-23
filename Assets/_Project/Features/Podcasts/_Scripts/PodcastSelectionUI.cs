using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Multiplatform.Podcasts
{
    public class PodcastSelectionUI : MonoBehaviour
    {
        [SerializeField]
        Dropdown dropDown;

        UnityAction<string> onSelectionChanged;

        private void Awake()
        {
            dropDown.onValueChanged.AddListener((int _i) => onSelectionChanged?.Invoke(dropDown.options[dropDown.value].text));
        }
        public void SetSelection(Dictionary<string, string>  _podcasts, UnityAction<string> _onSelectionChanged)
        {
            onSelectionChanged = _onSelectionChanged;

            dropDown.interactable = true;

            dropDown.options.Clear();

            foreach(string key in _podcasts.Keys)
            {
                Dropdown.OptionData optionData = new Dropdown.OptionData();
                optionData.text = key;
                dropDown.options.Add(optionData);
            }
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Multiplatform.Podcasts
{
    [RequireComponent(typeof(Slider))]
    public class CustomSliderControl : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        public UnityEvent<float> onValueChanged;
        Slider slider;

        bool canEdit = true;

        void Awake()
        {
            slider = GetComponent<Slider>();
        }

        public void SetInteractable(bool _value)
        {
            slider.interactable = _value;
        }
        public void SetValue(float _value)
        {
            if (canEdit)
            {
                //Debug.Log("Editing Slider");
                slider.value = _value;
            }
        }

        #region Interface Methods
        public void OnBeginDrag(PointerEventData eventData)
        {
            canEdit = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            onValueChanged?.Invoke(slider.value);
            canEdit = true;
        }
        #endregion
    }
}
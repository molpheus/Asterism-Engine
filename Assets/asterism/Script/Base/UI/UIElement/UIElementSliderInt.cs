using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Asterism.UI
{
    [Serializable]
    public class UIElementSliderInt : UIElement, IUIElementAttribute, IDisposable
    {
        private SliderInt _slider;

        public int Value { get => _slider.value; set => _slider.value = value; }
        public int HighValue { get => _slider.highValue; set => _slider.highValue = value; }
        public int LowValue { get => _slider.lowValue; set => _slider.lowValue = value; }

        public UnityEvent<int> ValueChanged;

        public void Initialize(VisualElement visualElement)
        {
            _slider = visualElement.SearchElement<SliderInt>(TagNameList, out var element);
            Element = element;
            Assert.IsNotNull(_slider);
            _slider.RegisterValueChangedCallback(HandleCallback);
        }

        private void HandleCallback(ChangeEvent<int> evt)
        {
            ValueChanged?.Invoke(evt.newValue);
        }

        public void Dispose()
        {
            _slider.UnregisterValueChangedCallback(HandleCallback);
            _slider = null;
        }
    }
}

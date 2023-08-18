using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Asterism.UI
{
    public class UIElementMinMaxSlider : UIElement, IUIElementAttribute, IDisposable
    {
        private MinMaxSlider _slider;

        public float LowerValue => _slider.value.x;
        public float HigherValue => _slider.value.y;
        public Vector2 Value { get => _slider.value; set => _slider.value = value; }
        public float MaxValue { get => _slider.maxValue; set => _slider.maxValue = value; }
        public float minValue { get => _slider.minValue; set => _slider.minValue = value; }

        public UnityEvent<Vector2> ValueChanged;

        public void Initialize(VisualElement visualElement)
        {
            _slider = visualElement.SearchElement<MinMaxSlider>(TagNameList, out var element);
            Element = element;
            Assert.IsNotNull(_slider);

            _slider.RegisterValueChangedCallback(HandleCallback);
        }

        private void HandleCallback(ChangeEvent<Vector2> evt)
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

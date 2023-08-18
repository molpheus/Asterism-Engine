using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Asterism.UI
{
    public class UIElementRadioButton : UIElement, IUIElementAttribute, IDisposable
    {
        private RadioButton _radioButton;

        public UnityEvent<bool> ValueChanged;

        public void Initialize(VisualElement visualElement)
        {
            _radioButton = visualElement.SearchElement<RadioButton>(TagNameList, out var element);
            Element = element;

            Assert.IsNotNull(Element);
            Assert.IsNotNull(_radioButton);

            _radioButton.RegisterValueChangedCallback(HandleCallback);
        }

        private void HandleCallback(ChangeEvent<bool> evt)
        {
            ValueChanged?.Invoke(evt.newValue);
        }


        public void Dispose()
        {
            _radioButton.UnregisterValueChangedCallback(HandleCallback);
            _radioButton = null;
        }
    }
}

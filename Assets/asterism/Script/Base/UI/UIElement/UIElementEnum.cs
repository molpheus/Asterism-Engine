using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Asterism.UI
{
    public class UIElementEnum : UIElement, IUIElementAttribute, IDisposable
    {
        private EnumField _enumField;
        public UnityEvent<Enum> ValueChanged;

        public void Initialize(VisualElement visualElement)
        {
            _enumField = visualElement.SearchElement<EnumField>(TagNameList, out var element);
            Element = element;

            Assert.IsNotNull(Element);
            Assert.IsNotNull(_enumField);

            _enumField.RegisterValueChangedCallback(HandleCallback);
        }

        private void HandleCallback(ChangeEvent<Enum> evt)
        {
            ValueChanged?.Invoke(evt.newValue);
        }


        public void Dispose()
        {
            _enumField.UnregisterValueChangedCallback(HandleCallback);
            _enumField = null;
        }
    }
}

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
    public class UIElementDropdown : UIElement, IUIElementAttribute, IDisposable
    {
        private DropdownField _dropDown;

        public string Value => _dropDown.value;
        public int Index => _dropDown.index;
        public List<string> Choice => _dropDown.choices;

        public UnityEvent<string> ValueChanged;


        public void Initialize(VisualElement visualElement)
        {
            _dropDown = visualElement.SearchElement<DropdownField>(TagNameList, out var element);
            Element = element;

            Assert.IsNotNull(Element);
            Assert.IsNotNull(_dropDown);

            _dropDown.RegisterValueChangedCallback(HandleCallback);
        }

        private void HandleCallback(ChangeEvent<string> evt)
        {
            ValueChanged?.Invoke(evt.newValue);
        }

        public void Dispose()
        {
            _dropDown.UnregisterValueChangedCallback(HandleCallback);
            _dropDown = null;
        }
    }
}

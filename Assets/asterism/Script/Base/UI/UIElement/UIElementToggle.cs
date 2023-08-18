using System;

using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Asterism.UI
{
    [Serializable]
    public sealed class UIElementToggle : UIElement, IUIElementAttribute, IDisposable
    {
        private Toggle _toggle;

        public string Label { get => _toggle.label; set => _toggle.label = value; }
        public bool Value { get => _toggle.value; set => _toggle.value = value; }

        public UnityEvent<bool> ValueChanged;

        public void Initialize(VisualElement visualElement)
        {
            _toggle = visualElement.SearchElement<Toggle>(TagNameList, out var element);
            Element = element;

            Assert.IsNotNull(Element);
            Assert.IsNotNull(_toggle);

            _toggle.RegisterValueChangedCallback(HandleCallback);
        }

        private void HandleCallback(ChangeEvent<bool> evt)
        {
            ValueChanged?.Invoke(evt.newValue);
        }

        public void Dispose()
        {
            if (_toggle != null)
                _toggle.UnregisterValueChangedCallback(HandleCallback);

            _toggle = null;
        }
    }
}

using System;

using UnityEngine.Assertions;
using UnityEngine.UIElements;
using UnityEngine.Events;

namespace Asterism.UI
{
    [Serializable]
    public class UIElementTextField : UIElement, IUIElementAttribute, IDisposable
    {
        private TextField _textField;

        public string Label { get => _textField.label; set => _textField.label = value; }
        public string tooltip { get => _textField.tooltip; set => _textField.tooltip = value; }

        public UnityEvent<string> ValueChanged;

        public void Initialize(VisualElement visualElement)
        {
            _textField = visualElement.SearchElement<TextField>(TagNameList, out var element);
            Element = element;

            Assert.IsNotNull(Element);
            Assert.IsNotNull(_textField);

            _textField.RegisterValueChangedCallback(HandleCallback);
        }

        private void HandleCallback(ChangeEvent<string> evt)
        {
            ValueChanged?.Invoke(evt.newValue);
        }

        public void Dispose()
        {
            if (_textField != null)
                _textField.UnregisterValueChangedCallback(HandleCallback);

            _textField = null;
        }
    }
}

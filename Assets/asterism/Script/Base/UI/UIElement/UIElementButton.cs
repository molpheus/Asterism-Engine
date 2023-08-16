using System;

using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Asterism.UI
{
    [Serializable]
    public sealed class UIElementButton : UIElement, IUIElementAttribute, IDisposable
    {
        private Button _button { get; set; }

        public string Text { get => _button.text; set => _button.text = value; }
        public Action OnClick { get; set; }

        public void Initialize(VisualElement visualElement)
        {
            _button = null;
            _button = visualElement.SearchElement<Button>(TagNameList, out var element);
            Element = element;
            Assert.IsNotNull(_button);
            _button.clicked += ButtOnClicked;
        }

        private void ButtOnClicked()
        {
            OnClick?.Invoke();
        }

        public void Dispose()
        {
            if (_button != null)
                _button.clicked -= ButtOnClicked;

            _button = null;
        }
    }
}
using System;

using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Asterism.UI
{
    [Serializable]
    public sealed class UIElementScroller : UIElement, IUIElementAttribute, IDisposable
    {
        private Scroller _scroller;

        public float LowValue { get => _scroller.lowValue; set => _scroller.lowValue = value; }
        public float HighValue { get => _scroller.highValue; set => _scroller.highValue = value; }
        public float Value { get => _scroller.value; set => _scroller.value = value; }

        public Action<float> OnChanged { get; set; }

        public void Initialize(VisualElement visualElement)
        {
            _scroller = visualElement.SearchElement<Scroller>(TagNameList, out var element);
            Element = element;

            Assert.IsNotNull(Element);

            _scroller.valueChanged += _scroller_valueChanged;
        }

        private void _scroller_valueChanged(float value)
        {
            OnChanged?.Invoke(value);
        }

        public void Dispose()
        {
            if (_scroller != null)
                _scroller.valueChanged -= _scroller_valueChanged;
            
            _scroller = null;
        }
    }
}

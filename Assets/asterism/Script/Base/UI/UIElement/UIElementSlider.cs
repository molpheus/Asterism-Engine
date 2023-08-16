using System;

using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Asterism.UI
{
    [Serializable]
    public class UIElementSlider : UIElement, IUIElementAttribute, IDisposable
    {
        private Slider _slider;

        public float Value { get => _slider.value; set => _slider.value = value; }
        public float HighValue { get => _slider.highValue; set => _slider.highValue = value; }
        public float LowValue { get => _slider.lowValue; set => _slider.lowValue = value; }

        public void Initialize(VisualElement visualElement)
        {
            _slider = visualElement.SearchElement<Slider>(TagNameList, out var element);
            Element = element;
            Assert.IsNotNull(_slider);
        }

        public void Dispose()
        {
            _slider = null;
        }
    }
}

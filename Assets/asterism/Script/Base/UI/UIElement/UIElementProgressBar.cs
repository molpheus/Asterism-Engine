using System;

using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Asterism.UI
{
    [Serializable]
    public sealed class UIElementProgressBar : UIElement, IUIElementAttribute, IDisposable
    {
        private ProgressBar _progressBar;

        public float Value { get => _progressBar.value; set => _progressBar.value = value; }
        public float HighValue { get => _progressBar.highValue; set => _progressBar.highValue = value; }
        public float LowValue { get => _progressBar.lowValue; set => _progressBar.lowValue = value; }

        public UnityEvent<float> ValueChanged;

        public void Initialize(VisualElement visualElement)
        {
            _progressBar = visualElement.SearchElement<ProgressBar>(TagNameList, out var element);
            Element = element;
            Assert.IsNotNull(_progressBar);

            _progressBar.RegisterValueChangedCallback(HandleCallback);
        }

        private void HandleCallback(ChangeEvent<float> evt)
        {
            ValueChanged?.Invoke(evt.newValue);
        }


        public void Dispose()
        {
            _progressBar.UnregisterValueChangedCallback(HandleCallback);
            _progressBar = null;
        }
    }
}

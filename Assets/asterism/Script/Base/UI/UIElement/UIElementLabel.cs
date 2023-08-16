using System;

using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Asterism.UI
{
    [Serializable]
    public sealed class UIElementLabel : UIElement, IUIElementAttribute, IDisposable
    {
        private Label _label;

        public string Text { get => _label.text; set => _label.text = value; }

        public void Initialize(VisualElement visualElement)
        {
            _label = visualElement.SearchElement<Label>(TagNameList, out var element);
            Element = element;

            Assert.IsNotNull(Element);
            Assert.IsNotNull(_label);
        }

        public void Dispose()
        {
            _label = null;
        }
    }
}

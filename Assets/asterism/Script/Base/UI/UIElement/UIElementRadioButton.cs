using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Asterism.UI
{
    public class UIElementRadioButton : UIElement, IUIElementAttribute, IDisposable
    {
        private RadioButton _radioButton;

        public void Initialize(VisualElement visualElement)
        {
            _radioButton = visualElement.SearchElement<RadioButton>(TagNameList, out var element);
            Element = element;

            Assert.IsNotNull(Element);
            Assert.IsNotNull(_radioButton);
        }

        public void Dispose()
        {
            _radioButton = null;
        }
    }
}

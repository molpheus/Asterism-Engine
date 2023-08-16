using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Asterism.UI
{
    public class UIElementRadioButtonGroup : UIElement, IUIElementAttribute, IDisposable
    {
        private RadioButtonGroup _group;

        public void Initialize(VisualElement visualElement)
        {
            _group = visualElement.SearchElement<RadioButtonGroup>(TagNameList, out var element);
            Element = element;

            Assert.IsNotNull(Element);
            Assert.IsNotNull(_group);
        }

        public void Dispose()
        {
            _group = null;
        }
    }
}

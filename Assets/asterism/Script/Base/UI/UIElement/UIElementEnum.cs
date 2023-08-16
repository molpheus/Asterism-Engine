using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Asterism.UI
{
    public class UIElementEnum : UIElement, IUIElementAttribute, IDisposable
    {
        private EnumField _enumField;

        public void Initialize(VisualElement visualElement)
        {
            _enumField = visualElement.SearchElement<EnumField>(TagNameList, out var element);
            Element = element;

            Assert.IsNotNull(Element);
            Assert.IsNotNull(_enumField);
        }

        public void Dispose()
        {
            _enumField = null;
        }
    }
}

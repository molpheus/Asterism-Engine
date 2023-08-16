using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Asterism.UI
{
    [Serializable]
    public class UIElementDropdown : UIElement, IUIElementAttribute, IDisposable
    {
        private DropdownField _dropDown;

        public string Value => _dropDown.value;
        public int Index => _dropDown.index;
        public List<string> Choice => _dropDown.choices;

        public void Initialize(VisualElement visualElement)
        {
            _dropDown = visualElement.SearchElement<DropdownField>(TagNameList, out var element);
            Element = element;

            Assert.IsNotNull(Element);
            Assert.IsNotNull(_dropDown);

        }

        public void Dispose()
        {
            _dropDown = null;
        }
    }
}

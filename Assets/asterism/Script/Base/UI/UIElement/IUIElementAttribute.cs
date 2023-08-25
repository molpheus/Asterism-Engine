using System;

using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Asterism.UI
{
    public interface IUIElementAttribute
    {
        void Initialize(VisualElement visualElement, string[] tagNameList = null);
    }
}

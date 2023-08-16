using System;

using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Asterism.UI
{
    public interface IUIElementAttribute
    {
        VisualElement Element { get; }
        void Initialize(VisualElement visualElement);
    }
}

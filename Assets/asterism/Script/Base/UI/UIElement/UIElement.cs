using UnityEngine;
using UnityEngine.UIElements;

namespace Asterism.UI
{
    public class UIElement
    {
        public string[] TagNameList { get => _tagNameList; set => _tagNameList = value; }
        [SerializeField]
        protected string[] _tagNameList;
        public VisualElement Element { get; protected set; }
    }
}

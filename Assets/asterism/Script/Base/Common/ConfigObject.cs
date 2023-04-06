using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asterism.Common
{
    [CreateAssetMenu(menuName = "ScriptableObject/Config")]
    public class ConfigObject : ScriptableObject
    {
        [Header("AddressableÝ’è")]
        public string AddressablePath = "AddressableData";

        //[SerializeField]
        //private bool m_ViewGraphy = false;
        //public bool ViewGraphy { get => m_ViewGraphy; set => m_ViewGraphy = value; }
    }
}
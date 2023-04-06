using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asterism.Engine
{
    public class EngineBase<T> : MonoBehaviour where T : EngineBase<T>
    {
        private T _instance;
        private void Awake()
        {
            if (ServiceLocator.IsRegistered<T>()) {
                Destroy(this);
                return;
            }
            _instance = FindObjectOfType<T>();
            ServiceLocator.Register<T>(_instance);
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<T>(_instance);
        }
    }
}

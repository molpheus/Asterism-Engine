using System.Collections;
using System.Collections.Generic;

using UnityEditor.Hardware;

using UnityEngine;

namespace Asterism.Engine
{
    [RequireComponent(typeof(ResourceReciver))]
    [RequireComponent(typeof(Debugger))]
    public class GameManager : EngineBase<GameManager>
    {
    }
}

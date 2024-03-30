using System.Collections.Generic;

namespace Asterism.Battle
{
    public interface IActionData
    {
        public int ActionType { get; set; }
        public int ActionId { get; set; }
        public bool IsForceExit { get; set; }
        public List<int> targetIdList { get; set; }
    }

    public interface IParameter
    {
        int ID { get; }
        bool IsCPU { get; }
        string Name { get; }

        NumVariable<int> LV { get; }

        NumVariable<int> HP { get; }
        NumVariable<int> MP { get; }
        NumVariable<int> SP { get; }
        NumVariable<int> EXP { get; }

        NumVariable<int> ATK { get; }
        NumVariable<int> DEF { get; }
        NumVariable<int> SPD { get; }
        NumVariable<int> HIT { get; }
        NumVariable<int> CRI { get; }
        NumVariable<int> LUK { get; }
    }
}

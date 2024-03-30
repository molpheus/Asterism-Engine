using Asterism.Common;

namespace Asterism.Battle.Turn
{
    public abstract class BattleActionId : Enumeration
    {
        public BattleActionId(int id, string name) : base(id, name)
        {

        }

        //public static IEnumerable<BattleActionId> GetItems() => GetItems<BattleActionId>();

        //public static BattleActionId Attack = new BattleActionId(1, nameof(Attack));

        //public static BattleActionId Item = new BattleActionId(2, nameof(Item));
        //public static BattleActionId Skill = new BattleActionId(3, nameof(Skill));

        //public static BattleActionId Back = new BattleActionId(99999, nameof(Back));
    }
}
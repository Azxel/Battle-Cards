using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCard_v2.Library
{
    public enum TypePlayer { Persona, FullDefense_FullAttack }
    public enum TypeCards { Soldier, Struct }
    public enum TypeAttribute { None, Attack, Defense }
    public enum TypeCondition { None, Have_Summoned_Cards, Have_Played_Cards, Bypass_Turn, Have_Attacked_Cards }

    interface IInteligence {
        void Summon(Board board, Hand hand, ref int mana, GameState state);
        void ActivateEffect(Board board, ref bool[] mask, GameState state);
        void Attack(Board board, Board boardOpponent, ref bool[] mask, ref int life, GameState state);
    }
    interface IComponents {
        Card this[int index] { get; set; }
        int Length { get; }
    }
}

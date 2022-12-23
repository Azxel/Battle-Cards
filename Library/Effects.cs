using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCard_v2.Library
{
    class Effect {
        public Effect(string condition, int conditionDependency, string attribute, int attributeDependency) {
            if (!IsValid(condition, conditionDependency, attribute, attributeDependency)) throw new Exception();
            this.condition = condition;
            this.conditionDependency = conditionDependency;
            this.attribute = attribute;
            this.attributeDependency = attributeDependency;   
        }

        public string Attribute { get { return attribute; } }
        public bool TryActivateEffect(GameState state) {
            if (condition == TypeCondition.Have_Summoned_Cards.ToString())  return conditionDependency <= state.Get_SummonedCards;
            if (condition == TypeCondition.Have_Played_Cards.ToString())    return conditionDependency <= state.Get_PlayedCards;
            if (condition == TypeCondition.Have_Attacked_Cards.ToString()) return conditionDependency <= state.Get_AttackedCards;
            if (condition == TypeCondition.Bypass_Turn.ToString())          return conditionDependency <= state.Get_Turns;
            return true;
        }
        public void ActivateEffect(ref int value) {
            value += attributeDependency;
        }
        public static bool IsValid(string condition, int conditionDependency, string attribute, int attributeDependency) {
            if (!MethNecesary.GetListCondition().Contains(condition)) return false;
            if (!MethNecesary.GetListAttribute().Contains(attribute)) return false;
            if (condition == TypeCondition.None.ToString() && conditionDependency != 0) return false;
            return (conditionDependency >= 0 && attributeDependency >= 0);  
        } 
        public string TransformEffectInString() {
            string temp = "";
            temp += condition + " ";
            temp += conditionDependency + " ";
            temp += attribute + " ";
            temp += attributeDependency + " ";
            return temp;
        }
        public override string ToString() {
            if (condition == TypeCondition.None.ToString()) {
                return attribute + " +" + attributeDependency;
            }
            string line = "if " + condition.Replace('_', ' ');
            line += " (" + conditionDependency + ")";
            line += " them " + attribute + " +" + attributeDependency;
            return line;
        }

        private string condition, attribute;
        private int conditionDependency, attributeDependency;
    }
}

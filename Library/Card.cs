using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCard_v2.Library
{
    abstract class Card : ICloneable {
        public abstract object Clone();

        public string Name { get { return name; } }
        public int Cost { get { return cost; } }

        public abstract string GetTypeCard { get; }
        public abstract string TransformCardToTXT();
        public abstract bool TryActivateEffect(GameState state);
        public abstract void ActivateEffect();

        protected string name;
        protected int cost;
        protected Effect effect;
    }
    class Soldier : Card {
        public Soldier(string name, int cost, int attack, int defense, Effect effect) {
            this.name = name;
            this.cost = cost;
            this.attack = attack;
            this.defense = defense;
            this.effect = effect;
        }
             
        public int attack, defense; 

        public override object Clone() => new Soldier(name, cost, attack, defense, effect);
        public override string ToString() => "Name: " + name + " (" + cost + ")\nAttack: " + attack + "\nDefense: " + defense + "\nEffect: " + effect.ToString();
        public override string GetTypeCard { get { return TypeCards.Soldier.ToString(); } }
        public override string TransformCardToTXT() {
            string transformation = "";
            transformation += GetTypeCard + " ";
            transformation += name.Replace(' ', '_') + " ";
            transformation += cost + " ";
            transformation += attack + " ";
            transformation += defense + " ";
            transformation += effect.TransformEffectInString();
            return transformation;
        } 
        public override bool TryActivateEffect(GameState state) => effect.TryActivateEffect(state);
        public override void ActivateEffect() {
            if (effect.Attribute == TypeAttribute.Attack.ToString()) effect.ActivateEffect(ref attack);
            if (effect.Attribute == TypeAttribute.Defense.ToString()) effect.ActivateEffect(ref defense);
            
            if (attack > CardIsValid.Attack_Defense) attack = CardIsValid.Attack_Defense;
            if (defense > CardIsValid.Attack_Defense) defense = CardIsValid.Attack_Defense;
        }
    }
    class Struct : Card {
        public Struct(string name, int cost, int defense, Effect effect) {
            this.name = name;
            this.cost = cost;
            this.defense = defense;
            this.effect = effect;
        }

        public int defense;

        public override object Clone() => new Struct(name, cost, defense, effect);
        public override string ToString() => "Name: " + name + " (" + cost + ")\nDefense: " + defense + "\nEffect: " + effect.ToString(); 
        public override string GetTypeCard { get { return TypeCards.Struct.ToString(); } }
        public override string TransformCardToTXT() {
            string transformation = "";
            transformation += GetTypeCard + " ";
            transformation += name.Replace(' ', '_') + " ";
            transformation += cost + " ";
            transformation += defense + " ";
            transformation += effect.TransformEffectInString();
            return transformation;
        } 
        public override bool TryActivateEffect(GameState state) => effect.TryActivateEffect(state);
        public override void ActivateEffect() {
            if (effect.Attribute == TypeAttribute.Defense.ToString()) effect.ActivateEffect(ref defense);

            if (defense > CardIsValid.Attack_Defense) defense = CardIsValid.Attack_Defense;
        }
    }
    class CardIsValid {
        private const int attack_defense = 99;
        private const int cost = 10;

        static public bool IsAttackDefense(int value) {
            if (value < 0) return false;
            if (value > attack_defense) return false;
            return true;
        }
        static public bool IsCost(int value) {
            if (value < 0) return false;
            if (value > cost) return false;
            return true;
        }
        static public bool IsName(string name) => (name != "");

        static public int Attack_Defense { get { return attack_defense; } }
        static public int Cost { get { return cost; } }
    }
}

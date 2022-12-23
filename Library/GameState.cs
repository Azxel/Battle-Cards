using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCard_v2.Library
{
    class GameState {
        public GameState() {
            AttackedCards = 0;
            SummonedCards = 0;
            PlayedCards = 0;
            Turns = 0;
        }

        public void Increase_AttackedCards(int n = 1) { AttackedCards += n; }
        public void Increase_SummonedCards(int n = 1) { SummonedCards += n; }
        public void Increase_PlayedCards(int n = 1) { PlayedCards += n; }
        public void Increase_Turns() { Turns++; }

        public int Get_AttackedCards { get { return AttackedCards; } }
        public int Get_SummonedCards { get { return SummonedCards; } }
        public int Get_PlayedCards { get { return PlayedCards; } }
        public int Get_Turns { get { return Turns; } }

        private int AttackedCards, SummonedCards, PlayedCards, Turns;
    }
    class Rules {
        public Rules(int maxLife, int maxMana, int maxBoard, int maxHand) {
            if (!(RulesIsValid.IsLife(maxLife) && RulesIsValid.IsMana(maxMana) && RulesIsValid.IsBoard(maxBoard) && RulesIsValid.IsHand(maxHand))) throw new Exception("Primero verifique que estan en el rango los valores");
            this.maxLife = maxLife;
            this.maxMana = maxMana;
            this.maxBoard = maxBoard;
            this.maxHand = maxHand;
        }

        public int GetMaxLife { get { return maxLife; } }
        public int GetMaxMana { get { return maxMana; } }
        public int GetMaxBoard { get { return maxBoard; } }
        public int GetMaxHand { get { return maxHand; } }

        private int maxLife, maxMana, maxBoard, maxHand;
    }
    class RulesIsValid {
        private const int maxLife = 20;
        private const int minLife = 10;
        private const int maxMana = 20;
        private const int minMana = 10;
        private const int maxBoard = 6;
        private const int minBoard = 2;
        private const int maxHand = 5;
        private const int minHand = 3;

        static public bool IsLife(int value) => (minLife <= value && value <= maxLife);
        static public bool IsMana(int value) => (minMana <= value && value <= maxMana);
        static public bool IsBoard(int value) => (minBoard <= value && value <= maxBoard);
        static public bool IsHand(int value) => (minHand <= value && value <= maxHand);

        static public string GetIntervalLife { get { return minLife + "-" + maxLife; } }
        static public string GetIntervalMana { get { return minMana + "-" + maxMana; } }
        static public string GetIntervalBoard { get { return minBoard + "-" + maxBoard; } }
        static public string GetIntervalHand { get { return minHand + "-" + maxHand; } }
    }
}

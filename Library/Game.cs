using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCard_v2.Library
{
    class Game {
        public Game(Deck deck1, Deck deck2, Rules rules) {
            this.rules = rules;
            state = new GameState();
            p1 = new Persona(deck1, rules.GetMaxLife, rules.GetMaxHand, rules.GetMaxBoard);
            p2 = new Persona(deck2, rules.GetMaxLife, rules.GetMaxHand, rules.GetMaxBoard);
        }
        public Game(Deck deck1, Deck deck2, Rules rules, IInteligence inteligence1) {
            this.rules = rules;
            state = new GameState();
            p1 = new Persona(deck1, rules.GetMaxLife, rules.GetMaxHand, rules.GetMaxBoard);
            p2 = new Virtual(deck2, rules.GetMaxLife, rules.GetMaxHand, rules.GetMaxBoard, inteligence1);
        }
        public Game(Deck deck1, Deck deck2, Rules rules, IInteligence inteligence1, IInteligence inteligence2) {
            this.rules = rules;
            state = new GameState();
            p1 = new Virtual(deck1, rules.GetMaxLife, rules.GetMaxHand, rules.GetMaxBoard, inteligence1);
            p2 = new Virtual(deck2, rules.GetMaxLife, rules.GetMaxHand, rules.GetMaxBoard, inteligence2);
        }

        public void NextTurn() {
            if (state.Get_Turns == 0) {
                p1.DrawCards(rules.GetMaxHand - 2);
                p2.DrawCards(rules.GetMaxHand - 2);
                p1.life = rules.GetMaxLife;
                p2.life = rules.GetMaxLife;
                lifeOpponent = rules.GetMaxLife;
            }
            state.Increase_Turns();
            if (state.Get_Turns % 2 == 0) { 
                p2.life = lifeOpponent;
                lifeOpponent = p1.life;
                boardOpponent = p1.board;
                p = p2;
                namePlayer = nameP2;
                nameOpponent = nameP1;
            } else { 
                p1.life = lifeOpponent;
                lifeOpponent = p2.life;
                boardOpponent = p2.board;
                p = p1;
                namePlayer = nameP1;
                nameOpponent = nameP2;
            }
            p.mana = rules.GetMaxMana;
            p.DrawCards(1);
            maskEffect = p.board.CardsThatCanActivateEffect();
            maskAttack = p.board.SoldierThatCanAttack();
        }

        public GameState GetState { get { return state; } }
        public Player GetPlayer { get { return p; } }
        public string GetNamePlayer { get { return namePlayer; } }
        public string GetOpponentPlayer { get { return nameOpponent; } }
        public Board GetBoardOpponent { get { return boardOpponent; } }

        public int lifeOpponent;
        public bool[] maskEffect, maskAttack;

        private string namePlayer, nameOpponent;
        private string nameP1 = "Jugador 1", nameP2 = "Jugador 2";
        private Rules rules;
        private GameState state;
        private Player p, p1, p2;
        private Board boardOpponent;
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCard_v2.Library
{
    abstract class Player {
        public void DrawCards(int n) {
            for (int i = 0; i < n; i++) {
                if (deck.Length == 0) deck = (Deck)deck.Clone();
                hand.DrawCard(deck);
            }
        }
        public int life, mana;
        public Board board;
        public Hand hand;
        protected Deck deck;
    }

    class Persona : Player {
        public Persona(Deck deck, int maxLife, int maxHand, int maxBoard) {
            this.deck = deck;
            life = maxLife;
            mana = 0;
            hand = new Hand(maxHand);
            board = new Board(maxBoard);   
        }

        public void Summon(int posBoard, int posHand, GameState state) {
            if (TryAction.Summon(board, hand, posBoard, posHand, mana)) {
                Action.Summon(board, hand, posBoard, posHand, ref mana);
                state.Increase_SummonedCards();
            } 
        }
        public void ActivateEffect(int posBoard, GameState state, ref bool[] mask) {
            if (TryAction.ActivateEffect(board, posBoard, mask) && board[posBoard].TryActivateEffect(state)) {
                Action.ActivateEffect(board, posBoard, ref mask);
                state.Increase_PlayedCards();
            } 
        }
        public void Attack(Board boardOpponent, int posBoard, int posBoardOpoonent, ref bool[] mask, GameState state) {
            if (TryAction.Attack(board, boardOpponent, posBoard, posBoardOpoonent, mask)) {
                Action.Attack(board, boardOpponent, posBoard, posBoardOpoonent, ref mask);
                state.Increase_AttackedCards();
            } 
        }
        public void Attack(Board boardOpponent, int posBoard, ref bool[] mask, GameState state) {
            if (TryAction.Attack(board, boardOpponent, posBoard, mask)) {
                Action.Attack(board, boardOpponent, posBoard, ref mask);
                state.Increase_AttackedCards();
            } 
        }
        public void Attack(int posBoard, ref bool[] mask, ref int life, GameState state) {
            if (TryAction.Attack(board, posBoard, mask)) {
                Action.Attack(board, posBoard, ref mask, ref life);
                state.Increase_AttackedCards();
            } 
        }
    }  
    class Virtual : Player {
        public Virtual(Deck deck, int maxLife, int maxHand, int maxBoard, IInteligence inteligence) {
            this.deck = deck;
            life = maxLife;
            mana = 0;
            hand = new Hand(maxHand);
            board = new Board(maxBoard);
            this.inteligence = inteligence;
        }

        public void Summon(GameState state) {
            inteligence.Summon(board, hand, ref mana, state);
        }
        public void ActivateEffect(ref bool[] mask, GameState state) {
            inteligence.ActivateEffect(board, ref mask, state);
        }
        public void Attack(Board boardOpponent, ref bool[] mask, ref int life, GameState state) {
            inteligence.Attack(board, boardOpponent, ref mask, ref life, state);
        }

        private IInteligence inteligence;
    }
    class FullDefenseFullAttack : IInteligence {
        public void Summon(Board board, Hand hand, ref int mana, GameState state) {
            if (!board.HaveStruct() && hand.HaveStruct() && TryAction.Summon(board, hand, 0, hand.GetStructPosition(), mana)) {
                Action.Summon(board, hand, 0, hand.GetStructPosition(), ref mana);
                state.Increase_SummonedCards();
            }
            if (board.GetEmptyPosition() != -1) {
                for (int i = 0; i < hand.Length; i++) {
                    if (TryAction.Summon(board, hand, board.GetEmptyPosition(), i, mana)) {
                        Action.Summon(board, hand, board.GetEmptyPosition(), i, ref mana);
                        state.Increase_SummonedCards();
                    } 
                }
            }
        } 
        public void ActivateEffect(Board board, ref bool[] mask, GameState state) {
            for (int i = 0; i < mask.Length; i++) {
                if (mask[i] && board[i].TryActivateEffect(state)) {
                    board[i].ActivateEffect();
                    mask[i] = false;
                    state.Increase_PlayedCards();
                }
            }
        }
        public void Attack(Board board, Board boardOpponent, ref bool[] mask, ref int life, GameState state) {
            for (int i = 0; i < mask.Length; i++) {
                if (boardOpponent.HaveSoldier()) for (int j = 1; j < boardOpponent.Length; j++) if (boardOpponent[j] is Soldier) if (TryAction.Attack(board, boardOpponent, i, j, mask)) {
                                Action.Attack(board, boardOpponent, i, j, ref mask);
                                state.Increase_PlayedCards();
                            }
                if (TryAction.Attack(board, boardOpponent, i, mask)) {
                    Action.Attack(board, boardOpponent, i, ref mask);
                    state.Increase_PlayedCards();
                } 
                if (TryAction.Attack(board, i, mask)) {
                    Action.Attack(board, i, ref mask, ref life);
                    state.Increase_PlayedCards();
                }
            }
        }
    }
}

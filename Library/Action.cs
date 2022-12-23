using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCard_v2.Library
{
    class Action {
        public static void Summon(Board board, Hand hand, int posBoard, int posHand, ref int mana) {
            board[posBoard] = hand[posHand];
            mana -= board[posBoard].Cost;
            hand[posHand] = null;
        }
        public static void ActivateEffect(Board board, int posBoard, ref bool[] mask) {
            board[posBoard].ActivateEffect();
            mask[posBoard] = false;
        }
        public static void Attack(Board board, Board boardOpponent, int posBoard, int posBoardOpponent, ref bool[] mask) {
            ((Soldier)boardOpponent[posBoardOpponent]).defense -= ((Soldier)board[posBoard]).attack;
            ((Soldier)board[posBoard]).defense -= ((Soldier)boardOpponent[posBoardOpponent]).attack;
            mask[posBoard] = false;
            if (((Soldier)board[posBoard]).defense <= 0) board[posBoard] = null;
            if (((Soldier)boardOpponent[posBoardOpponent]).defense <= 0) boardOpponent[posBoardOpponent] = null;

        }
        public static void Attack(Board board, Board boardOpponent, int posBoard, ref bool[] mask) {
            ((Struct)boardOpponent[0]).defense -= ((Soldier)board[posBoard]).attack;
            mask[posBoard] = false;
            if (((Struct)boardOpponent[0]).defense <= 0) boardOpponent[0] = null;
        }
        public static void Attack(Board board, int posBoard, ref bool[] mask, ref int life) {
            life -= ((Soldier)board[posBoard]).attack;
            mask[posBoard] = false;
        }
    }
    class TryAction {
        public static bool Summon(Board board, Hand hand, int posBoard, int posHand, int mana) {
            if (!IsValid(board.Length, posBoard)) return false;
            if (!IsValid(hand.Length, posHand)) return false;
            if (posBoard != 0 && hand[posHand] is Struct) return false; 
            if (board[posBoard] != null) return false;
            if (hand[posHand] == null) return false;
            if (hand[posHand].Cost > mana) return false; 
            return true;
        }
        public static bool ActivateEffect(Board board, int posBoard, bool[] mask) {
            if (!IsValid(board.Length, posBoard)) return false;
            if (board[posBoard] == null) return false;
            if (!mask[posBoard]) return false;
            return true;
        }
        public static bool Attack(Board board, Board boardOpponent, int posBoard, int posBoardOpponent, bool[] mask) {
            if (!IsValid(board.Length, posBoard)) return false;
            if (!IsValid(boardOpponent.Length, posBoardOpponent)) return false;
            if (!mask[posBoard]) return false;
            return (board[posBoard] is Soldier && boardOpponent[posBoardOpponent] is Soldier);
        }
        public static bool Attack(Board board, Board boardOpponent, int posBoard, bool[] mask) {
            if (!IsValid(board.Length, posBoard)) return false;
            if (!mask[posBoard]) return false;
            return (board[posBoard] is Soldier && boardOpponent[0] is Struct);
        }
        public static bool Attack(Board board, int posBoard, bool[] mask) { 
            if (!IsValid(board.Length, posBoard)) return false;
            if (!mask[posBoard]) return false;
            return (board[posBoard] is Soldier); 
        }
        private static bool IsValid(int length, int pos) => (0 <= pos && pos < length);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCard_v2.Library
{
    class Board : IComponents {
        public Board(int length) {
            board = new Card[length];
        }

        public Card this[int index] {
            get {
                if (IsIndexValid(index)) throw new Exception("index incorrect");
                return board[index];
            }
            set {
                if (IsIndexValid(index)) throw new Exception("index incorrect");
                if (value is Card || value == null) board[index] = value;
            }
        }
        public int Length { get { return board.Length; } }
        public bool HaveStruct() => board[0] is Struct;
        public bool HaveSoldier() {
            for (int i = 0; i < board.Length; i++) {
                if (board[i] is Soldier) return true;
            } return false;
        }
        public bool[] SoldierThatCanAttack() {
            bool[] mask = new bool[board.Length];
            for (int i = 0; i < board.Length; i++) {
                if (board[i] is Soldier) mask[i] = true;
                else mask[i] = false;
            } return mask;
        }
        public bool[] CardsThatCanActivateEffect() {
            bool[] mask = new bool[board.Length];
            for (int i = 0; i < mask.Length; i++) {
                if (board[i] != null) mask[i] = true;
                else mask[i] = false;
            } return mask;
        }
        private bool IsIndexValid(int index) => index < 0 || index > board.Length;
        public int GetEmptyPosition() {
            for (int i = 1; i < board.Length; i++) {
                if (board[i] == null) return i;
            } return -1;
        }


        private Card[] board;
    }
    class Hand : IComponents {
        public Hand(int length) {
            hand = new Card[length];
        }

        public Card this[int index] {
            get {
                if (IsIndexValid(index)) throw new Exception("index incorrect");
                return hand[index];
            }
            set {
                if (IsIndexValid(index)) throw new Exception("index incorrect");
                if (value is Card || value == null) hand[index] = value;
            }
        }
        public int Length { get { return hand.Length; } }
        public bool HaveStruct() {
            foreach (Card card in hand) {
                if (card is Struct) return true;
            } return false;
        }
        public int GetStructPosition() {
            for (int i = 0; i < hand.Length; i++) {
                if (hand[i] is Struct) return i;
            } return -1;
        }
        public List<int> GetStructPositions() {
            List<int> list = new List<int>();
            for (int i = 0; i < hand.Length; i++) {
                if (hand[i] is Struct) list.Add(i);
            } return list;
        }
        public void DrawCard(Deck deck) {
            int index = GetEmptyPosition();
            if (index != -1) {
                hand[index] = deck[0];
                deck.Remove();
            }
        }
        private bool IsIndexValid(int index) => index < 0 || index > hand.Length;
        private int GetEmptyPosition() {
            for (int i = 0; i < hand.Length; i++) {
                if (hand[i] == null) return i;
            } return -1;
        }
        private List<int> GetEmptyPositions() {
            List<int> list = new List<int>();
            for (int i = 0; i < hand.Length; i++) {
                if (hand[i] == null) list.Add(i);
            } return list;
        }

        private Card[] hand;
    }
    class Deck : ICloneable, IComponents {
        public Deck(List<Card> deck) {
            if (!(5 < deck.Count && deck.Count <= 20)) throw new Exception();
            this.deck = new List<Card>();
            deckOriginal = new List<Card>();
            foreach(Card card in deck) {
                this.deck.Add((Card)card.Clone());
                deckOriginal.Add((Card)card.Clone());
            } 
        }

        public Card this[int index] {
            get {
                if (IsIndexValid(index)) throw new Exception("index incorrect");
                return deck[index];
            }
            set {
                if (IsIndexValid(index)) throw new Exception("index incorrect");
                if (value is Card || value == null) deck[index] = value;
            }
        }
        public bool TryAdd() => maxCount != deck.Count;
        public void Add(Card card) {
            if (TryAdd()) deck.Add(card);
            throw new Exception("no puede adicionarse otra carta al deck");
        }
        public int Length { get { return deck.Count; } }
        public void Randomizer() {
            Random r = new Random();
            deck = deck.OrderBy(_ => r.Next()).ToList();
        }
        private bool IsIndexValid(int index) => index < 0 || index > deck.Count;
        public void Remove() {
            deck.RemoveAt(0);
        }
        public object Clone() {
            deck.Clear();
            foreach(Card card in deckOriginal) {
                deck.Add((Card)card.Clone());
            } return deck;
        }

        private const int maxCount = 20;
        private List<Card> deck, deckOriginal;
    }
}

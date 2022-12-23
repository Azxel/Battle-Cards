using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleCard_v2.Library;

namespace BattleCard_v2
{
    class BattleCard {
        public static void RunProgram() {
            // Cargar archivos
            string dir = ReadFiles.Seach_dirProject("BattleCard_v2");
            string dirBD = ReadFiles.Seach_dirProjectFolder(dir, "Database");
            string dirDC = ReadFiles.Seach_dirProjectFolder(dir, "Deck");
            List<Card> BD = ReadFiles.LoadDataBaseCard(dirBD);
            List<Deck> DC = ReadFiles.LoadDeck(dirDC);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" ~ Battle Cards ~ ");
            Console.ForegroundColor = ConsoleColor.White;

            string options = "Elija una opcion:\n1. Ejecutar juego\n2. Construir una carta\n3. Mostrar cartas\n4. Mostrar mazos";
            int answer = GetInfo(options);

            Console.Clear();
            switch (answer) {
                case 1:
                    Game game = ConfigGame(DC);
                    RunGame(game);
                    RunProgram();
                    break;
                case 2:
                    BuildCards(dirBD);
                    RunProgram();
                    break;
                case 3:
                    PrintDB(BD);
                    RunProgram();
                    break;
                case 4:
                    PrintDeck(DC);
                    RunProgram();
                    break;
            }

        }
        #region RunGame 
        public static Game ConfigGame(List<Deck> DC) {
            Deck d1 = ChooseDeck(DC, "Elija el primer deck con el cual jugar");
            Deck d2 = ChooseDeck(DC, "Elija el segundo deck con el cual jugar");
            Rules rules = ChooseRules();

            Console.Clear();
            int iterator, p1, p2;
            do {
                iterator = 0;
                foreach (string option in MethNecesary.GetListPlayer()) {
                    Console.WriteLine((++iterator) + ". " + option);
                } p1 = GetInfo("Quien jugara en el primer turno y con el primer deck: ");
            } while (!(0 < p1 && p1 <= MethNecesary.GetListPlayer().Count));
            Console.Clear();
            do {
                iterator = 0;
                foreach (string option in MethNecesary.GetListPlayer()) {
                    Console.WriteLine((++iterator) + ". " + option);
                } p2 = GetInfo("Quien jugara en el segundo turno y con el segundo deck: ");
            } while (!(0 < p2 && p2 <= MethNecesary.GetListPlayer().Count));

            d2.Randomizer();
            d1.Randomizer();

            if (p1 == 1 && p2 == 2) return new Game(d1, d2, rules, new FullDefenseFullAttack());
            if (p1 == 2 && p2 == 1) return new Game(d2, d1, rules, new FullDefenseFullAttack());
            if (p1 == 2 && p2 == 2) return new Game(d1, d2, rules, new FullDefenseFullAttack(), new FullDefenseFullAttack());
            return new Game(d1, d2, rules); // si llega aqui el juego es persona contra persona
        }
        public static Deck ChooseDeck(List<Deck> DC, string line) {
            Console.Clear();
            int answer = 0, iterator = 0;
            do {
                foreach (Deck deck in DC) {
                    Console.WriteLine("Mazo " + (++iterator));
                    PrintComponent(deck, ConsoleColor.DarkGreen);
                } answer = GetInfo(line);
            } while (!(0 < answer && answer <= DC.Count));
            return DC[answer - 1];
        }
        public static Rules ChooseRules() {
            return new Rules(15, 15, 5, 5); 
        }

        public static void RunGame(Game game) {
            Console.Clear();
            while (true) {
                game.NextTurn();
                PrintState(game);
                Console.ReadKey();
                Console.Clear();

                if (game.GetPlayer is Persona) { // Codigo de persona 
                    Persona p = (Persona)game.GetPlayer;

                    int answer = 0;
                    do {
                        do {
                            answer = GetInfo("Elija:\n1. Invocar\n2. Activar Efectos\n3. Atacar a Lider\n4. Atacar a la Estructura\n5. Atacar a otra Carta\n6. Finalizar turno");
                        } while (!(0 < answer && answer <= 6));

                        int posHand = -1, posBoard = -1, posBoardOpponent = -1;
                        bool isPosible;
                        switch (answer) {
                            case 1:
                                Console.WriteLine("CAMPO");
                                PrintComponent(p.board, ConsoleColor.DarkRed);
                                Console.WriteLine("MANO");
                                PrintComponent(p.hand, ConsoleColor.DarkGreen);
                                PrintState(game);

                                do {
                                    Console.WriteLine("Elija la carta de la mano a invocar (introducir posicion): ");
                                    isPosible = int.TryParse(Console.ReadLine(), out posHand);
                                } while (!isPosible);

                                do {
                                    Console.WriteLine("Elija la posicion del campo donde invocar la carta: ");
                                    isPosible = int.TryParse(Console.ReadLine(), out posBoard);
                                } while (!isPosible);

                                p.Summon(posBoard, posHand, game.GetState);

                                Console.WriteLine("... Invocando ...");
                                Console.WriteLine("CAMPO");
                                PrintComponent(p.board, ConsoleColor.DarkRed);
                                Console.WriteLine("MANO");
                                PrintComponent(p.hand, ConsoleColor.DarkGreen);
                                PrintState(game);
                                break;
                            case 2:
                                Console.WriteLine("CAMPO");
                                PrintComponent(p.board, ConsoleColor.DarkRed);
                                PrintState(game);

                                do {
                                    Console.WriteLine("Elija la carta que desea activar su efecto: ");
                                    isPosible = int.TryParse(Console.ReadLine(), out posBoard);
                                } while (!isPosible);

                                p.ActivateEffect(posBoard, game.GetState, ref game.maskEffect);

                                Console.WriteLine("... Activando efectos ...");
                                Console.WriteLine("CAMPO");
                                PrintComponent(p.board, ConsoleColor.DarkRed);
                                PrintState(game);
                                break;
                            case 3:
                                Console.WriteLine("CAMPO");
                                PrintComponent(p.board, ConsoleColor.DarkRed);
                                Console.WriteLine("CAMPO ENEMIGO");
                                PrintComponent(game.GetBoardOpponent, ConsoleColor.DarkYellow);
                                PrintState(game);

                                do {
                                    Console.WriteLine("Elija la carta que desea que ataque al lider enemigo: ");
                                    isPosible = int.TryParse(Console.ReadLine(), out posBoard);
                                } while (!isPosible);

                                p.Attack(posBoard, ref game.maskAttack, ref game.lifeOpponent, game.GetState);

                                Console.WriteLine("... Atacando ...");
                                Console.WriteLine("CAMPO");
                                PrintComponent(p.board, ConsoleColor.DarkRed);
                                Console.WriteLine("CAMPO ENEMIGO");
                                PrintComponent(game.GetBoardOpponent, ConsoleColor.DarkYellow);
                                PrintState(game);
                                break;
                            case 4:
                                Console.WriteLine("CAMPO");
                                PrintComponent(p.board, ConsoleColor.DarkRed);
                                Console.WriteLine("CAMPO ENEMIGO");
                                PrintComponent(game.GetBoardOpponent, ConsoleColor.DarkYellow);
                                PrintState(game);

                                do {
                                    Console.WriteLine("Elija la carta con la que desea atacar a la Estructura enemiga: ");
                                    isPosible = int.TryParse(Console.ReadLine(), out posBoard);
                                } while (!isPosible);

                                p.Attack(game.GetBoardOpponent, posBoard, ref game.maskAttack, game.GetState);

                                Console.WriteLine("... Atacando ...");
                                Console.WriteLine("CAMPO");
                                PrintComponent(p.board, ConsoleColor.DarkRed);
                                Console.WriteLine("CAMPO ENEMIGO");
                                PrintComponent(game.GetBoardOpponent, ConsoleColor.DarkYellow);
                                PrintState(game);
                                break;
                            case 5:
                                Console.WriteLine("CAMPO");
                                PrintComponent(p.board, ConsoleColor.DarkRed);
                                Console.WriteLine("CAMPO ENEMIGO");
                                PrintComponent(game.GetBoardOpponent, ConsoleColor.DarkYellow);
                                PrintState(game);

                                do {
                                    Console.WriteLine("Elija la carta con la que desea atacar: ");
                                    isPosible = int.TryParse(Console.ReadLine(), out posBoard);
                                } while (!isPosible);
                                do {
                                    Console.WriteLine("Elija la carta objetivo: ");
                                    isPosible = int.TryParse(Console.ReadLine(), out posBoardOpponent);
                                } while (!isPosible);

                                p.Attack(game.GetBoardOpponent, posBoard, posBoardOpponent, ref game.maskAttack, game.GetState);

                                Console.WriteLine("... Atacando ...");
                                Console.WriteLine("CAMPO");
                                PrintComponent(p.board, ConsoleColor.DarkRed);
                                Console.WriteLine("CAMPO ENEMIGO");
                                PrintComponent(game.GetBoardOpponent, ConsoleColor.DarkYellow);
                                PrintState(game);
                                break;
                        }
                        if (game.lifeOpponent <= 0) break;
                    } while (answer != 6);
                    if (game.lifeOpponent <= 0) break;
                }

                if (game.GetPlayer is Virtual) { 
                    Virtual v = (Virtual)game.GetPlayer;

                    Console.WriteLine("CAMPO");
                    PrintComponent(v.board, ConsoleColor.DarkRed);
                    Console.WriteLine("MANO");
                    PrintComponent(v.hand, ConsoleColor.DarkGreen);
                    PrintState(game);
                    Console.WriteLine("... Invocando ...");
                    v.Summon(game.GetState);
                    Console.WriteLine("CAMPO");
                    PrintComponent(v.board, ConsoleColor.DarkRed);
                    Console.WriteLine("MANO");
                    PrintComponent(v.hand, ConsoleColor.DarkGreen);
                    PrintState(game);

                    Console.ReadKey();
                    Console.Clear();

                    Console.WriteLine("CAMPO");
                    PrintComponent(v.board, ConsoleColor.DarkRed);
                    PrintState(game);
                    Console.WriteLine("... Activando efectos ...");
                    v.ActivateEffect(ref game.maskEffect, game.GetState);
                    Console.WriteLine("CAMPO");
                    PrintComponent(v.board, ConsoleColor.DarkRed);
                    PrintState(game);

                    Console.ReadKey();
                    Console.Clear();

                    Console.WriteLine("CAMPO");
                    PrintComponent(v.board, ConsoleColor.DarkRed);
                    Console.WriteLine("CAMPO ENEMIGO");
                    PrintComponent(game.GetBoardOpponent, ConsoleColor.DarkYellow);
                    PrintState(game);
                    Console.WriteLine("... Atacando ...");
                    v.Attack(game.GetBoardOpponent, ref game.maskAttack, ref game.lifeOpponent, game.GetState);
                    if (game.lifeOpponent <= 0) break;
                    Console.WriteLine("CAMPO");
                    PrintComponent(v.board, ConsoleColor.DarkRed);
                    Console.WriteLine("CAMPO ENEMIGO");
                    PrintComponent(game.GetBoardOpponent, ConsoleColor.DarkYellow);
                    PrintState(game);

                    Console.ReadKey();
                    Console.Clear();
                }
            }
            Console.Clear();
            PrintState(game);
            Console.WriteLine("Existe Ganador!!!!!");
            Console.WriteLine("Ganador: " + game.GetNamePlayer);
            Console.ReadKey();
            Console.Clear();
        }
        #endregion

        #region BuildCards 
        public static void BuildCards(string dirBD) {
            List<string> listOption = MethNecesary.GetListCards();
            string line = "", answerLine = "", name;
            int iterator = 1, answerNumber = 0, cost;

            foreach (string option in listOption) {
                Console.WriteLine((iterator++) + ". " + option);
            }
            do {
                Console.WriteLine("Introduzca el tipo de la carta a crear: ");
            } while (!int.TryParse(Console.ReadLine(), out iterator) || iterator <= 0);

            line = "De un nombre a la carta";
            do {
                Console.Clear();
                Console.WriteLine(line);
                answerLine = Console.ReadLine();
            } while (!CardIsValid.IsName(answerLine));
            name = answerLine;

            line = $"De un coste a la carta\nMax: {CardIsValid.Cost}";
            do {
                Console.Clear();
                answerNumber = GetInfo(line);
            } while (!CardIsValid.IsCost(answerNumber));
            cost = answerNumber;

            Console.Clear();
            Effect effect = BuildEffect();


            int attack, defense;
            switch (iterator) {
                case 1: // Soldier
                    // Definiendo ataque
                    do {
                        Console.Clear();
                        attack = GetInfo("Defina el ataque de la carta:\nMax: " + CardIsValid.Attack_Defense);
                    } while (!CardIsValid.IsAttackDefense(attack));
                    // Definiendo defensa
                    do {
                        Console.Clear();
                        defense = GetInfo("Defina la defensa de la carta:\nMax: " + CardIsValid.Attack_Defense);
                    } while (!CardIsValid.IsAttackDefense(attack));

                    Soldier _soldier = new Soldier(name, cost, attack, defense, effect);
                    Console.WriteLine(_soldier.TransformCardToTXT()); Console.ReadKey();
                    ReadFiles.TransformCardToText(_soldier, dirBD);
                    break;
                case 2: // Struct 
                    do {
                        Console.Clear();
                        defense = GetInfo("Defina la defensa de la carta:\nMax: " + CardIsValid.Attack_Defense);
                    } while (!CardIsValid.IsAttackDefense(defense));

                    Struct _struct = new Struct(name, cost, defense, effect);
                    Console.WriteLine(_struct.TransformCardToTXT()); Console.ReadKey();
                    ReadFiles.TransformCardToText(_struct, dirBD);
                    break;
            } Console.Clear();
        }
        public static Effect BuildEffect() {
            Console.Clear();
            Console.WriteLine("Construyendo Efecto\n");
            string line = "", condition, attribute;
            int iterator, answer = 0, dependenceCondition, dependenceAttribute;

            line = "Elije una condicion: ";
            // Construyendo condicion
            do {
                iterator = 0;
                foreach (string _condition in MethNecesary.GetListCondition()) {
                    Console.WriteLine(++iterator + " - " + _condition.Replace('_', ' '));
                } answer = GetInfo(line);
            } while (!(0 < answer && answer <= MethNecesary.CountTypeCondition()));
            condition = MethNecesary.GetListCondition()[answer - 1];
            Console.Clear();
            line = "Eije un atributo: ";
            // Elijiendo atributo 
            do {
                iterator = 0;
                foreach (string _attribute in MethNecesary.GetListAttribute()) {
                    Console.WriteLine(++iterator + " - " + _attribute.Replace('_', ' '));
                } answer = GetInfo(line);
            } while(!(0 < answer && answer <= MethNecesary.CountTypeAttribute()));
            attribute = MethNecesary.GetListAttribute()[answer - 1];
            Console.Clear();

            line = "Defina un valor entre 0 y " + CardIsValid.Attack_Defense + " para la condicion";
            // Definiendo el valor de dependencia para la condicion 
            do {
                answer = GetInfo(line);
            } while (!(0 <= answer && answer <= CardIsValid.Attack_Defense));
            dependenceCondition = answer;
            Console.Clear();
            line = "Defina un valor entre 0 y " + CardIsValid.Attack_Defense + " para el atributo";
            // Definiendo el valor de dependencia para el atributo
            do {
                answer = GetInfo(line);
            } while (!(0 <= answer && answer <= CardIsValid.Attack_Defense));
            dependenceAttribute = answer;

            if (Effect.IsValid(condition, dependenceCondition, attribute, dependenceAttribute)) return new Effect(condition, dependenceCondition, attribute, dependenceAttribute);
            Console.WriteLine("No se pudo construir el efecto");
            return BuildEffect();
        }
        #endregion

        public static int GetInfo(string line) {
            bool isValid;
            int answer;
            do {
                Console.WriteLine(line);
                isValid = int.TryParse(Console.ReadLine(), out answer);
            } while (!isValid);
            return answer;
        }

        public static void PrintComponent(IComponents component, ConsoleColor color) {
            for (int i = 0; i < component.Length; i++) {
                if (component[i] != null) {
                    Console.ForegroundColor = color;
                    Console.WriteLine("Position: " + i + "\nType: " + component[i].GetTypeCard);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(component[i].ToString());
                } else {
                    Console.ForegroundColor = color;
                    Console.WriteLine("Position: " + i + " ~~~~~VACIO~~~~~");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
        public static void PrintState(Game game) {
            Console.WriteLine("~~~~~ Estadisticas del juego ~~~~~");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Vida del " + game.GetNamePlayer + ": " + game.GetPlayer.life);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Mana del " + game.GetNamePlayer + ": " + game.GetPlayer.mana);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Vida del Oponente " + game.GetOpponentPlayer + ": " + game.lifeOpponent);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("No. Turno: " + game.GetState.Get_Turns);
        }

        public static void PrintDB (List<Card> database) {
            Console.WriteLine(" ~ Mostrando todas las cartas ~");
            foreach(Card item in database) {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Type: " + item.GetTypeCard);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(item);
            }
        }
        public static void PrintDeck (List<Deck> decks) {
            int i = 1;
            foreach(Deck deck in decks) {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("DECK " + i++);
                Console.ForegroundColor = ConsoleColor.White;
                PrintDeck(deck);
                Console.WriteLine("TOTAL: " + deck.Length + " cartas");
            }
        }
        public static void PrintDeck (Deck deck) {
            for (int i = 0; i < deck.Length; i++) {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Type: " + deck[i].GetTypeCard);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(deck[i]);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCard_v2.Library
{
    class ReadFiles {

        public static string Seach_dirProject(string nameProject) {
            string dir = Environment.CurrentDirectory;
            while (true) {
                int temp = dir.LastIndexOf(@"\");
                string line = dir.Substring(temp + 1);
                if (line == nameProject) return dir;
                else dir = dir.Remove(temp);
            } throw new Exception("nameProject incorrect");
        } 
        public static string Seach_dirProjectFolder(string dirProject, string folder) {
            List<string> files = Directory.GetDirectories(dirProject).ToList();
            foreach (string file in files) {
                int temp = file.LastIndexOf(@"\");
                string line = file.Substring(temp + 1);
                if (line == folder) return file;
            } throw new Exception("not exist folder");
        } 
        public static List<Card> LoadDataBaseCard(string dir) {
            List<Card> database = new List<Card>();
            List<string> files = Directory.GetFiles(dir).ToList();
            foreach (string file in files) {
                database.Add(TransformTextToCard(file));
            } return database;
        } 
        public static List<Deck> LoadDeck(string dir) {
            List<Deck> database = new List<Deck>();
            List<string> folders = Directory.GetDirectories(dir).ToList();
            foreach (string folder in folders) {
                database.Add(new Deck(LoadDataBaseCard(folder)));
            } return database;
        } 
        public static void TransformCardToText(Card card, string dir) {
            dir += @"\" + card.Name + ".txt";
            File.WriteAllText(dir, card.TransformCardToTXT());
        } 
        public static Card TransformTextToCard(string dir) {
            List<string> list = MethNecesary.Extract(ReadTXT(dir));
            if (list[0] == TypeCards.Soldier.ToString()) {
                return new Soldier(list[1].Replace('_', ' '), int.Parse(list[2]), int.Parse(list[3]), int.Parse(list[4]), new Effect(list[5], int.Parse(list[6]), list[7], int.Parse(list[8])));
            }
            if (list[0] == TypeCards.Struct.ToString()) {
                return new Struct(list[1].Replace('_', ' '), int.Parse(list[2]), int.Parse(list[3]), new Effect(list[4], int.Parse(list[5]), list[6], int.Parse(list[7])));
            } throw new Exception();
        }
        private static string ReadTXT (string dir) {
            StreamReader sr = new StreamReader(dir);
            string line = sr.ReadToEnd();
            sr.Close();
            return line;
        }

    }
}

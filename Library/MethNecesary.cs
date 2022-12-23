using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCard_v2.Library
{
    class MethNecesary {
        public static List<string> GetListCondition() {
            List<string> list = new List<string>();
            foreach (string item in Enum.GetNames(typeof(TypeCondition))) list.Add(item);
            return list;
        }
        public static List<string> GetListAttribute() {
            List<string> list = new List<string>();
            foreach (string item in Enum.GetNames(typeof(TypeAttribute))) list.Add(item);
            return list;
        }
        public static List<string> GetListCards() {
            List<string> list = new List<string>();
            foreach (string item in Enum.GetNames(typeof(TypeCards))) list.Add(item);
            return list;
        }
        public static List<string> GetListPlayer() {
            List<string> list = new List<string>();
            foreach (string item in Enum.GetNames(typeof(TypePlayer))) list.Add(item);
            return list;
        }
        public static List<string> Extract (string line) {
            List<string> list = new List<string>();
            string temp = "";
            for (int i = 0; i < line.Length; i++) {
                if (line[i] == ' ') {
                    list.Add(temp);
                    temp = "";
                } else temp += line[i];
            } return list;
        }
        public static int CountTypeCards() => GetListCards().Count;
        public static int CountTypeCondition() => GetListCondition().Count;
        public static int CountTypeAttribute() => GetListAttribute().Count;
    }
}

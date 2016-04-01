using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace IA.detectarIdioma
{
    public class LanguageDetector
    {
        public Dictionary<char, double> DictEnglish;
        public Dictionary<char, double> DictGerman;
        public Dictionary<char, double> DictSpanish;
        public Dictionary<char, double> DictTurkish;
        public Dictionary<char, double> DictText;

        public LanguageDetector()
        {

            DictEnglish = new Dictionary<char, double>()
                    {{'a', 8.167}, {'b', 1.492}, {'c', 2.782}, {'d', 4.253}, {'e', 12.702}, {'f', 2.228}, {'g', 2.015},
                     {'h', 6.094}, {'i', 6.966}, {'j', 0.153}, {'k', 0.772}, {'l', 4.025}, {'m', 2.406}, {'n', 6.749},
                     {'o', 7.507}, {'p', 1.929}, {'q', 0.095}, {'r', 5.987}, {'s', 6.327}, {'t', 9.056}, {'u', 2.758},
                     {'v', 0.978}, {'w', 2.360}, {'x', 0.150}, {'y', 1.974}, {'z', 0.074}, {'á', 0.000}, {'ä', 0.000},
                     {'é', 0.000}, {'í', 0.000}, {'ñ', 0.000}, {'ö', 0.000}, {'ó', 0.000}, {'ß', 0.000}, {'ú', 0.000},
                     {'ü', 0.000}};

            DictGerman = new Dictionary<char, double>()
                    {{'a', 6.516}, {'b', 1.886}, {'c', 2.732}, {'d', 5.076}, {'e', 17.396}, {'f', 1.656}, {'g', 3.009},
                     {'h', 4.757}, {'i', 7.550}, {'j', 0.268}, {'k', 1.417}, {'l', 3.437}, {'m', 2.534}, {'n', 9.776},
                     {'o', 2.594}, {'p', 0.670}, {'q', 0.018}, {'r', 7.003}, {'s', 7.273}, {'t', 6.154}, {'u', 4.346},
                     {'v', 0.846}, {'w', 1.921}, {'x', 0.034}, {'y', 0.039}, {'z', 1.134}, {'á', 0.000}, {'ä', 0.447},
                     {'é', 0.000}, {'í', 0.000}, {'ñ', 0.000}, {'ö', 0.573}, {'ó', 0.000}, {'ß', 0.307}, {'ú', 0.995},
                     {'ü', 0.995}};

            DictSpanish = new Dictionary<char, double>()
                    {{'a', 12.525}, {'b', 2.215}, {'c', 4.139}, {'d', 5.860}, {'e', 13.681}, {'f', 0.692}, {'g', 1.768},
                     {'h', 0.703}, {'i', 6.247}, {'j', 0.443}, {'k', 0.011}, {'l', 4.967}, {'m', 3.157}, {'n', 6.712},
                     {'o', 8.683}, {'p', 2.510}, {'q', 0.877}, {'r', 6.871}, {'s', 7.977}, {'t', 4.632}, {'u', 3.927},
                     {'v', 1.138}, {'w', 0.017}, {'x', 0.215}, {'y', 1.008}, {'z', 0.517}, {'á', 0.502}, {'ä', 0.000},
                     {'é', 0.433}, {'í', 0.725}, {'ñ', 0.311}, {'ö', 0.000}, {'ó', 0.827}, {'ß', 0.000}, {'ú', 0.168},
                     {'ü', 0.012}};

            DictTurkish = new Dictionary<char, double>()
                    {{'a',12.920 }, {'b',2.844 }, {'c',1.463 }, {'d',5.206 },  {'e',9.912 }, {'f', 0.461}, {'g',1.253 },
                     {'h',1.212 },  {'i',9.600 }, {'j',0.034},  {'k',5.683 },  {'l',5.922 }, {'m',3.752 }, {'n',7.987 },
                     {'o', 2.976},  {'p',0.886},  {'q',0 },     {'r',7.722},   {'s',3.014 }, {'t',3.314 }, {'u',3.235 },
                     {'v',0.959},   {'w', 0},     {'x',0 },     {'y', 3.336},  {'z', 1.500 }, {'á',0 },    {'ä', 0},
                     {'é',0 },      {'í',0 },     {'ñ',0 },     {'ö', 0.777 }, {'ó',0 },      {'ß',0 },    {'ú', 0},
                     {'ü',1.854 }};

        }

        public string classifier(string Text)
        {
            DictText = new Dictionary<char, double>();

            Text = Text.ToLower();
            string TextClean = "";
            string Letters = "abcdefghijklmnopqrstuvwxyzáäéíñöóßúü";

            foreach (char Letter in Text)
            {
                if (Letters.IndexOf(Letter) != -1)
                {
                    TextClean += Letter;
                }
            }

            int originalLength = TextClean.Length;
            while (TextClean.Length > 0)
            {
                int OldLenght = TextClean.Length;
                char ActualLetter = TextClean[0];
                TextClean = TextClean.Replace(ActualLetter.ToString(), string.Empty);
                DictText.Add(ActualLetter, 100 * ((OldLenght - (double)TextClean.Length) / (double)originalLength));
            }

            double deviationEnglish = 0, deviationGerman = 0, deviationSpanish = 0, deviationTurkish = 0;
            foreach (KeyValuePair<char, double> entry in DictText.OrderBy(Letter => Letter.Key))
            {
                deviationEnglish += Math.Pow((DictEnglish[entry.Key] - entry.Value), 2);
                deviationGerman += Math.Pow((DictGerman[entry.Key] - entry.Value), 2);
                deviationSpanish += Math.Pow((DictSpanish[entry.Key] - entry.Value), 2);
                deviationTurkish += Math.Pow((DictTurkish[entry.Key] - entry.Value), 2);

            }

            //Promedio
            deviationEnglish /= DictText.Count;
            deviationGerman /= DictText.Count;
            deviationSpanish /= DictText.Count;
            deviationTurkish /= DictText.Count;

            string Result = "Por favor ingrese mas palabras para mejorar los resultados";
            if (deviationEnglish < deviationGerman && deviationEnglish < deviationSpanish && deviationEnglish < deviationTurkish)
                Result = "Ingles";
            else if (deviationGerman < deviationEnglish && deviationGerman < deviationSpanish && deviationGerman < deviationTurkish)
                Result = "Aleman";
            else if (deviationSpanish < deviationEnglish && deviationSpanish < deviationGerman && deviationSpanish < deviationTurkish)
                Result = "Español";
            else if (deviationTurkish < deviationEnglish && deviationTurkish < deviationGerman && deviationTurkish < deviationSpanish)
                Result = "Turco";
            return Result;
        }

        public Dictionary<char, double> GetDictionary(string Idioma)
        {
            switch (Idioma)
            {
                case "Ingles":
                    return DictEnglish;
                case "Aleman":
                    return DictGerman;
                case "Español":
                    return DictSpanish;
                case "Turco":
                    return DictTurkish;
                default:
                    return null;
            }
        }

        public static DataTable DictionaryToDatatable(Dictionary<char, double> Dictionary)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Letter", typeof(char));
            table.Columns.Add("Frequency", typeof(double));
            foreach (KeyValuePair<char, double> entry in Dictionary.OrderBy(Letter => Letter.Key))
            {
                table.Rows.Add(entry.Key, entry.Value);
            }
            return table;
        }
    }
}
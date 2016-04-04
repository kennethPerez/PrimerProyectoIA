using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IA.stopWords
{
    public class stopWords
    {
        public List<string> stopWordSpanish = new List<string>();
        public List<string> stopWordEnglish = new List<string>();
        public List<string> stopWordGerman = new List<string>();
        public List<string> stopWordTurkish = new List<string>();
        public stopWords()
        {

            /*Espannol*/
            var stringEspannol = new List<String>
            {
                "un",
                "una",
                "unas",
                "unos",
                "uno",
                "sobre",
                "todo",
                "también",
                "tras",
                "otro",
                "algún",
                "alguno",
                "alguna",
                "algunos",
                "algunas",
                "ser",
                "es",
                "soy",
                "eres",
                "somos",
                "sois",
                "estoy",
                "esta",
                "estamos",
                "estais",
                "estan",
                "como",
                "en",
                "para",
                "atras",
                "porque",
                "por qué",
                "estado",
                "estaba",
                "ante",
                "antes",
                "siendo",
                "ambos",
                "pero",
                "por",
                "poder",
                "puede",
                "puedo",
                "podemos",
                "podeis",
                "pueden",
                "fui",
                "fue",
                "fuimos",
                "fueron",
                "hacer",
                "hago",
                "hace",
                "hacemos",
                "haceis",
                "hacen",
                "cada",
                "fin",
                "incluso",
                "primero desde",
                "conseguir",
                "consigo",
                "consigue",
                "consigues",
                "conseguimos",
                "consiguen",
                "ir",
                "voy",
                "va",
                "vamos",
                "vais",
                "van",
                "vaya",
                "gueno",
                "ha",
                "tener",
                "tengo",
                "tiene",
                "tenemos",
                "teneis",
                "tienen",
                "el",
                "la",
                "lo",
                "las",
                "los",
                "su",
                "aqui",
                "mio",
                "tuyo",
                "ellos",
                "ellas",
                "nos",
                "nosotros",
                "vosotros",
                "vosotras",
                "si",
                "dentro",
                "solo",
                "solamente",
                "saber",
                "sabes",
                "sabe",
                "sabemos",
                "sabeis",
                "saben",
                "ultimo",
                "largo",
                "bastante",
                "haces",
                "muchos",
                "aquellos",
                "aquellas",
                "sus",
                "entonces",
                "tiempo",
                "verdad",
                "verdadero",
                "verdadera",
                "cierto",
                "ciertos",
                "cierta",
                "ciertas",
                "intentar",
                "intento",
                "intenta",
                "intentas",
                "intentamos",
                "intentais",
                "intentan",
                "dos",
                "bajo",
                "arriba",
                "encima",
                "usar",
                "uso",
                "usas",
                "usa",
                "usamos",
                "usais",
                "usan",
                "emplear",
                "empleo",
                "empleas",
                "emplean",
                "ampleamos",
                "empleais",
                "valor",
                "muy",
                "era",
                "eras",
                "eramos",
                "eran",
                "modo",
                "bien",
                "cual",
                "cuando",
                "donde",
                "mientras",
                "quien",
                "con",
                "entre",
                "sin",
                "trabajo",
                "trabajar",
                "trabajas",
                "trabaja",
                "trabajamos",
                "trabajais",
                "trabajan",
                "podria",
                "podrias",
                "podriamos",
                "podrian",
                "podriais",
                "yo",
                "aquel",
                "de",
                "que",
                "del",
                "a",
                "o",
                "y",
                "más"

            };
            stringEspannol.ForEach(s => stopWordSpanish.Add(s));

            /*Ingles*/
            var stringIngles = new List<String>
            {
                "Tecnologia"
                ,"mainot"

            };
            stringIngles.ForEach(s => stopWordEnglish.Add(s));

            /*Aleman*/
            var stringAleman = new List<String>
            {
                "Tecnologia"
                ,"mainot"

            };
            stringAleman.ForEach(s => stopWordGerman.Add(s));

            /*Turco*/
            var stringTurco = new List<String>
            {
                "Tecnologia"
                ,"mainot"

            };
            stringTurco.ForEach(s => stopWordTurkish.Add(s));

        }
    }
}
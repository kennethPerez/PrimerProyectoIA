using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IA.bayes_algoritmo
{

    public class NaiveBayes
    {
        List<bayesCategoria> tablaResult;
        List<bayesCategoria> stockData;
        string texto;

        private int cantidadApariciones(string palabra, string[] lista)
        {
            int i = 0;
            foreach (string palabraEnLista in lista)
            {
                if (palabraEnLista.Equals(palabra))
                {
                    i++;
                }
            }
            return i;
        }
        public NaiveBayes(List<bayesCategoria> data, List<bayesCategoria> categorias, string texto)
        {
            this.stockData = data;
            this.tablaResult = categorias;
            this.texto = texto;
        }

        public Respuesta classifier()
        {

            foreach (bayesCategoria categoria in stockData)
            {

                foreach (bayesCategoria categoriaFinales in tablaResult)
                {
                    if (categoria.categoria.Equals(categoriaFinales.categoria))
                    {
                        categoriaFinales.listaDelistasDepalabras.Add(categoria.palabra);
                    }
                }
            }


            foreach (bayesCategoria categoriaFinales in tablaResult)
            {
                for (int i = 0; i < categoriaFinales.listaDelistasDepalabras.ElementAt(0).Count(); i++)
                {
                    List<double> numerosPorPalabra = new List<double>();
                    foreach (List<bayesPalabra> listaPalabras in categoriaFinales.listaDelistasDepalabras)
                    {
                        numerosPorPalabra.Add(listaPalabras.ElementAt(i).frecuencia);
                    }
                    categoriaFinales.palabra.Add(new bayesPalabra(categoriaFinales.listaDelistasDepalabras.ElementAt(0).ElementAt(i).palabra, calculateMedia(numerosPorPalabra), calculateVarianza(numerosPorPalabra)));


                }
            }


            return probabilidades(tablaResult);
        }

        public Respuesta probabilidades(List<bayesCategoria> tableResult)
        {
            List<bayesPalabra> palabrasMuestra = new List<bayesPalabra>();

            foreach (bayesPalabra palabra in tableResult.ElementAt(0).palabra)
            {
                palabrasMuestra.Add(new bayesPalabra(palabra.palabra, System.Text.RegularExpressions.Regex.Matches(texto, palabra.palabra).Count));
            }

            /*
            palabrasMuestra.Add(new bayesPalabra("altura", 6));
            palabrasMuestra.Add(new bayesPalabra("peso", 130));
            palabrasMuestra.Add(new bayesPalabra("pie", 8));*/

            bayesCategoria tablaMuestra = new bayesCategoria("muestra", palabrasMuestra);


            /*----------------Lista de resultados de la parte de arruba del posteriori---------------------*/
            List<resultPosterioriategoriaArriba> listaResultadosArriba = new List<resultPosterioriategoriaArriba>();

            foreach (bayesCategoria categoria in tableResult)
            {
                double result = (1.0 / tableResult.Count);
                for (int i = 0; i < tablaMuestra.palabra.Count; i++)
                {
                    result = result * probabilidadPalabraEnCategoria(categoria.palabra.ElementAt(i), tablaMuestra.palabra.ElementAt(i));
                }
                listaResultadosArriba.Add(new resultPosterioriategoriaArriba(categoria.categoria, result));
            }

            /*------------------------Sacando la evidencia---------------------------------------------*/
            double evidencia = 0;
            foreach (resultPosterioriategoriaArriba resultPosteriori in listaResultadosArriba)
            {
                evidencia += resultPosteriori.result;
            }

            /*------------------------Probabilidad final de cada categoria------------------------------------------*/
            List<ResultNaiveBayes> listaProbabilidadesFinales = new List<ResultNaiveBayes>();
            foreach (resultPosterioriategoriaArriba resultPosteriori in listaResultadosArriba)
            {
                listaProbabilidadesFinales.Add(new ResultNaiveBayes(resultPosteriori.categoria, resultPosteriori.result / evidencia));
            }

            string categ = "";
            double prob = 0;
            foreach (ResultNaiveBayes resultado in listaProbabilidadesFinales)
            {
                if (resultado.probabilidad > prob)
                {
                    categ = resultado.categoria;
                    prob = resultado.probabilidad;
                }
            }



            List<incidencia> incidencias = new List<incidencia>();
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            string[] words = texto.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);

            foreach (bayesPalabra palabra in tableResult.ElementAt(0).palabra)
            {
                //palabrasMuestra.Add(new bayesPalabra(palabra.palabra, System.Text.RegularExpressions.Regex.Matches(texto, palabra.palabra).Count));
                palabrasMuestra.Add(new bayesPalabra(palabra.palabra, cantidadApariciones(palabra.palabra, words)));
            }
            

            List<incidencia> SortedList = incidencias.OrderBy(o => o.repeticiones).ToList();
            SortedList.Reverse();


            Respuesta respuestaFinal = new Respuesta(SortedList, listaProbabilidadesFinales, tablaMuestra, categ);
            return respuestaFinal;
        }

        // Calcula la probabilidad de una palabra dada la categoria
        public double probabilidadPalabraEnCategoria(bayesPalabra palabraBase, bayesPalabra palabraMuestra)
        {
            double result = (1 / (Math.Sqrt(2 * Math.PI * palabraBase.calculateVarianza))) * Math.Pow(Math.E, -(Math.Pow((palabraMuestra.frecuencia - palabraBase.calculateMedia), 2) / (2 * palabraBase.calculateVarianza)));
            return result;
        }

        // Calcula la media de una lista de numeros
        public double calculateMedia(List<double> numeros)
        {
            double N = 0, prom = 0, suma = 0;
            N = numeros.Count;
            foreach (double dato in numeros)
            {
                suma += dato;
            }
            prom = suma / N;

            if(prom == 0)
                return 0.1;
            return prom;
        }

        // Calcula la varianza de una lista de numeros
        public double calculateVarianza(List<double> numeros)
        {
            try {
                double N = 0, prom = 0, NrestadoUno = 0, sumapotencias = 0, desvStd = 0;
                N = numeros.Count;
                NrestadoUno = N - 1;
                prom = calculateMedia(numeros);

                foreach (double dato in numeros)
                {
                    sumapotencias += Math.Pow((dato - prom), 2);
                }
                desvStd = Math.Sqrt((1 * sumapotencias) / NrestadoUno);
                double x = Math.Pow(desvStd, 2);
                if (x == 0)
                    return 0.1;
                else
                    return x;
            }
            catch
            {
                return 0.1;
            }
        }

    }

    public class bayesCategoria
    {
        public string categoria;
        public List<bayesPalabra> palabra;
        public List<List<bayesPalabra>> listaDelistasDepalabras = new List<List<bayesPalabra>>();//para mejorar rendimiento perder memoria
        public int muestraId;
        public bayesCategoria(string categoria, List<bayesPalabra> palabra)
        {
            this.categoria = categoria;
            this.palabra = palabra;
        }
        public bayesCategoria(string categoria, List<bayesPalabra> palabra,int muestraId)
        {
            this.categoria = categoria;
            this.palabra = palabra;
            this.muestraId = muestraId;
        }

    }

    public class resultPosterioriategoriaArriba
    {
        public string categoria;
        public double result;
        public resultPosterioriategoriaArriba(string categoria, double result)
        {
            this.categoria = categoria;
            this.result = result;
        }

    }

    public class ResultNaiveBayes
    {
        public string categoria;
        public double probabilidad;
        public ResultNaiveBayes(string categoria, double probabilidad)
        {
            this.categoria = categoria;
            this.probabilidad = probabilidad;
        }

    }

    public class bayesPalabra
    {
        public string palabra;
        public double frecuencia;
        public double calculateMedia;
        public double calculateVarianza;
        public bayesPalabra(string palabra, double frecuencia)
        {
            this.frecuencia = frecuencia;
            this.palabra = palabra;
        }
        public bayesPalabra(string palabra, double calculateMedia, double calculateVarianza)
        {
            this.palabra = palabra;
            this.calculateMedia = calculateMedia;
            this.calculateVarianza = calculateVarianza;
        }

    }

    public class incidencia
    {
        public string palabra;
        public int repeticiones;

        public incidencia(string palabra, int repeticiones)
        {
            this.palabra = palabra;
            this.repeticiones = repeticiones;
        }
    }

    public class Respuesta
    {
        public List<incidencia> incidencias;
        public List<ResultNaiveBayes> listaDeProbabilidadesFinales;
        public bayesCategoria tablaMuestra;
        public string categoria;
        public Respuesta(List<incidencia> incidencia, List<ResultNaiveBayes> listaDeProbabilidadesFinales, bayesCategoria tablaMuestra, string categoria)
        {
            this.incidencias = incidencia;
            this.listaDeProbabilidadesFinales = listaDeProbabilidadesFinales;
            this.tablaMuestra = tablaMuestra;
            this.categoria = categoria;
        }

    }
}
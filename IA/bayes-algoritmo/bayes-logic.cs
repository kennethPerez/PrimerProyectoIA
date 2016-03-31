using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IA.bayes_algoritmo
{
    public class bayes_logic
    {
        /*------------------------------------Para el mock----------------------------------------------------------*/
        List<bayesCategoria> mockData = new List<bayesCategoria>();
        public List<bayesPalabra> listaPalabrasTecno = new List<bayesPalabra>();
        public List<bayesPalabra> listaPalabrasreli = new List<bayesPalabra>();
        public List<bayesPalabra> listaPalabrassuce = new List<bayesPalabra>();
        public List<bayesPalabra> listaPalabrastecno = new List<bayesPalabra>();


        /*-----------------------tabla resultante con media y demas-----------------------------------------------*/
        List<bayesCategoria> tablaResult = new List<bayesCategoria>();

        public bayes_logic()
        {
            listaPalabrasTecno.Add(new bayesPalabra("tecnologia", 5));
            listaPalabrasTecno.Add(new bayesPalabra("computacion", 7));
            listaPalabrasTecno.Add(new bayesPalabra("biblia", 0));
            listaPalabrasTecno.Add(new bayesPalabra("religion", 1));
            listaPalabrasTecno.Add(new bayesPalabra("asesino", 1));
            listaPalabrasTecno.Add(new bayesPalabra("arma", 0));
            listaPalabrasTecno.Add(new bayesPalabra("celular", 10));
            listaPalabrasTecno.Add(new bayesPalabra("muerte", 3));
            mockData.Add(new bayesCategoria("Tecnologia", listaPalabrasTecno));


            listaPalabrasreli.Add(new bayesPalabra("tecnologia", 2));
            listaPalabrasreli.Add(new bayesPalabra("computacion", 1));
            listaPalabrasreli.Add(new bayesPalabra("biblia", 10));
            listaPalabrasreli.Add(new bayesPalabra("religion", 10));
            listaPalabrasreli.Add(new bayesPalabra("asesino", 2));
            listaPalabrasreli.Add(new bayesPalabra("arma", 1));
            listaPalabrasreli.Add(new bayesPalabra("celular", 3));
            listaPalabrasreli.Add(new bayesPalabra("muerte", 5));
            mockData.Add(new bayesCategoria("religion", listaPalabrasreli));

            listaPalabrassuce.Add(new bayesPalabra("tecnologia", 2));
            listaPalabrassuce.Add(new bayesPalabra("computacion", 2));
            listaPalabrassuce.Add(new bayesPalabra("biblia", 2));
            listaPalabrassuce.Add(new bayesPalabra("religion", 2));
            listaPalabrassuce.Add(new bayesPalabra("asesino", 10));
            listaPalabrassuce.Add(new bayesPalabra("arma", 8));
            listaPalabrassuce.Add(new bayesPalabra("celular", 5));
            listaPalabrassuce.Add(new bayesPalabra("muerte", 10));
            mockData.Add(new bayesCategoria("sucesos", listaPalabrassuce));

            listaPalabrastecno.Add(new bayesPalabra("tecnologia", 7));
            listaPalabrastecno.Add(new bayesPalabra("computacion", 8));
            listaPalabrastecno.Add(new bayesPalabra("biblia", 7));
            listaPalabrastecno.Add(new bayesPalabra("religion", 0));
            listaPalabrastecno.Add(new bayesPalabra("asesino", 0));
            listaPalabrastecno.Add(new bayesPalabra("arma", 7));
            listaPalabrastecno.Add(new bayesPalabra("celular", 15));
            listaPalabrastecno.Add(new bayesPalabra("muerte", 3));
            mockData.Add(new bayesCategoria("Tecnologia", listaPalabrastecno));


            /*---------------------------Tabla result--------------------------------------------*/
            tablaResult.Add(new bayesCategoria("Tecnologia", new List<bayesPalabra>()));
            tablaResult.Add(new bayesCategoria("religion", new List<bayesPalabra>()));
            tablaResult.Add(new bayesCategoria("sucesos", new List<bayesPalabra>()));
            /*---------------------------llamada metodo--------------------------------------------*/
            bayesSolucionTableResult();
        }


        public void bayesSolucionTableResult()
        {

            List<List<bayesPalabra>> listaT = new List<List<bayesPalabra>>();
            List<List<bayesPalabra>> listaR = new List<List<bayesPalabra>>();
            List<List<bayesPalabra>> listaS = new List<List<bayesPalabra>>();

            foreach (bayesCategoria categoria in mockData)
            {
                if (categoria.categoria.Equals("Tecnologia"))
                {
                    listaT.Add(categoria.palabra);
                }
                if (categoria.categoria.Equals("religion"))
                {
                    listaR.Add(categoria.palabra);
                }
                if (categoria.categoria.Equals("sucesos"))
                {
                    listaS.Add(categoria.palabra);
                }
            }


            List<bayesPalabra> listaPalabrasTec = new List<bayesPalabra>();

            for (int i = 0; i < listaT.ElementAt(0).Count(); i++)
            {
                List<int> numerosPorPalabra = new List<int>();
                foreach (List<bayesPalabra> listaPalabras in listaT)
                {
                    numerosPorPalabra.Add(listaPalabras.ElementAt(i).frecuencia);
                }
                tablaResult.ElementAt(0).palabra.Add(new bayesPalabra(listaT.ElementAt(0).ElementAt(i).palabra, media(numerosPorPalabra), variansa(numerosPorPalabra)));
            }

            for (int i = 0; i < listaR.ElementAt(0).Count(); i++)
            {
                List<int> numerosPorPalabra = new List<int>();
                foreach (List<bayesPalabra> listaPalabras in listaR)
                {
                    numerosPorPalabra.Add(listaPalabras.ElementAt(i).frecuencia);
                }
                tablaResult.ElementAt(1).palabra.Add(new bayesPalabra(listaR.ElementAt(0).ElementAt(i).palabra, media(numerosPorPalabra), variansa(numerosPorPalabra)));
            }

            for (int i = 0; i < listaS.ElementAt(0).Count(); i++)
            {
                List<int> numerosPorPalabra = new List<int>();
                foreach (List<bayesPalabra> listaPalabras in listaS)
                {
                    numerosPorPalabra.Add(listaPalabras.ElementAt(i).frecuencia);
                }
                tablaResult.ElementAt(2).palabra.Add(new bayesPalabra(listaS.ElementAt(0).ElementAt(i).palabra, media(numerosPorPalabra), variansa(numerosPorPalabra)));
            }

            probabilidades(tablaResult);
        }

        public void probabilidades(List<bayesCategoria> tableResult) {
            List<bayesPalabra> palabrasMuestra = new List<bayesPalabra>();

            string texto = "mainor coje colas de computacion cuando el celular asesino la religion de la biblia muerte";
            
            

            palabrasMuestra.Add(new bayesPalabra("tecnologia", System.Text.RegularExpressions.Regex.Matches(texto, "tecnologia").Count  ));
            palabrasMuestra.Add(new bayesPalabra("computacion", System.Text.RegularExpressions.Regex.Matches(texto, "computacion").Count));
            palabrasMuestra.Add(new bayesPalabra("biblia", System.Text.RegularExpressions.Regex.Matches(texto, "biblia").Count ));
            palabrasMuestra.Add(new bayesPalabra("religion", System.Text.RegularExpressions.Regex.Matches(texto, "religion").Count ));
            palabrasMuestra.Add(new bayesPalabra("asesino", System.Text.RegularExpressions.Regex.Matches(texto, "asesino").Count ));
            palabrasMuestra.Add(new bayesPalabra("arma", System.Text.RegularExpressions.Regex.Matches(texto, "arma").Count ));
            palabrasMuestra.Add(new bayesPalabra("celular", System.Text.RegularExpressions.Regex.Matches(texto, "celular").Count ));
            palabrasMuestra.Add(new bayesPalabra("muerte", System.Text.RegularExpressions.Regex.Matches(texto, "muerte").Count ));

            bayesCategoria tablaMuestra = new bayesCategoria("muestra", palabrasMuestra);
    }


        public double media(List<int> numeros)
        {
            
            double N = 0, prom = 0, suma = 0;
            N = numeros.Count;
            foreach (double dato in numeros)
            {
                suma += dato;
            }
            prom = suma / N;
            return prom;
        }
        public double variansa(List<int> numeros)
        {
            double desvStd = 0;
            double N = 0, prom = 0, suma = 0, NrestadoUno = 0, sumapotencias = 0;
            N = numeros.Count;
            NrestadoUno = N - 1;
            foreach (double dato in numeros)
            {
                suma += dato;
            }
            prom = suma / N;
            foreach (double dato in numeros)
            {
                sumapotencias += Math.Pow((dato - prom), 2);
            }
            desvStd = Math.Sqrt((1 * sumapotencias) / NrestadoUno);
            return Math.Pow(desvStd,2);
        }

        public class bayesCategoria
        {
            public string categoria;
            public List<bayesPalabra> palabra;
            public bayesCategoria(string categoria, List<bayesPalabra> palabra)
            {
                this.categoria = categoria;
                this.palabra = palabra;
            }

        }
        public class bayesPalabra
        {
            public string palabra;
            public int frecuencia;
            public double media;
            public double variansa;
            public bayesPalabra(string palabra, int frecuencia)
            {
                this.frecuencia = frecuencia;
                this.palabra = palabra;
            }
            public bayesPalabra(string palabra, double media, double variansa)
            {
                this.palabra = palabra;
                this.media = media;
                this.variansa = variansa;
            }

        }



    }
}
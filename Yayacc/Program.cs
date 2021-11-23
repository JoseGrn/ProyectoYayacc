using System;
using System.Collections.Generic;

namespace Yayacc
{
    class Program
    {
        static void Main(string[] args)
        {
            bool decision1 = true;
            Parser parser = new Parser();
            Console.WriteLine("Ingrese la ruta de su archivo .y");
            string ruta = Console.ReadLine();
            string text = System.IO.File.ReadAllText(ruta);
            Listas Result = parser.initializeGrammar(text);
            GraphCreator grafo = new GraphCreator();
            List<Nodo> Grafo = new List<Nodo>();

            if (Result != null)
            {
                grafo.Evaluacion(Result);
                while (decision1)
                {
                    Console.WriteLine("Ingrese una prueba para su gramatica: ");
                    string prueba = Console.ReadLine();
                    //parser.tal(prueba)
                    Console.WriteLine("desea seguir haciendo pruebas?");
                    string decision2 = Console.ReadLine();
                    if (decision2 != "si")
                        decision1 = false;
                }
            }
            else
            {
                Console.WriteLine("Gramatica rechazada");
            }            
            Console.ReadLine();
        }
    }
}
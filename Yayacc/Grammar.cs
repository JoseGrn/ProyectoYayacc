using System;
using System.Collections.Generic;
using Yayacc;
namespace Yayacc
{
    class Grammar
    {
        //E '->' E (split)
        //Estados -> Variable (reglas)
        //checamos si la variable existe, si existe solo almacenamos la regla
        //si no existe, almacenamos ambos
        List<string> terminales = new List<string>();
        List<string> variables = new List<string>();
        List<string> Reglas = new List<string>();
        Listas List = new Listas();

        string inicial;
        public Grammar(List<string> t, List<string> v, string ini, List<string> r)
        {
            terminales = t;
            variables = v;
            inicial = ini;
            Reglas = r;
        }
        public Listas Guardar()
        {
            List.T = terminales;
            List.V = variables;
            List.R = Reglas;
            return List;
        }
        public void imprimir()
        {
            Console.WriteLine("Variables:");
            foreach (var item in variables)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("Terminales:");
            foreach (var item in terminales)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("No terminal inicial:");
            Console.WriteLine(inicial);

            Console.WriteLine("Desea imprimir las reglas? (si/no)");
            string Decision = Console.ReadLine();
            if (Decision == "si" || Decision == "SI" || Decision == "Si" || Decision == "sI")
            {
                Console.WriteLine("Reglas:");
                foreach (var item in Reglas)
                {
                    Console.WriteLine(item);
                }
            }
            Console.WriteLine("Enter para continuar");
            Console.ReadLine();
            Console.Clear();
        }
    }
}

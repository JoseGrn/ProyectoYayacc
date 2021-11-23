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
    }
}

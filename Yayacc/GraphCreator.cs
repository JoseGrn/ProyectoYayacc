using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yayacc
{
    public class GraphCreator
    {
        List<Nodo> Grafo = new List<Nodo>();
        List<Nodo> GrafoAux = new List<Nodo>();
        List<Estates> Estados = new List<Estates>();
        Listas listas = new Listas();
        List<string> Variables = new List<string>();
        List<string> Faltantes = new List<string>();
        List<string> Leidos = new List<string>();
        List<string> NuevoNodo = new List<string>();
        List<string> ListaNodo = new List<string>();
        List<string> Reglas = new List<string>();
        int NodoActual = 0, contador = 0;
        bool final = true, existe = true;

        public List<Nodo> Evaluacion (Listas result)
        {
            Faltantes.Add("S`");
            listas = result;
            Variables = listas.V;
            Reglas = listas.R;
            LlenadoLista();
            CreacionGrafo();
            return Grafo;
        }

        public void LlenadoLista()
        {
            foreach (var item in listas.R)
            {
                string[] aux = item.Split('→');

                if (!Estados.Exists(x => x.Variable == aux[0]))
                {
                    Estates AuxEstate = new Estates();
                    AuxEstate.Variable = aux[0];
                    AuxEstate.Estados.Add(aux[1]);
                    Estados.Add(AuxEstate);
                }
                else
                {
                    foreach (var item2 in Estados)
                    {
                        if (item2.Variable == aux[0])
                        {
                            item2.Estados.Add(aux[1]);
                        }
                    }
                }
            }
        }

        public void CreacionGrafo()
        {
            Nodo Aux = new Nodo();

            if (Grafo.Count == 0 && final)
            {
                Aux = LlenadoNodos(Aux);
                Aux.Estado = "S`";
                Aux.NoEstado = NodoActual;
                Grafo.Add(Aux);
                NodoActual++;
                CreacionGrafo();
            }
            
            foreach (var item in Grafo)
            {
                if (item.NoEstado == contador)
                {
                    ListaNodo = item.RulesList;
                    break;
                }
            }
            if (Grafo.ToArray().Length >= contador)
            {
                Aux = LlenadoNodos(Aux);
                foreach (var item in GrafoAux)
                {
                    Grafo.Add(item);
                }
                GrafoAux = new List<Nodo>();
                contador++;
                CreacionGrafo();
            }
        }

        public Nodo LlenadoNodos(Nodo nodo)
        {
            Nodo Aux = nodo;
            if (NodoActual == 0)
            {
                foreach (var item in Estados)
                {
                    
                    if (Faltantes.Exists(x => x == item.Variable) && !Leidos.Exists(x => x == item.Variable))
                    {
                        
                        Leidos.Add(item.Variable);
                        foreach (var item1 in item.Estados)
                        {
                            string itemaux = "";
                            Aux.RulesList.Add(item.Variable + "→•" + item1); //Estado 'final' Estado
                            
                            for (int i = 0; i < item1.Length; i++)
                            {
                                if (item1.Substring(i, 1) != "'" && item1.Substring(i, 1) != " ")
                                    itemaux += item1.Substring(i, 1);
                                else
                                    break;
                            }
                            
                            if (!Faltantes.Exists(x => x == itemaux) && itemaux != "'")
                            {
                                Faltantes.Add(itemaux);
                            }
                        }
                        Aux = LlenadoNodos(Aux);
                        break;
                    }
                }
            }
            else
            {
                foreach (var itemx in ListaNodo)
                {
                    string item = itemx.Trim();
                    int punto = item.IndexOf('•');
                    string aux = "";
                    
                    if ((punto + 1) != item.Length && item.Substring(punto+1,1) == "'")
                    {
                        Nodo NodoAux = new Nodo();
                        string ItemAux = "";
                        int cont1 = 0, cont2 = punto+1;
                        while (cont1 != 2)
                        {
                            if (item.Substring(cont2, 1) == "'")
                            {
                                cont1++;
                            }
                            aux += item.Substring(cont2, 1);
                            cont2++;
                        }
                        for (int i = 0; i < item.Length; i++) //S`-•E    S`-E•
                        {
                            if (item.Substring(i, 1) != "•")
                            {
                                ItemAux += item.Substring(i, 1);
                            }
                            else
                            {
                                ItemAux += aux + "•";
                                i += aux.Length;
                            }
                        }
                        List<string> ListaNuevosValores = RetornoCerradura(ItemAux);
                        foreach (var item2 in GrafoAux)
                        {
                            if (item2.Estado == aux)
                            {
                                item2.RulesList.Add(ItemAux);
                                item2.RulesList.AddRange(ListaNuevosValores);
                                existe = false;
                            }
                        }
                        if (existe)
                        {
                            NodoAux.Estado = aux;
                            NodoAux.NoEstado = NodoActual;
                            NodoAux.RulesList.Add(ItemAux);
                            NodoAux.RulesList.AddRange(ListaNuevosValores);
                            NodoActual++;
                            bool chequeo = true;
                            foreach (var item4 in Grafo)
                            {
                                if (item4.RulesList == NodoAux.RulesList)
                                {
                                    chequeo = false;
                                }
                            }
                            if (chequeo)
                            {
                                GrafoAux.Add(NodoAux);
                            }
                            chequeo = true;
                        }
                        existe = true;
                    }//validacion de si es un terminal
                    else if ((punto + 1) != item.Length && item.Substring(punto + 1, 1) == " ")
                    {
                        punto++;
                        if ((punto + 1) != item.Length && item.Substring(punto + 1, 1) == "'")
                        {
                            Nodo NodoAux = new Nodo();
                            string ItemAux = "";
                            int cont1 = 0, cont2 = punto + 1;
                            while (cont1 != 2)
                            {
                                if (item.Substring(cont2, 1) == "'")
                                {
                                    cont1++;
                                }
                                aux += item.Substring(cont2, 1);
                                cont2++;
                            }
                            for (int i = 0; i < item.Length; i++) //S`-•E    S`-E•
                            {
                                if (item.Substring(i, 1) != "•")
                                {
                                    ItemAux += item.Substring(i, 1);
                                }
                                else
                                {
                                    ItemAux += aux + "•";
                                    i += aux.Length+1;
                                }
                            }
                            List<string> ListaNuevosValores = RetornoCerradura(ItemAux);
                            foreach (var item2 in GrafoAux)
                            {
                                if (item2.Estado == aux)
                                {
                                    item2.RulesList.Add(ItemAux);
                                    item2.RulesList.AddRange(ListaNuevosValores);
                                    existe = false;
                                }
                            }
                            
                            if (existe)
                            {
                                NodoAux.Estado = aux;
                                NodoAux.NoEstado = NodoActual;
                                NodoAux.RulesList.Add(ItemAux);
                                NodoAux.RulesList.AddRange(ListaNuevosValores);
                                NodoActual++;
                                bool chequeo = true;
                                foreach (var item4 in Grafo)
                                {
                                    if (item4.RulesList == NodoAux.RulesList)
                                    {
                                        chequeo = false;
                                    }
                                }
                                if (chequeo)
                                {
                                    GrafoAux.Add(NodoAux);
                                }
                                chequeo = true;
                            }
                            existe = true;
                        }//validacion de si es un terminal despus de un espacio
                        else
                        {
                            Nodo NodoAux = new Nodo();
                            for (int i = punto + 1; i < item.Length; i++)
                            {
                                if (item.Substring(i, 1) != " " && item.Substring(i, 1) != "'")
                                    aux += item.Substring(i, 1);
                                else
                                    break;
                            }
                            string ItemAux = "";
                            for (int i = 0; i < item.Length; i++) //S`-•E    S`-E•
                            {
                                if (item.Substring(i, 1) != "•")
                                {
                                    ItemAux += item.Substring(i, 1);
                                }
                                else
                                {
                                    ItemAux += aux + "•";
                                    i += aux.Length+1;
                                }
                            }
                            List<string> ListaNuevosValores = RetornoCerradura(ItemAux);
                            foreach (var item2 in GrafoAux)
                            {
                                if (item2.Estado == aux)
                                {
                                    item2.RulesList.Add(ItemAux);
                                    NodoAux.RulesList.AddRange(ListaNuevosValores);
                                    existe = false;
                                }
                            }
                            if (existe)
                            {
                                NodoAux.Estado = aux;
                                NodoAux.NoEstado = NodoActual;
                                NodoAux.RulesList.Add(ItemAux);
                                NodoAux.RulesList.AddRange(ListaNuevosValores);
                                NodoActual++;
                                bool chequeo = true;
                                foreach (var item4 in Grafo)
                                {
                                    if (item4.RulesList == NodoAux.RulesList)
                                    {
                                        chequeo = false;
                                    }
                                }
                                if (chequeo)
                                {
                                    GrafoAux.Add(NodoAux);
                                }
                                chequeo = true;
                            }
                            existe = true;
                            //NodoAux.RulesList.Add();
                        } //validacion de si es variable despues de un espacio
                    }//validcion de si es un espacio
                    else if ((punto + 1) == item.Length)
                    {

                    }
                    else 
                    {
                        Nodo NodoAux = new Nodo();
                        for (int i = punto+1; i < item.Length; i++)
                        {
                            if (item.Substring(i, 1) != " " && item.Substring(i, 1) != "'")
                                aux += item.Substring(i, 1);
                            else
                                break;
                        }
                        string ItemAux = "";
                        for (int i = 0; i < item.Length; i++) //S`-•E    S`-E•
                        {
                            if (item.Substring(i, 1) != "•")
                            {
                                ItemAux += item.Substring(i, 1);
                            }
                            else
                            {
                                ItemAux += aux + "•";
                                i += aux.Length;
                            }
                        }
                        List<string> ListaNuevosValores = RetornoCerradura(ItemAux);
                        foreach (var item2 in GrafoAux)
                        {
                            if (item2.Estado == aux)
                            {
                                item2.RulesList.AddRange(ListaNuevosValores);
                                item2.RulesList.Add(ItemAux);
                                existe = false;
                            }
                        }
                        if (existe)
                        {

                            NodoAux.Estado = aux;
                            NodoAux.NoEstado = NodoActual;
                            NodoAux.RulesList.Add(ItemAux);
                            NodoActual++;
                            bool chequeo = false;
                            foreach (var item4 in Grafo)
                            {
                                if (item4.RulesList == NodoAux.RulesList)
                                {
                                    chequeo = false;
                                }
                            }
                            if (chequeo)
                            {
                                GrafoAux.Add(NodoAux);
                            }
                            chequeo = true;
                        }
                        existe = true;
                        //NodoAux.RulesList.Add();
                    }//validacion de si es una variable
                }
            }
            return Aux;
        }

        public List<string> RetornoCerradura(string Regla)
        {
            List<string> Cerradura = new List<string>();
            string aux = "";
            string Regla2 = Regla.Replace(" ", String.Empty);
            int punto = Regla2.IndexOf('•');
            if (Regla2.Length > punto+1)//E -> Eo '+' T
            {
                punto++;
                if (Regla2.Substring(punto, 1) != "'")
                {
                    for (int i = punto; i < Regla2.Length; i++)
                    {
                        if (Regla2.Substring(i, 1) == "'")
                        {
                            break;
                        }
                        aux += Regla2.Substring(i, 1);
                    }
                    foreach (var item in Reglas)
                    {
                        string[] aux2 = item.Split('→');
                        if (aux2[0] == aux)
                        {
                            string aux3 = aux2[0] + "→•" + aux2[1];
                            Cerradura.Add(aux3);
                        }
                    }
                }
            }
            return Cerradura;
        }
    }

    

    
}

using System;
using System.Collections.Generic;
using Yayacc;

namespace Yayacc
{
    class Parser
    {
        Scanner _scanner;
        Token _token;
        string noTerminalInicial = "", VariableActual = "", reglasUnidas = "";
        List<string> terminales = new List<string>();
        List<string> variables = new List<string>();
        List<string> reglas = new List<string>();
        Listas list = new Listas();
        Dictionary<int, Dictionary<string, int>> Movimientos = new Dictionary<int, Dictionary<string, int>>();
        Stack<int> Pila_Estados = new Stack<int>();
        Stack<string> Pila_Caracteres = new Stack<string>();
        string Pila_Char_Aux = "";

        //generacion tabla
        public bool parserfuncion(string texto, int estado)
        {
            if (Movimientos.ContainsKey(estado))
            {
                if (terminales.Exists(x => x == texto))
                {
                    Pila_Estados.Push(estado);
                    Pila_Caracteres.Push(texto);
                    Pila_Char_Aux = texto + Pila_Char_Aux;
                }
                else if (variables.Exists(x => x == texto))
                {
                    foreach (var item in reglas)
                    {
                        string Variable = item.Replace(" ", String.Empty).Replace("'", String.Empty);
                        if (Variable == Pila_Char_Aux.Substring(0, Variable.Length))
                        {
                            int total_regla = item.Replace(" ", String.Empty).Replace("'", String.Empty).Substring(3).Length;
                            for (int i = 0; i < total_regla; i++)
                            {
                                Pila_Caracteres.Pop();
                                Pila_Estados.Pop();
                            }
                            Pila_Caracteres.Push(item.Replace(" ", String.Empty).Substring(1, 1));
                            Pila_Estados.Push(Movimientos[estado][item.Replace(" ", String.Empty).Substring(1, 1)]);
                            ;
                        }

                    }
                }
                else
                {
                    return false;
                }
                return true;
            }
            else
                return false;
        }
        public Listas initializeGrammar(string Texto)
        {
            _scanner = new Scanner(Texto);
            _token = _scanner.GetToken();

            if (_token.Tag == "Variable")
            {
                VariableActual = _token.Value + "→";
                noTerminalInicial = _token.Value;
                reglas.Add("S`→" + noTerminalInicial);
                variables.Add(_token.Value);
                list = i1();
            }
            else
                return null;

            return list;
        }

        public Listas i1()
        {
            _token = _scanner.GetToken();
            if (_token.Tag == "DobPoint")
                return i2();
            else
                return null;
        }

        public Listas i2()
        {
            _token = _scanner.GetToken();
            if (_token.Tag == "Variable" || _token.Tag == "Terminal" || _token.Tag == "Pleca" || _token.Tag == "Jump")
            {
                if (_token.Tag == "Variable")
                {
                    reglasUnidas += _token.Value + " ";
                    if (!variables.Contains(_token.Value))
                    {
                        variables.Add(_token.Value);
                    }
                }
                else if (_token.Tag == "Terminal")
                {
                    reglasUnidas += _token.Value + " ";
                    if (!terminales.Contains(_token.Value))
                    {
                        terminales.Add(_token.Value);
                    }
                }
                else if (_token.Tag == "Pleca")
                {
                    reglas.Add(VariableActual + reglasUnidas);
                    reglasUnidas = "";
                }
                return i2();
            }
            else if (_token.Tag == "PointComa")
            {
                reglas.Add(VariableActual + reglasUnidas);
                reglasUnidas = "";
                return i3();
            }
            else
                return null;
        }

        public Listas i3()
        {
            _token = _scanner.GetToken();
            if (_token.Tag == "Jump")
                return i3();
            else if (_token.Tag == "EOF")
            {
                Grammar grammar = new Grammar(terminales, variables, noTerminalInicial, reglas);
                list = grammar.Guardar();
                return list;
            }
            else if (_token.Tag == "Variable")
            {
                VariableActual = _token.Value + "→";
                return i1();
            }
            else
                return null;
        }
    }
}

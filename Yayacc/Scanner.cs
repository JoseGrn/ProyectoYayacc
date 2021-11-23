using System;
using System.Text.RegularExpressions;

namespace Yayacc
{
    class Scanner
    {
        private string _regexp = "";
        private int _index = 0;
        private bool _final;
        private string word;

        public Scanner(string regexp)
        {
            _regexp = regexp;
            _index = 0;
            _final = false;
        }

        public Token GetToken()
        {
            Token result = new Token();

            word = "";

            Regex rgx1 = new Regex(@"^([_a-zA-Z])([_0-9a-zA-Z])*$");
            Regex rgx2 = new Regex(@"^(')( )?([_A-Za-z0-9!""#%&\(\)*+,-./:;<=>?\[\]^_\{\|\}~ ])+( )?(')$");

            bool TokenFound = false;

            while (!TokenFound)
            {
                while (string.IsNullOrWhiteSpace(new string(_regexp[_index], 1)))
                {
                    _index++;
                }

                string aux = Convert.ToString(_regexp[_index]);

                if (aux == "'")
                {
                    if (_index == _regexp.Length - 1)
                        break;
                    aux = "";
                    word += _regexp[_index];
                    _index++;
                    while (aux != "'")
                    {
                        word += _regexp[_index];
                        aux = Convert.ToString(_regexp[_index]);
                        if (_index == _regexp.Length - 1)
                            break;
                        _index++;
                    }
                }
                else
                {
                    do
                    {
                        if (_final)
                            break;
                        word += _regexp[_index];
                        if (_index == _regexp.Length - 1)
                        {
                            _final = true;
                            break;
                        }
                        _index++;
                        aux = Convert.ToString(_regexp[_index]);
                    } while (!string.IsNullOrWhiteSpace(new string(_regexp[_index], 1)) && aux != ";" && aux != ":" && aux != "|" && word != ":" && word != ";" && word != "|");
                }

                if (word == "\r\n")
                {
                    TokenFound = true;
                    result.Tag = "Jump";
                    result.Value = word;
                }
                else if (word == ";")
                {
                    TokenFound = true;
                    result.Tag = "PointComa";
                    result.Value = word;
                }
                else if (word == ":")
                {
                    TokenFound = true;
                    result.Tag = "DobPoint";
                    result.Value = word;
                }
                else if (word == "|")
                {
                    TokenFound = true;
                    result.Tag = "Pleca";
                    result.Value = word;
                }
                else if (rgx1.IsMatch(word))
                {
                    TokenFound = true;
                    result.Tag = "Variable";
                    result.Value = word;
                }
                else if (rgx2.IsMatch(word))
                {
                    TokenFound = true;
                    result.Tag = "Terminal";
                    result.Value = word;
                }
                else if (_index == _regexp.Length - 1)
                {
                    TokenFound = true;
                    result.Tag = "EOF";
                    result.Value = "Fin";
                }
                else
                {
                    Console.WriteLine("Error en la gramatica");
                    break;
                }

            }

            return result;
        }
    }
}

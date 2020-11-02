using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coctails_Affaires
{
    public class Boisson
    {
        private string _nom;
        private decimal _tauxAlcool;

        public Boisson(string nom, decimal tauxAlcool)
        {
            _nom = nom;
            _tauxAlcool = tauxAlcool;
        }

        public string Nom { get { return _nom; } set { _nom = value; } }
        public decimal TauxAlcool { get { return _tauxAlcool; } set { _tauxAlcool = value; } }
    }
}

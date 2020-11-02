using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coctails_Affaires
{
    public class Ingredient
    {
        private int _quantiteMl;
        private Boisson _boisson;
        private static List<int> UNITES = new List<int>() { 1, 2, 5, 10, 15 };
        private static List<string> MESURES = new List<string> { "trait@", "doigt@", "cuiller@ a cafe", "cuiller@ a soupe", "tasse@" };

        public Ingredient(Boisson uneBoisson, int qteMl)
        {
            _quantiteMl = qteMl;
            _boisson = uneBoisson;
        }

        /**
         * Permet de calculer le volume d'alcool.
         * Base sur le produit entre la quantite de boisson
         * et le taux d'alcool de la boisson.
         */
        public decimal calculerVolumeAlcool()
        {
            return _quantiteMl * _boisson.TauxAlcool;
        }

        /**
         * Permet de calculer le nombre d'unites selon l'echelle de mesure
         */
        public int calculerNbUnites()
        {
            int iFacteur = 0;
            for (int i = 0; i < 5; i++)
                if ((_quantiteMl % UNITES[i]) == 0)
                    iFacteur = i;
            return (_quantiteMl / UNITES[iFacteur]);
        }

        /** 
         * Permet de determiner l'unite de mesure
         * L'unite de mesure est determinee sur le
         * facteur le plus eleve parmi le tableau de
         * correspondance, afin d'obtenir un nombre
         * entier d'unites.
         * @return Chaine de caracteres indiquant l'unite de mesure
         */
        public String determinerUniteMesure()
        {
            int iFacteur = 0;
            for (int i = 0; i < 5; i++)
                if ((_quantiteMl % UNITES[i]) == 0)
                    iFacteur = i;
            string[] parties = MESURES[iFacteur].Split('@');
            String chaine = parties[0];
            if (UNITES[iFacteur] < _quantiteMl)
                chaine += "s";
            if (parties.Length > 1)
                chaine += parties[2];
            return chaine;
        }

        public Boisson Boisson { get { return _boisson; } set { _boisson = value; } }
        public int QuantiteMl { get { return _quantiteMl; } set { _quantiteMl = value; } }
    }
}

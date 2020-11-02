using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coctails_Affaires
{
    public class Cocktail
    {
        private string _nom;
        private decimal _prixVente;
        private List<Ingredient> _ingredients;

        public Cocktail()
        {
            _nom = "";
            _ingredients = new List<Ingredient>();
            _prixVente = 0.00m;
        }

        /**
         * Permet d'ajouter un ingredient au cocktail
         */
        public void ajouter(Ingredient ingredient)
        {
            _ingredients.Add(ingredient);
        }

        /**
         * Permet de calculer le volume en millilitres du cocktail,
         * base sur la somme de toutes les quantites des ingredients
         * qui composent le cocktail.
         */
        public int calculerVolume()
        {
            int volume = 0;
            foreach (Ingredient ingr in _ingredients)
                volume += ingr.QuantiteMl;
            return volume;
        }

        /**
         * Permet de calculer le taux d'alcool moyen du cocktail, base
         * sur la somme des volumes des ingredients reporte sur le 
         * volume du cocktail.
         */
        public decimal calculerTauxAlcoolMoyen()
        {
            decimal totVA = 0.0m;
            int volume = this.calculerVolume();
            foreach (Ingredient ingr in _ingredients)
                totVA += ingr.calculerVolumeAlcool();
            return (volume == 0) ? 0m : totVA / volume;
        }

        /**
        * Teste l'approbation du cocktail par le gerant.
        */
        public bool estApprouve()
        {
            return _prixVente != 0.0m;
        }

        public string Nom { get { return _nom; } set { _nom = value; } }
        public decimal PrixVente { get { return _prixVente; } set { _prixVente = value; } }
        public List<Ingredient> Ingredients { get { return _ingredients; } set { _ingredients = value; } }
    }
}

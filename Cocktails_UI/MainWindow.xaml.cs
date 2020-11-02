using Cocktails_Services;
using Coctails_Affaires;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cocktails_UI
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ChargerBoissons(object sender, RoutedEventArgs e)
        {
            List<Boisson> boissons = PCocktails.getInstance().chargerBoissons();
            tbMessages.AppendText($"Nombre de boissons: {boissons.Count()}\n");
        }

        private void DenombrerCocktailsApprouves(object sender, RoutedEventArgs e)
        {
            int nb = PCocktails.getInstance().denombrerCocktailsApprouves();
            tbMessages.AppendText($"Nb cocktails approuves: {nb}\n");
        }

        private void SupprimerCocktailParadise(object sender, RoutedEventArgs e)
        {
            PCocktails.getInstance().supprimerCocktail("Paradise");
            tbMessages.AppendText("Paradise supprimé\n");
        }

        private void ChargerNomsCocktails(object sender, RoutedEventArgs e)
        {
            List<string> noms = PCocktails.getInstance().chargerNomsCocktails();
            tbMessages.AppendText("--Voir la fenêtre de sortie pour les messages\n");
            tbMessages.AppendText($"Nombre de noms: {noms.Count}\n");
        }

        private void ListerIngredientsParadise(object sender, RoutedEventArgs e)
        {
            tbMessages.AppendText("--Voir la fenêtre de sortie pour les messages\n");
            PCocktails.getInstance().listerIngredients("Paradise");
        }

        private void Renommer1reBoisson(object sender, RoutedEventArgs e)
        {
            tbMessages.AppendText("--Voir la fenêtre de sortie pour les messages\n");
            PCocktails.getInstance().renommerPremiereBoisson();
        }

        private void Renommer2eBoisson(object sender, RoutedEventArgs e)
        {
            tbMessages.AppendText("--Voir la fenêtre de sortie pour les messages\n");
            PCocktails.getInstance().renommerSecondeBoisson();
        }

        private void AjouterSupprimerBoisson(object sender, RoutedEventArgs e)
        {
            tbMessages.AppendText("--Voir la fenêtre de sortie pour les messages\n");
            PCocktails.getInstance().ajouterEtSupprimerBoissons();
        }
    }
}

using Coctails_Affaires;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocktails_Services
{
    public class PCocktails
    {
        private static PCocktails _pCocktails = null;
        private string chaineConnexion = "";

        private PCocktails()
        {
            // Constructeur
        }

        public static PCocktails getInstance()
        {
            if (_pCocktails == null)
                _pCocktails = new PCocktails();
            return _pCocktails;
        }

        public List<Boisson> chargerBoissons()
        {
            List<Boisson> boissons = new List<Boisson>();

            using (SqlConnection connexion = new SqlConnection(chaineConnexion))
            {
                SqlCommand cmdSelectBoissons = new SqlCommand("SELECT nom, tauxAlcool FROM GC_Boisson", connexion);
                connexion.Open();
                SqlDataReader reader = cmdSelectBoissons.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        //Boisson b = new Boisson(reader["nom"].ToString(), Decimal.Parse(reader["tauxAlcool"].ToString()));
                        Boisson b = new Boisson(reader.GetString(reader.GetOrdinal("nom")), reader.GetDecimal(reader.GetOrdinal("tauxAlcool")));
                        //reader.GetDecimal(1);
                        Debug.WriteLine(String.Format("{0}, {1}", reader[0], reader[1]));
                        boissons.Add(b);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return boissons;
        }

        public int denombrerCocktailsApprouves()
        {
            int nbCocktailsApprouves = 0;
            using (SqlConnection connexion = new SqlConnection(chaineConnexion))
            {
                SqlCommand cmdDenombrerCocktailsApprouves = new SqlCommand("SELECT COUNT(*) as nbCocktails FROM GC_COCKTAIL Where prixVente > 0.0", connexion);
                connexion.Open();
                nbCocktailsApprouves = (int)cmdDenombrerCocktailsApprouves.ExecuteScalar();
                connexion.Close();
            }
            return nbCocktailsApprouves;
        }

        public void supprimerCocktail(string nomCocktail)
        {
            using (SqlConnection connexion = new SqlConnection(chaineConnexion))
            {
                SqlCommand cmdSupprimerCocktailSelonNom = new SqlCommand("DELETE FROM GC_Cocktail Where nom = @nomCocktailASupprimer", connexion);
                cmdSupprimerCocktailSelonNom.Parameters.Add("@nomCocktailASupprimer", SqlDbType.VarChar);
                cmdSupprimerCocktailSelonNom.Parameters["@nomCocktailASupprimer"].Value = nomCocktail;
                connexion.Open();
                int nbCocktailsSupprimes = (int)cmdSupprimerCocktailSelonNom.ExecuteNonQuery();
                Debug.WriteLine("{0} cocktail(s) supprimé(s) pour le nom {1}", nbCocktailsSupprimes, nomCocktail);
                connexion.Close();
            }
        }

        public List<string> chargerNomsCocktails()
        {
            List<string> nomsCocktails = new List<string>();
            using (SqlConnection connexion = new SqlConnection(chaineConnexion))
            {
                SqlDataAdapter daCocktails = new SqlDataAdapter("SELECT * from GC_Cocktail", connexion);
                DataSet dsCocktails = new DataSet();
                daCocktails.Fill(dsCocktails, "GC_Cocktail");
                foreach (DataRow tupleCocktail in dsCocktails.Tables["GC_Cocktail"].Rows)
                {
                    Debug.WriteLine(String.Format("Nom: {0}", tupleCocktail["nom"]));
                    nomsCocktails.Add(tupleCocktail["nom"].ToString());
                }
                return nomsCocktails;
            }
        }

        public void listerIngredients(string nomCocktail)
        {
            using (SqlConnection connexion = new SqlConnection(chaineConnexion))
            {
                SqlCommand cmdRechercheCocktail = new SqlCommand("SELECT oidCocktail FROM GC_COCKTAIL Where nom = @nomCocktail", connexion);
                cmdRechercheCocktail.Parameters.Add("@nomCocktail", SqlDbType.VarChar);
                cmdRechercheCocktail.Parameters["@nomCocktail"].Value = nomCocktail;
                connexion.Open();
                Debug.WriteLine("{0}", cmdRechercheCocktail.ExecuteScalar());
                int oidCocktail = Convert.ToInt32(cmdRechercheCocktail.ExecuteScalar());
                connexion.Close();

                DataSet dsRecette = new DataSet();
                SqlDataAdapter daIngredients = new SqlDataAdapter("SELECT * from GC_Ingredient where oidCocktail = " + oidCocktail, connexion);
                SqlDataAdapter daBoissons = new SqlDataAdapter("SELECT * from GC_Boisson", connexion);
                daIngredients.Fill(dsRecette, "GC_Ingredient");
                daBoissons.Fill(dsRecette, "GC_Boisson");
                DataColumn colonneParent = dsRecette.Tables["GC_Ingredient"].Columns["oidBoisson"];
                DataColumn colonneEnfant = dsRecette.Tables["GC_Boisson"].Columns["oidBoisson"];
                DataRelation relation = new DataRelation("Ingredient_Boisson", colonneParent, colonneEnfant, false);
                dsRecette.Relations.Add(relation);

                foreach (DataRow tupleParent in dsRecette.Tables["GC_Ingredient"].Rows)
                {
                    DataRow[] tuplesEnfants = tupleParent.GetChildRows("Ingredient_Boisson");
                    Debug.WriteLine(String.Format("  -- {0}ml de {1}", tupleParent["quantiteMl"], tuplesEnfants[0]["nom"]));
                }
            }
        }

        public void renommerPremiereBoisson()
        {
            using (SqlConnection connexion = new SqlConnection(chaineConnexion))
            {
                SqlDataAdapter daBoissons = new SqlDataAdapter("SELECT * FROM GC_Boisson", connexion);
                daBoissons.UpdateCommand = new SqlCommand("UPDATE GC_Boisson SET nom = @nom, tauxAlcool = @tauxAlcool WHERE oidBoisson = @oidBoisson", connexion);
                daBoissons.UpdateCommand.Parameters.Add("@nom", SqlDbType.VarChar, 50, "nom");
                SqlParameter paramTauxAlcool = new SqlParameter("@tauxAlcool", SqlDbType.Decimal, 2, "tauxAlcool");
                paramTauxAlcool.Precision = 2;
                paramTauxAlcool.Scale = 2;
                daBoissons.UpdateCommand.Parameters.Add(paramTauxAlcool);
                SqlParameter paramOidBoisson = new SqlParameter("@oidBoisson", SqlDbType.Int);
                paramOidBoisson.SourceColumn = "oidBoisson";
                paramOidBoisson.SourceVersion = DataRowVersion.Original;
                daBoissons.UpdateCommand.Parameters.Add(paramOidBoisson);

                DataTable dtBoisson = new DataTable();
                daBoissons.Fill(dtBoisson);
                DataRow premierTuple = dtBoisson.Rows[0];
                premierTuple["nom"] = "CTHULHU";
                daBoissons.Update(dtBoisson);
                Debug.WriteLine("1re boisson modifiée");
            }
        }

        public void renommerSecondeBoisson()
        {
            using (SqlConnection connexion = new SqlConnection(chaineConnexion))
            {
                SqlDataAdapter daBoissons = new SqlDataAdapter("SELECT * FROM GC_Boisson", connexion);
                SqlCommandBuilder builder = new SqlCommandBuilder(daBoissons);
                DataTable dtBoisson = new DataTable();
                daBoissons.Fill(dtBoisson);
                DataRow premierTuple = dtBoisson.Rows[1];
                premierTuple["nom"] = "Nyarlathotep";
                daBoissons.Update(dtBoisson);
                Debug.WriteLine("2e boisson modifiée");
            }
        }
        public void ajouterEtSupprimerBoissons()
        {
            using (SqlConnection connexion = new SqlConnection(chaineConnexion))
            {
                SqlDataAdapter daBoissons = new SqlDataAdapter("SELECT * FROM GC_Boisson", connexion);
                SqlCommandBuilder builder = new SqlCommandBuilder(daBoissons);
                DataTable dtBoisson = new DataTable();
                daBoissons.Fill(dtBoisson);
                string nomBoissonDetruite = (string)dtBoisson.Rows[2]["nom"];
                dtBoisson.Rows[2].Delete();
                DataRow nouvelleRangee = dtBoisson.NewRow();
                nouvelleRangee["nom"] = "Ithaqua";
                nouvelleRangee["tauxAlcool"] = 0.5m;
                dtBoisson.Rows.Add(nouvelleRangee);
                daBoissons.Update(dtBoisson);
                Debug.WriteLine("Détruire la boisson {0}", nomBoissonDetruite);
                Debug.WriteLine("Ajouter une nouvelle boisson");
            }
        }
    }
}

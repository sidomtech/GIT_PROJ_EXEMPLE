//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.Serialization;
//using System.ServiceModel;
//using System.Text;
using ServiceAgence.DataContracts;
namespace ServiceAgence
{
    public class Agence : IAgence
    {
        internal const string DATABASE_PATH = "";
        public ResultatListeBiensImmobiliers LireListeBiensImmobiliers(CriteresRechercheBiensImmobiliers criteres, int? page, int? nbBiens)
        {
            return Tools.GestionnaireOperation.ExecuterOperation<ResultatListeBiensImmobiliers, CriteresRechercheBiensImmobiliers, int?>(this.OperationLireListeBiensImmobiliers, criteres, page, nbBiens);
        }
        private ResultatListeBiensImmobiliers OperationLireListeBiensImmobiliers(SQL.GestionnaireBDD bdd, Tools.Log log, CriteresRechercheBiensImmobiliers criteres, params int?[] parametres)
        {
            ResultatListeBiensImmobiliers resultat = new ResultatListeBiensImmobiliers();
            // COM DRO avant remontee qsdsqd
            // COM DRO avant remontee 002
            // Chargement des paramètres
            int? page = null, nbBiens = null;
            if (parametres.Length > 0) page = parametres[0];
            if (parametres.Length > 1) nbBiens = parametres[1];

            if (criteres == null)
            {
                Tools.GestionnaireOperation.LireParametreURI("criteres", out criteres);
            }

            if (page == null)
            {
                int tmp;
                if (Tools.GestionnaireOperation.LireParametreURIEntier("page", out tmp))
                    page = tmp;
            }

            if (nbBiens == null)
            {
                int tmp;
                if (Tools.GestionnaireOperation.LireParametreURIEntier("nbBiens", out tmp))
                    nbBiens = tmp;
            }
            
            // Lecture des biens immobiliers
            ListeBiensImmobiliers l = bdd.LireContenuBDD(criteres, page, nbBiens);
            if (l == null)
            {
                Tools.GestionnaireOperation.GererErreur(resultat, log, bdd.ErrorMessage);
                return resultat;
            }
            resultat.Liste.List.AddRange(l.List);
            resultat.Liste.Page = l.Page;
            resultat.Liste.PagesCount = l.PagesCount;
            resultat.Liste.Start = l.Start;
            resultat.Liste.SelectCount = l.SelectCount;
            resultat.Liste.TakeCount = l.TakeCount;
            resultat.Liste.TotalCount = l.TotalCount;

            return resultat;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultatBienImmobilier LireDetailsBienImmobilier(string id)
        {
            return Tools.GestionnaireOperation.ExecuterOperation<ResultatBienImmobilier, string, object>(this.OperationLireDetailsBienImmobilier, id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bdd"></param>
        /// <param name="log"></param>
        /// <param name="id"></param>
        /// <param name="nonUtilise"></param>
        /// <returns></returns>
        private ResultatBienImmobilier OperationLireDetailsBienImmobilier(SQL.GestionnaireBDD bdd, Tools.Log log, string id, params object[] nonUtilise)
        {
            ResultatBienImmobilier resultat = new ResultatBienImmobilier();
            // Conversion de l'identifiant
            int idBien;
            if (!int.TryParse(id, out idBien))
            {
                Tools.GestionnaireOperation.GererErreur(resultat, log, "L'identifiant du bien immobilier à charger est invalide !");
                return resultat;
            }
            // Lecture du bien immobilier
            BienImmobilier b = bdd.LireDetailsBienImmobilier(idBien);
            if (b == null)
            {
                Tools.GestionnaireOperation.GererErreur(resultat, log, bdd.ErrorMessage);
                return resultat;
            }
            resultat.Bien = b;
            //
            return resultat;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bien"></param>
        /// <returns></returns>
        public ResultatBool AjouterBienImmobilier(BienImmobilier bien)
        {
            return Tools.GestionnaireOperation.ExecuterOperation<ResultatBool, BienImmobilier, object>(this.OperationAjouterBienImmobilier, bien);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bdd"></param>
        /// <param name="log"></param>
        /// <param name="bien"></param>
        /// <param name="nonUtilise"></param>
        /// <returns></returns>
        private ResultatBool OperationAjouterBienImmobilier(SQL.GestionnaireBDD bdd, Tools.Log log, BienImmobilier bien, params object[] nonUtilise)
        {
            ResultatBool resultat = new ResultatBool();

            // Ajout du bien immobilier
            resultat.Valeur = bdd.AjouterBienImmobilier(bien);
            if (!resultat.Valeur)
            {
                Tools.GestionnaireOperation.GererErreur(resultat, log, bdd.ErrorMessage);
                return resultat;
            }

            return resultat;
        }

        public ResultatBool ModifierBienImmobilier(BienImmobilier bien)
        {
            return Tools.GestionnaireOperation.ExecuterOperation<ResultatBool, BienImmobilier, object>(this.OperationModifierBienImmobilier, bien);
        }
        private ResultatBool OperationModifierBienImmobilier(SQL.GestionnaireBDD bdd, Tools.Log log, BienImmobilier bien, params object[] nonUtilise)
        {
            ResultatBool resultat = new ResultatBool();

            // Mise à jour du bien immobilier
            resultat.Valeur = bdd.ModifierBienImmobilier(bien);
            if (!resultat.Valeur)
            {
                Tools.GestionnaireOperation.GererErreur(resultat, log, bdd.ErrorMessage);
                return resultat;
            }

            return resultat;
        }

        public ResultatBool SupprimerBienImmobilier(string id)
        {
            return Tools.GestionnaireOperation.ExecuterOperation<ResultatBool, string, object>(this.OperationSupprimerBienImmobilier, id);
        }
        private ResultatBool OperationSupprimerBienImmobilier(SQL.GestionnaireBDD bdd, Tools.Log log, string id, params object[] nonUtilise)
        {
            ResultatBool resultat = new ResultatBool();

            // Conversion de l'identifiant
            int idBien;
            if (!int.TryParse(id, out idBien))
            {
                Tools.GestionnaireOperation.GererErreur(resultat, log, "L'identifiant du bien immobilier à charger est invalide !");
                return resultat;
            }

            // Suppression du bien immobilier
            resultat.Valeur = bdd.SupprimerBienImmobilier(idBien);
            if (!resultat.Valeur)
            {
                Tools.GestionnaireOperation.GererErreur(resultat, log, bdd.ErrorMessage);
                return resultat;
            }

            return resultat;
        }
    }
}

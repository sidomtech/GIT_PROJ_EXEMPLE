using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ServiceAgence
{
    [DataContract]
    public class BienImmobilierBase
    {

        #region Enumérations

        public enum eTypeTransaction
        {
            Vente = 0,
            Location = 1
        }
        public enum eTypeBien
        {
            Appartement = 0,
            Maison = 1,
            Garage = 2,
            Terrain = 3
        }
        public enum eTypeChauffage
        {
            Individuel = 0,
            Collectif = 1
        }
        public enum eEnergieChauffage
        {
            Fioul = 0,
            Gaz = 1,
            Electrique = 2,
            Bois = 3
        }

        #endregion

        #region Attributs

        protected int _id;
        protected eTypeTransaction _typeTransaction = eTypeTransaction.Vente;
        protected eTypeBien _typeBien = eTypeBien.Appartement;
        protected string _description = "";
        protected double _prix = 0;
        protected double _montantCharges = 0;
        protected List<string> _photosBase64 = new List<string>();

        #endregion

        #region Propriétés

        [DataMember]
        public int Id
        {
            get { return _id; }
        }

        [DataMember]
        public eTypeTransaction TypeTransaction
        {
            get { return _typeTransaction; }
            set { _typeTransaction = value; }
        }

        [DataMember]
        public eTypeBien TypeBien
        {
            get { return _typeBien; }
            set { _typeBien = value; }
        }

        [DataMember]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        [DataMember]
        public double Prix
        {
            get { return _prix; }
            set { _prix = value; }
        }

        [DataMember]
        public double MontantCharges
        {
            get { return _montantCharges; }
            set { _montantCharges = value; }
        }

        [DataMember]
        public string PhotoPrincipaleBase64
        {
            get
            {
                if (_photosBase64.Count > 0)
                    return _photosBase64[0];
                else
                    return "";
            }
            set
            {
                if (_photosBase64.Count > 0)
                    _photosBase64[0] = value;
                else
                    _photosBase64.Add(value);
            }
        }
        
        #endregion

        internal BienImmobilierBase(int id)
        {
            this._id = id;
        }

    }

}
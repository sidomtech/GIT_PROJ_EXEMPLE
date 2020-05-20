using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web;

namespace ServiceAgence
{
    [DataContract]
    public class BienImmobilier : BienImmobilierBase
    {

        #region Attributs
        
        double _surface = 0;
        int _nbPieces = 0;
        int _numEtage = 0;
        int _nbEtages = 0;
        eTypeChauffage _typeChauffage = eTypeChauffage.Individuel;
        eEnergieChauffage _energieChauffage = eEnergieChauffage.Fioul;
        
        #endregion

        #region Propriétés

        [DataMember]
        public double Surface
        {
            get { return _surface; }
            set { _surface = value; }
        }

        [DataMember]
        public int NbPieces
        {
            get { return _nbPieces; }
            set { _nbPieces = value; }
        }

        [DataMember]
        public int NumEtage
        {
            get { return _numEtage; }
            set { _numEtage = value; }
        }

        [DataMember]
        public int NbEtages
        {
            get { return _nbEtages; }
            set { _nbEtages = value; }
        }

        [DataMember]
        public eTypeChauffage TypeChauffage
        {
            get { return _typeChauffage; }
            set { _typeChauffage = value; }
        }

        [DataMember]
        public eEnergieChauffage EnergieChauffage
        {
            get { return _energieChauffage; }
            set { _energieChauffage = value; }
        }

        [DataMember]
        public List<string> PhotosBase64
        {
            get { return _photosBase64; }
        }

        #endregion

        internal BienImmobilier(int id) : base(id) { }
    }
}
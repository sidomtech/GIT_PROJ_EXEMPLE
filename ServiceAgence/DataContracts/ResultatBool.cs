using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web;

namespace ServiceAgence.DataContracts
{
    [DataContract]
    public class ResultatBool : ResultatOperation
    {
        [DataMember]
        public bool Valeur { get; set; }

        public ResultatBool() : base()
        {
            this.Valeur = false;
        }
    }
}

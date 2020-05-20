using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ServiceAgence.DataContracts;

namespace ServiceAgence
{
    [ServiceContract]
    public interface IAgence
    {

        [OperationContract]
        [WebGet(UriTemplate = "biens", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ResultatListeBiensImmobiliers LireListeBiensImmobiliers(CriteresRechercheBiensImmobiliers criteres, int? page, int? nbBiens);

        [OperationContract]
        [WebGet(UriTemplate = "biens/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ResultatBienImmobilier LireDetailsBienImmobilier(string id);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "biens/{bien}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ResultatBool AjouterBienImmobilier(BienImmobilier bien);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "biens/{bien}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ResultatBool ModifierBienImmobilier(BienImmobilier bien);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "biens/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ResultatBool SupprimerBienImmobilier(string id);
    }
}

using System.ServiceModel; 

namespace Web.API
{
    [ServiceContract] 
    public interface ILogService
    {
        [OperationContract]
        string SistemLoguKaydet(string mesaj);
    }
}
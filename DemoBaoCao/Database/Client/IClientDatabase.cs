using DemoBaoCao.Models;

namespace DemoBaoCao.Database.Client
{
    public interface IClientDatabase
    {
        List<Clients> AllClient();

        List<Clients> ClientById(int ClientName);

        List<Clients> ClientBy(string ClientName, string ClientIdNumber, string ClientSMSNumber);

        List<Clients> AddClient(string ClientName, string ClientAddress, string ClientIdNumber, 
                                string CientIdLssuePlace, DateTime ClientIDLssueDate,
                                string ClientSMSNumber, string ClientEmail, string ClientBranch);

        List<Clients> EditClient(int ClientId, string ClientName, string ClientAddress, string ClientIdNumber,
                                string CientIdLssuePlace, DateTime ClientIDLssueDate, string ClientSMSNumber,
                                string ClientEmail, string ClientBranch);

        List<Clients> DeleteClient(int ClientId);

    }
}

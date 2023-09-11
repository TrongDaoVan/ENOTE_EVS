using DemoBaoCao.Models;

namespace DemoBaoCao.Database.Contract
{
    public interface IContractDatabase
    {
        List<Contracts> AllContract();

        List<Contracts> ContractById(int ContractId);
        List<Contracts> ContractByValueCode(string Value, string ContractCode);

        List<Contracts> AddContract(decimal ContractValue, decimal ContractAmountWithdrawn,
                                        decimal ContractRemainingAmount, DateTime ContractBorrowedDate,
                                        int ContractNumberOfDays, DateTime ContractEffectiveDate,
                                        DateTime ContractPaymentDate, int ClientId, int ProductId);

        List<Contracts> EditContract(int ContractId, decimal ContractValue, decimal ContractAmountWithdrawn,
                                        decimal ContractRemainingAmount, DateTime ContractBorrowedDate,
                                        int ContractNumberOfDays, DateTime ContractEffectiveDate,
                                        DateTime ContractPaymentDate, int ClientId, int ProductId);

        List<Contracts> DeleteContract(int ContractId);

        List<Contracts> ChiTietHD(int ContractId);
        decimal TotalMoney(int ContractId);

        //List<Contracts> ContractByClientId(int ClientId);
    }
}

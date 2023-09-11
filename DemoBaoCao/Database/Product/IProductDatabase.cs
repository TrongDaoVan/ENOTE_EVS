using DemoBaoCao.Models;

namespace DemoBaoCao.Database.Product
{
    public interface IProductDatabase
    {
        List<Products> AllProduct();

        List<Products> ProductById(int ProductId);

        List<Products> ProductByName(string ProductName);

        List<Products> AddProduct(string ProductName, DateTime ProductEffectiveDateFrom, DateTime ProductEffectiveDateTo,
                                  int ProductMinimumSendingDate, int ProductMaximumSendingDate, 
                                  decimal ProductPackageMinimumAmount, decimal ProductPackageMaximumAmount, 
                                  decimal ProductTotalRemainingLimit, decimal ProductNonTermRate);

        List<Products> EditProduct(int ProductId, string ProductName, DateTime ProductEffectiveDateFrom, 
                                   DateTime ProductEffectiveDateTo, int ProductMinimumSendingDate, 
                                   int ProductMaximumSendingDate, decimal ProductPackageMinimumAmount,
                                   decimal ProductPackageMaximumAmount, decimal ProductTotalRemainingLimit, 
                                   decimal ProductNonTermRate);

        List<Products> DeleteProduct(int ProductId);
    }
}

using DemoBaoCao.Models;
using Microsoft.Extensions.Options;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics.Contracts;
using Microsoft.CodeAnalysis;

namespace DemoBaoCao.Database.Product
{
    public class ProductDatabase : IProductDatabase
    {
        private readonly string _connectionString;

        public ProductDatabase()
        {
            _connectionString = "Data Source=DESKTOP-OVPEISB\\SQLEXPRESS;Initial Catalog=DemoEnd;Persist Security Info=False;User ID=sa;Password=sa;";
        }



        // phương thức hiện danh sách sản phẩm
        public List<Products> AllProduct()
        {
            var procedure = "Product_All";

            using (var connection = new SqlConnection(_connectionString))
            {
                var results = connection.Query<Products>(procedure, commandType: CommandType.StoredProcedure).ToList();
                return results;
            }
        }



        // phương thức tìm kiếm sản phẩm theo id
        public List<Products> ProductById(int ProductId)
        {
            var procedure = "Product_ById";
            var values = new { ProductId = ProductId };

            using (var con = new SqlConnection(_connectionString))
            {
                var results = con.Query<Products>(procedure, values, commandType: CommandType.StoredProcedure).ToList();
                return results;
            }
        }



        // Tìm kiếm sản phẩm theo tên
        public List<Products> ProductByName(string ProductName)
        {
            var procedure = "Product_ByName";
            var values = new { ProductName = ProductName };

            using (var con = new SqlConnection(_connectionString))
            {
                var results = con.Query<Products>(procedure, values, commandType: CommandType.StoredProcedure).ToList();
                return results;
            }

        }



        // thêm sản phẩm mới 
        //public List<Products> AddProduct(string ProductName, DateTime ProductEffectiveDateFrom, DateTime ProductEffectiveDateTo,
        //                                int ProductMinimumSendingDate, int ProductMaximumSendingDate, decimal ProductPackageMinimumAmount,
        //                                decimal ProductPackageMaximumAmount, decimal ProductTotalRemainingLimit, decimal ProductNonTermRate)
        //{
        //    using (SqlConnection con = new SqlConnection(_connectionString))
        //    {
        //        con.Open();

        //        var cmd = new SqlCommand("Product_Add", con);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.AddWithValue("@ProductName", ProductName);
        //        cmd.Parameters.AddWithValue("@ProductEffectiveDateFrom", ProductEffectiveDateFrom);
        //        cmd.Parameters.AddWithValue("@ProductEffectiveDateTo", ProductEffectiveDateTo);
        //        cmd.Parameters.AddWithValue("@ProductMinimumSendingDate", ProductMinimumSendingDate);
        //        cmd.Parameters.AddWithValue("@ProductMaximumSendingDate", ProductMaximumSendingDate);
        //        cmd.Parameters.AddWithValue("@ProductPackageMinimumAmount", ProductPackageMinimumAmount);
        //        cmd.Parameters.AddWithValue("@ProductPackageMaximumAmount", ProductPackageMaximumAmount);
        //        cmd.Parameters.AddWithValue("@ProductTotalRemainingLimit", ProductTotalRemainingLimit);
        //        cmd.Parameters.AddWithValue("@ProductNonTermRate", ProductNonTermRate);

        //        cmd.ExecuteNonQuery();

        //        con.Close();
        //    }
        //    return AllProduct();
        //}


        // thêm sản phẩm sử dụng thư viện dapper
        public List<Products> AddProduct(string ProductName, DateTime ProductEffectiveDateFrom, DateTime ProductEffectiveDateTo,
                                int ProductMinimumSendingDate, int ProductMaximumSendingDate, decimal ProductPackageMinimumAmount,
                                decimal ProductPackageMaximumAmount, decimal ProductTotalRemainingLimit, decimal ProductNonTermRate)
        {
            var procedure = "Product_Add";
            var values = new
            {
                ProductName = ProductName,
                ProductEffectiveDateFrom = ProductEffectiveDateFrom,
                ProductEffectiveDateTo = ProductEffectiveDateTo,
                ProductMinimumSendingDate = ProductMinimumSendingDate,
                ProductMaximumSendingDate = ProductMaximumSendingDate,
                ProductPackageMinimumAmount = ProductPackageMinimumAmount,
                ProductPackageMaximumAmount = ProductPackageMaximumAmount,
                ProductTotalRemainingLimit = ProductTotalRemainingLimit,
                ProductNonTermRate = ProductNonTermRate
            };

            using (var con = new SqlConnection(_connectionString))
            {
                var results = con.Query<Products>(procedure, values, commandType: CommandType.StoredProcedure).ToList();
                return results;
            }
        }



        // sửa thông tin sản phẩm
        //public List<Products> EditProduct(int ProductId, string ProductName, DateTime ProductEffectiveDateFrom, DateTime ProductEffectiveDateTo,
        //                                int ProductMinimumSendingDate, int ProductMaximumSendingDate, decimal ProductPackageMinimumAmount,
        //                                decimal ProductPackageMaximumAmount, decimal ProductTotalRemainingLimit, decimal ProductNonTermRate)
        //{
        //    using (SqlConnection con = new SqlConnection(_connectionString))
        //    {
        //        con.Open();

        //        var cmd = new SqlCommand("Product_Edit", con);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.AddWithValue("@ProductId", ProductId);
        //        cmd.Parameters.AddWithValue("@ProductName", ProductName);
        //        cmd.Parameters.AddWithValue("@ProductEffectiveDateFrom", ProductEffectiveDateFrom);
        //        cmd.Parameters.AddWithValue("@ProductEffectiveDateTo", ProductEffectiveDateTo);
        //        cmd.Parameters.AddWithValue("@ProductMinimumSendingDate", ProductMinimumSendingDate);
        //        cmd.Parameters.AddWithValue("@ProductMaximumSendingDate", ProductMaximumSendingDate);
        //        cmd.Parameters.AddWithValue("@ProductPackageMinimumAmount", ProductPackageMinimumAmount);
        //        cmd.Parameters.AddWithValue("@ProductPackageMaximumAmount", ProductPackageMaximumAmount);
        //        cmd.Parameters.AddWithValue("@ProductTotalRemainingLimit", ProductTotalRemainingLimit);
        //        cmd.Parameters.AddWithValue("@ProductNonTermRate", ProductNonTermRate);

        //        cmd.ExecuteNonQuery();

        //        con.Close();
        //    }
        //    return AllProduct();
        //}


        // sửa thông tin sản phẩm sử dụng dapper 
        public List<Products> EditProduct(int ProductId, string ProductName, DateTime ProductEffectiveDateFrom, DateTime ProductEffectiveDateTo,
                                        int ProductMinimumSendingDate, int ProductMaximumSendingDate, decimal ProductPackageMinimumAmount,
                                        decimal ProductPackageMaximumAmount, decimal ProductTotalRemainingLimit, decimal ProductNonTermRate)
        {
            var procedure = "Product_Edit";
            var values = new
            {
                ProductId = ProductId,
                ProductName = ProductName,
                ProductEffectiveDateFrom = ProductEffectiveDateFrom,
                ProductEffectiveDateTo = ProductEffectiveDateTo,
                ProductMinimumSendingDate = ProductMinimumSendingDate,
                ProductMaximumSendingDate = ProductMaximumSendingDate,
                ProductPackageMinimumAmount = ProductPackageMinimumAmount,
                ProductPackageMaximumAmount = ProductPackageMaximumAmount,
                ProductTotalRemainingLimit = ProductTotalRemainingLimit,
                ProductNonTermRate = ProductNonTermRate
            };

            using (var con = new SqlConnection(_connectionString))
            {
                var results = con.Query<Products>(procedure, values, commandType: CommandType.StoredProcedure).ToList();
                return results;
            }
        }



        // xóa thông tin sản phẩm 
        //public List<Products> DeleteProduct(int ProductId)
        //{
        //    using (SqlConnection con = new SqlConnection(_connectionString))
        //    {
        //        con.Open();

        //        var cmd = new SqlCommand("Product_Delete", con);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.AddWithValue("@ProductId", ProductId);
        //        cmd.ExecuteNonQuery();

        //        con.Close();
        //    }
        //    return AllProduct();
        //}


        // xóa thông tin khách hàng dùng dappper
        public List<Products> DeleteProduct(int ProductId)
        {
            var procedure = "Product_Delete";
            var values = new
            {
                ProductId = ProductId
            };

            using (var con = new SqlConnection(_connectionString))
            {
                var results = con.Query<Products>(procedure, values, commandType: CommandType.StoredProcedure).ToList();
                return results;
            }
        }
    }
}

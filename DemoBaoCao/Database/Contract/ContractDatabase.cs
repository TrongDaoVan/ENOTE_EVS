using DemoBaoCao.Models;
using Microsoft.Extensions.Options;
using Dapper;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data;
using System.Diagnostics.Contracts;

namespace DemoBaoCao.Database.Contract
{
    public class ContractDatabase : IContractDatabase
    {
        private readonly string _connectionString;

        public ContractDatabase()
        {
            _connectionString = "Data Source=DESKTOP-OVPEISB\\SQLEXPRESS;Initial Catalog=DemoEnd;Persist Security Info=False;User ID=sa;Password=sa;";
        }



        // phương thức in danh sách
        public List<Contracts> AllContract()
        {
            try
            {
                var procedure = "Contract_GetList";
                var values = new { ClientCode = "" };

                using (var con = new SqlConnection(_connectionString))
                {
                    var results = con.Query<Contracts>(procedure, values, commandType: CommandType.StoredProcedure).ToList();
                    return results;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }



        // phương thức tìm Hợp đồng theo id
        public List<Contracts> ContractById(int ContractId)
        {
            var procedure = "ContractById";
            var values = new { ContractId = ContractId };

            using (var con = new SqlConnection(_connectionString))
            {
                var results = con.Query<Contracts>(procedure, values, commandType: CommandType.StoredProcedure).ToList();
                return results;
            }
        }



        // phương thức tìm kiếm theo tên khách hàng, cccd, sdt và mã hợp đồng
        public List<Contracts> ContractByValueCode(string Value, string ContractCode)
        {
            var procedure = "Contract_ByNameCccdSMS";
            var values = new { Value = Value, ContractCode = ContractCode};

            using (var con = new SqlConnection(_connectionString))
            {
                var results = con.Query<Contracts>(procedure, values, commandType: CommandType.StoredProcedure).ToList();
                return results;
            }
        }



            // phương thức thêm Hợp đồng
            public List<Contracts> AddContract(decimal ContractValue, decimal ContractAmountWithdrawn,
                                           decimal ContractRemainingAmount, DateTime ContractBorrowedDate,
                                           int ContractNumberOfDays, DateTime ContractEffectiveDate, 
                                           DateTime ContractPaymentDate, int ClientId, int ProductId)
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    var cmd = new SqlCommand("Contract_Add", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ContractValue", ContractValue);
                    cmd.Parameters.AddWithValue("@ContractAmountWithdrawn", ContractAmountWithdrawn);
                    cmd.Parameters.AddWithValue("@ContractRemainingAmount", ContractRemainingAmount);
                    cmd.Parameters.AddWithValue("@ContractBorrowedDate", ContractBorrowedDate);
                    cmd.Parameters.AddWithValue("@ContractNumberOfDays", ContractNumberOfDays);
                    cmd.Parameters.AddWithValue("@ContractEffectiveDate", ContractEffectiveDate);
                    cmd.Parameters.AddWithValue("@ContractPaymentDate", ContractPaymentDate);
                    cmd.Parameters.AddWithValue("@ClientId", ClientId);
                    cmd.Parameters.AddWithValue("@ProductId", ProductId);

                    cmd.ExecuteNonQuery();

                    con.Close();
                }
                return AllContract();
            }



        // phương thức sửa hợp đồng
        public List<Contracts> EditContract(int ContractId, decimal ContractValue, decimal ContractAmountWithdrawn,
                                            decimal ContractRemainingAmount, DateTime ContractBorrowedDate,
                                            int ContractNumberOfDays, DateTime ContractEffectiveDate,
                                            DateTime ContractPaymentDate, int ClientId, int ProductId)
        {

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                var cmd = new SqlCommand("Contract_Edit", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ContractId", ContractId);
                cmd.Parameters.AddWithValue("@ContractValue", ContractValue);
                cmd.Parameters.AddWithValue("@ContractAmountWithdrawn", ContractAmountWithdrawn);
                cmd.Parameters.AddWithValue("@ContractRemainingAmount", ContractRemainingAmount);
                cmd.Parameters.AddWithValue("@ContractBorrowedDate", ContractBorrowedDate);
                cmd.Parameters.AddWithValue("@ContractNumberOfDays", ContractNumberOfDays);
                cmd.Parameters.AddWithValue("@ContractEffectiveDate", ContractEffectiveDate);
                cmd.Parameters.AddWithValue("@ContractPaymentDate", ContractPaymentDate);
                cmd.Parameters.AddWithValue("@ClientId", ClientId);
                cmd.Parameters.AddWithValue("@ProductId", ProductId);

                cmd.ExecuteNonQuery();

                con.Close();
            }
            return AllContract();
        }


        public List<Contracts> DeleteContract(int ContractId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                var cmd = new SqlCommand("Contract_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ContractId", ContractId);
                cmd.ExecuteNonQuery();

                con.Close();
            }
            return AllContract();
        }



        // hiện chi tiết danh sách hợp đồng
        public List<Contracts> ChiTietHD(int ContractId)
        {
            var procedure = "Contract_Client_Product_ById";
            var values = new { ContractId = ContractId };

            using (var con = new SqlConnection(_connectionString))
            {
                var results = con.Query<Contracts>(procedure, values, commandType: CommandType.StoredProcedure).ToList();
                return results;
            }
        }



        // Tính tiền lãi nhận được
        public decimal TotalMoney(int ContractId)
        {
            var procedure = "Contract_TotalMoney";
            var values = new { ContractId = ContractId };

            using (var con = new SqlConnection(_connectionString))
            {
                var result = con.QueryFirstOrDefault<dynamic>(procedure, values, commandType: CommandType.StoredProcedure);

                if (result != null)
                {
                    // Lấy giá trị TotalMoney từ đối tượng dynamic result
                    decimal totalMoney = result.TotalMoney;
                    return totalMoney;
                }
                else
                {
                    throw new Exception("Không tìm thấy kết quả.");
                }
            }
        }

        //// tìm kiếm hợp đồng bằng ClientID
        //public List<Contracts> ContractByClientId(int ClientId)
        //{
        //    var procedure = "Contract_ByClientId";
        //    var values = new { ClientId = ClientId };

        //    using (var con = new SqlConnection(_connectionString))
        //    {
        //        var results = con.Query<Contracts>(procedure, values, commandType: CommandType.StoredProcedure).ToList();
        //        return results;
        //    }
        //}
    }
}

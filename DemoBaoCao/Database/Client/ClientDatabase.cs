using DemoBaoCao.Models;
using Microsoft.Extensions.Options;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing.Printing;

namespace DemoBaoCao.Database.Client
{
    public class ClientDatabase : IClientDatabase
    {
        private readonly string _connectionString;

        public ClientDatabase()
        {
            _connectionString = "Data Source=DESKTOP-OVPEISB\\SQLEXPRESS;Initial Catalog=DemoEnd;Persist Security Info=False;User ID=sa;Password=sa;";
        }


        // Phương thức hiện danh sách khách hàng
        public List<Clients> AllClient()
        {
            try
            {
                var procedure = "Client_All";

                using (var connection = new SqlConnection(_connectionString))
                {
                    var results = connection.Query<Clients>(procedure, commandType: CommandType.StoredProcedure).ToList();
                    return results;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }



        // phương thức tìm khách hàng theo id
        public List<Clients> ClientById(int ClientId)
        {
            var procedure = "Client_ById";
            var values = new { ClientId = ClientId };

            using (var con = new SqlConnection(_connectionString))
            {
                var results = con.Query<Clients>(procedure, values, commandType: CommandType.StoredProcedure).ToList();
                return results;
            }
        }



        // phương thức tìm kiếm khách hàng theo tên, số cccd, số điện thoại
        public List<Clients> ClientBy(string ClientName, string ClientIdNumber, string ClientSMSNumber) 
        {
            var procedure = "ClientBy_NameNumberSMS";
            var values = new { ClientName = ClientName, ClientIdNumber = ClientIdNumber, ClientSMSNumber = ClientSMSNumber };

            using (var con = new SqlConnection(_connectionString))
            {
                var results = con.Query<Clients>(procedure, values, commandType: CommandType.StoredProcedure).ToList();
                return results;
            }
        }



        // phương thức thêm khách hàng
        public List<Clients> AddClient(string ClientName, string ClientAddress, string ClientIdNumber, string CientIdLssuePlace,
                                        DateTime ClientIDLssueDate, string ClientSMSNumber, string ClientEmail, string ClientBranch)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                var cmd = new SqlCommand("Client_Add", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ClientName", ClientName);
                cmd.Parameters.AddWithValue("@ClientAddress", ClientAddress);
                cmd.Parameters.AddWithValue("@ClientIdNumber", ClientIdNumber);
                cmd.Parameters.AddWithValue("@CientIdLssuePlace", CientIdLssuePlace);
                cmd.Parameters.AddWithValue("@ClientIDLssueDate", ClientIDLssueDate);
                cmd.Parameters.AddWithValue("@ClientSMSNumber", ClientSMSNumber);
                cmd.Parameters.AddWithValue("@ClientEmail", ClientEmail);
                cmd.Parameters.AddWithValue("@ClientBranch", ClientBranch);

                cmd.ExecuteNonQuery();

                con.Close();
            }
            return AllClient();
        }



        // phương thức sửa thông tin khách hàng
        public List<Clients> EditClient(int ClientId, string ClientName, string ClientAddress, string ClientIdNumber, string CientIdLssuePlace,
                                        DateTime ClientIDLssueDate, string ClientSMSNumber, string ClientEmail, string ClientBranch)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                var cmd = new SqlCommand("Client_Edit", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ClientId", ClientId);
                cmd.Parameters.AddWithValue("@ClientName", ClientName);
                cmd.Parameters.AddWithValue("@ClientAddress", ClientAddress);
                cmd.Parameters.AddWithValue("@ClientIdNumber", ClientIdNumber);
                cmd.Parameters.AddWithValue("@CientIdLssuePlace", CientIdLssuePlace);
                cmd.Parameters.AddWithValue("@ClientIDLssueDate", ClientIDLssueDate);
                cmd.Parameters.AddWithValue("@ClientSMSNumber", ClientSMSNumber);
                cmd.Parameters.AddWithValue("@ClientEmail", ClientEmail);
                cmd.Parameters.AddWithValue("@ClientBranch", ClientBranch);

                cmd.ExecuteNonQuery();

                con.Close();
            }
            return AllClient();
        }



        // phương thức xóa khách hàng
        public List<Clients> DeleteClient(int ClientId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                var cmd = new SqlCommand("Client_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ClientId", ClientId);
                cmd.ExecuteNonQuery();

                con.Close();
            }
            return AllClient();
        }

    }
}

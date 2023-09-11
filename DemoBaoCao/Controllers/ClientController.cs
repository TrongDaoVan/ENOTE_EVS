using DemoBaoCao.Database.Client;
using DemoBaoCao.Database.Contract;
using DemoBaoCao.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Drawing.Printing;

namespace DemoBaoCao.Controllers
{
    [Authorize]
    [Route("Client")]
    public class ClientController : Controller
    {
        private readonly IClientDatabase _database;
        private readonly IContractDatabase _contractdata;

        public ClientController(IClientDatabase database, IContractDatabase contractdata)
        {
            _database = database;
            _contractdata = contractdata;
        }


        // hiện danh sách khách hàng và tìm kiếm khách hàng theo tên, số điện thoại, căn cước công dân
        [Route("AllClient")]
        [HttpGet("AllClient")]
        public IActionResult AllClient(string ClientName, string ClientIdNumber, string ClientSMSNumber/*, int page = 1*/)
        {
            //int pageSize = 10; // Số bản ghi trên mỗi trang
            List<Clients> clients = new List<Clients>();

            if (string.IsNullOrEmpty(ClientName) && string.IsNullOrEmpty(ClientIdNumber) && string.IsNullOrEmpty(ClientSMSNumber))
            {
                clients = _database.AllClient();
            }
            else
            {
                clients = _database.ClientBy(ClientName, ClientIdNumber, ClientSMSNumber);
            }

            //// Tính tổng số trang
            //int totalRecords = clients.Count;
            //int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            //// Giới hạn giá trị của page trong khoảng từ 1 đến totalPages
            //page = Math.Max(1, Math.Min(page, totalPages));

            //// Gán giá trị cho ViewBag.Page và ViewBag.TotalPages
            //ViewBag.Page = page;
            //ViewBag.TotalPages = totalPages;

            //// Lấy danh sách khách hàng của trang hiện tại
            //List<Clients> clientsForPage = clients.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return View(clients);
        }



        // tìm kiếm khách hàng theo id
        [Route("ClientById")]
        [HttpGet("ClientById")]
        public IActionResult ClientById(int ClientId)
        {
            var myData = _database.ClientById(ClientId);
            var ds = myData;
            return View(ds);
        }



        // thêm khách hàng mới
        [Route("AddClient")]
        [HttpGet]
        public IActionResult AddClient()
        {
            return View(new Clients());
        }



        [Route("AddClient")]
        [HttpPost]
        public IActionResult AddClient(string ClientName, string ClientAddress, string ClientIdNumber, string CientIdLssuePlace,
                                        DateTime ClientIDLssueDate, string ClientSMSNumber, string ClientEmail, string ClientBranch)
        {
            var myData = _database.AddClient(ClientName, ClientAddress, ClientIdNumber, CientIdLssuePlace,
                                              ClientIDLssueDate, ClientSMSNumber, ClientEmail, ClientBranch);
            var ds = myData;
            return RedirectToAction("AllClient");
        }



        // sửa thông tin khách hàng
        [Route("EditClient")]
        [HttpGet]
        public IActionResult EditClient(int Id)
        {
            var myData = _database.ClientById(Id);
            var ds = myData.FirstOrDefault();
            return View(ds);
        }



        [Route("EditClient")]
        [HttpPut("EditClient")]
        public IActionResult EditClient(int ClientId, string ClientName, string ClientAddress, string ClientIdNumber, string CientIdLssuePlace,
                                        DateTime ClientIDLssueDate, string ClientSMSNumber, string ClientEmail, string ClientBranch)
        {
            var myData = _database.EditClient(ClientId, ClientName, ClientAddress, ClientIdNumber, CientIdLssuePlace,
                                              ClientIDLssueDate, ClientSMSNumber, ClientEmail, ClientBranch);
            return RedirectToAction("AllClient");
        }



        // xóa thông tin khách hàng
        [Route("DeleteClient")]
        [HttpGet]
        public ActionResult DeleteClient(int Id)
        {
            List<Clients> clients = _database.DeleteClient(Id);
            if (clients != null)
            {
                return RedirectToAction("AllClient", "Client"); 
            }
            else
            {
                ViewBag.Error = "Không thể xóa Client này!"; 
                return View(clients); 
            }
        }

        //// tìm kiếm khách hàng theo id
        //[Route("ContractByClientId")]
        //[HttpGet("ContractByClientId")]
        //public IActionResult ContractByClientId(int ClientId)
        //{
        //    var myData = _contractdata.ContractByClientId(ClientId);
        //    return View();
        //}


    }
}

//create table Clientst
//(
//ClientId int IDENTITY(1,1) primary key,
//ClientCode AS ('evs' + RIGHT('0000000' + CAST(ClientId AS VARCHAR(7)), 7)) PERSISTED,
//ClientName nvarchar(50),
//ClientAddress nvarchar(500),
//ClientIdNumber nvarchar(50),
//CientIdLssuePlace nvarchar(500),
//ClientIdLssueDate date,
//ClientSMSNumber nvarchar(50),
//ClientEmail nvarchar(50),
//ClientBranch nvarchar(50)
//);

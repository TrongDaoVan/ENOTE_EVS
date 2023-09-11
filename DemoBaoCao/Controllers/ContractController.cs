using DemoBaoCao.Database;
using DemoBaoCao.Database.Client;
using DemoBaoCao.Database.Contract;
using DemoBaoCao.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;

namespace DemoBaoCao.Controllers
{
    [Authorize]
    [Route("Contract")]
    public class ContractController : Controller
    {
        private readonly IContractDatabase _contractDatabase;
        private readonly IClientDatabase _clientDatabase;

        public ContractController(IContractDatabase contractDatabase, IClientDatabase clientDatabase)
        {
            _contractDatabase = contractDatabase;
            _clientDatabase = clientDatabase;  
        }



        // hiện danh sách hợp đồng và tìm kiếm theo sdt, tên kh, CCCD, và mã hợp đồng
        [Route("AllContract")]
        [HttpGet("AllContract")]
        public IActionResult AllContract(string Value, string ContractCode, int page = 1)
        {
            int pageSize = 10; // Số bản ghi trên mỗi trang
            List<Contracts> contracts = new List<Contracts>();

            if (string.IsNullOrEmpty(Value) && string.IsNullOrEmpty(ContractCode))
            {
                contracts = _contractDatabase.AllContract();
            }
            else
            {
                contracts = _contractDatabase.ContractByValueCode(Value, ContractCode);
            }

            // Tính tổng số trang
            int totalRecords = contracts.Count;
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Giới hạn giá trị của page trong khoảng từ 1 đến totalPages
            page = Math.Max(1, Math.Min(page, totalPages));

            // Gán giá trị cho ViewBag.Page và ViewBag.TotalPages
            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;

            // Lấy danh sách hợp đồng của trang hiện tại
            List<Contracts> contractsForPage = contracts.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return View(contractsForPage);
        }



        // tìm kiếm hợp đồng theo id
        [Route("ContractById")]
        [HttpGet("ContractById")]
        public IActionResult ContractById(int Id)
        {
            var myData = _contractDatabase.ContractById(Id);
            var ds = myData;
            return View(ds);
        }



        // thêm hợp đồng mới 
        [Route("AddContract")]
        [HttpGet]
        public IActionResult AddContract(int ClientId)
        {
            ViewData["ClientId"] = ClientId;
            return View(new Contracts());
        }



        [Route("AddContract")]
        [HttpPost]
        public IActionResult AddContract(decimal ContractValue, decimal ContractAmountWithdrawn,
                                        decimal ContractRemainingAmount, DateTime ContractBorrowedDate,
                                        int ContractNumberOfDays, DateTime ContractEffectiveDate,
                                        DateTime ContractPaymentDate, int ClientId, int ProductId)
        {
            var myData = _contractDatabase.AddContract(ContractValue, ContractAmountWithdrawn,
                                                       ContractRemainingAmount, ContractBorrowedDate,
                                                       ContractNumberOfDays, ContractEffectiveDate,
                                                       ContractPaymentDate, ClientId, ProductId);
            var ds = myData;
            return RedirectToAction("AllContract");
        }



        // sửa thông tin hợp đồng
        [Route("EditContract")]
        [HttpGet]
        public IActionResult EditContract(int Id)
        {
            var myData = _contractDatabase.ContractById(Id);
            var ds = myData.FirstOrDefault();
            return View(ds);
        }



        [Route("EditContract")]
        [HttpPut("EditContract")]
        public IActionResult EditContract(int ContractId, decimal ContractValue, decimal ContractAmountWithdrawn,
                                        decimal ContractRemainingAmount, DateTime ContractBorrowedDate,
                                        int ContractNumberOfDays, DateTime ContractEffectiveDate,
                                        DateTime ContractPaymentDate, int ClientId, int ProductId)
        {
            var myData = _contractDatabase.EditContract(ContractId, ContractValue, ContractAmountWithdrawn,
                                                        ContractRemainingAmount, ContractBorrowedDate,
                                                        ContractNumberOfDays, ContractEffectiveDate,
                                                        ContractPaymentDate, ClientId, ProductId);
            return RedirectToAction("AllContract");
        }



        // Xóa hợp đồng
        [Route("DeleteContract")]
        [HttpGet]
        public IActionResult DeleteContract(int Id)
        {
            List<Contracts> contracts = _contractDatabase.DeleteContract(Id);
            if (contracts != null)
            {
                return RedirectToAction("AllContract", "Contract");
            }
            else
            {
                ViewBag.Error = "Không thể xóa Contract này!";
                return View(contracts);
            }
        }



        // hiện danh sách chi tiết hợp đồng
        [Route("ChiTietHD")]
        [HttpGet]
        public IActionResult ChiTietHD(int Id)
        {
            var myData = _contractDatabase.ChiTietHD(Id);
            var ds = myData.FirstOrDefault();
            return View(ds);
        }



        [Route("GetTotalMoney")]
        [HttpGet]
        public IActionResult GetTotalMoney(int Id)
        {
            try
            {
                decimal totalMoney = _contractDatabase.TotalMoney(Id);
                ViewBag.TotalMoney = totalMoney; // Đặt giá trị totalMoney vào ViewBag

                return View(); // Trả về view
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }
    }
}

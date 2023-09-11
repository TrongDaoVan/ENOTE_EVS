using DemoBaoCao.Database.Client;
using DemoBaoCao.Database.Product;
using DemoBaoCao.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoBaoCao.Controllers
{
    [Authorize]
    [Route("Product")]
    public class ProductController : Controller
    {
        private readonly IProductDatabase _productDatabase;

        public ProductController(IProductDatabase productDatabase)
        {
            _productDatabase = productDatabase;
        }



        // hiện danh sách sản phẩm và tìm kiếm sản phẩm theo tên gần đúng
        [Route("AllProduct")]
        [HttpGet("AllProduct")]
        public IActionResult AllProduct(string ProductName, int page = 1)
        {
            int pageSize = 10; // Số bản ghi trên mỗi trang
            List<Products> products = new List<Products>();

            if (string.IsNullOrEmpty(ProductName))
            {
                products = _productDatabase.AllProduct();
            }
            else
            {
                products = _productDatabase.ProductByName(ProductName);
            }

            // Tính tổng số trang
            int totalRecords = products.Count;
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Giới hạn giá trị của page trong khoảng từ 1 đến totalPages
            page = Math.Max(1, Math.Min(page, totalPages));

            // Gán giá trị cho ViewBag.Page và ViewBag.TotalPages
            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;

            // Lấy danh sách sản phẩm của trang hiện tại
            List<Products> productsForPage = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return View(productsForPage);
        }



        // Thêm mới sản phẩm
        [Route("AddProduct")]
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View(new Products());
        }



        [Route("AddProduct")]
        [HttpPost]
        public IActionResult AddProduct(string ProductName, DateTime ProductEffectiveDateFrom, DateTime ProductEffectiveDateTo,
            int ProductMinimumSendingDate, int ProductMaximumSendingDate, decimal ProductPackageMinimumAmount,
            decimal ProductPackageMaximumAmount, decimal ProductTotalRemainingLimit, decimal ProductNonTermRate)
        {
            var myData = _productDatabase.AddProduct(ProductName, ProductEffectiveDateFrom, ProductEffectiveDateTo, ProductMinimumSendingDate,
                                              ProductMaximumSendingDate, ProductPackageMinimumAmount, ProductPackageMaximumAmount,
                                              ProductTotalRemainingLimit, ProductNonTermRate);
            var ds = myData;
            return RedirectToAction("AllProduct");
        }



        // Sửa thông tin sản phẩm
        [Route("EditProduct")]
        [HttpGet]
        public IActionResult EditProduct(int Id)
        {
            var myData = _productDatabase.ProductById(Id);
            var ds = myData.FirstOrDefault();
            return View(ds);
        }



        [Route("EditProduct")]
        [HttpPut("EditProduct")]
        public IActionResult EditProduct(int ProductId, string ProductName, DateTime ProductEffectiveDateFrom,
                                         DateTime ProductEffectiveDateTo, int ProductMinimumSendingDate, 
                                         int ProductMaximumSendingDate, decimal ProductPackageMinimumAmount,
            decimal ProductPackageMaximumAmount, decimal ProductTotalRemainingLimit, decimal ProductNonTermRate)
        {
            var myData = _productDatabase.EditProduct(ProductId, ProductName, ProductEffectiveDateFrom, ProductEffectiveDateTo,
                                                     ProductMinimumSendingDate, ProductMaximumSendingDate, ProductPackageMinimumAmount,
                                                     ProductPackageMaximumAmount, ProductTotalRemainingLimit, ProductNonTermRate);
            return RedirectToAction("AllProduct");
        }



        // Xóa sản phẩm 
        [Route("DeleteProduct")]
        [HttpGet]
        public IActionResult DeleteProduct (int Id)
        {
            List<Products> products = _productDatabase.DeleteProduct(Id);
            if (products != null)
            {
                return RedirectToAction("AllProduct", "Product");
            }
            else
            {
                ViewBag.Error = "Không thể xóa Product này!";
                return View(products);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data.SqlClient;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using DemoBaoCao.Models;
using Microsoft.CodeAnalysis.Scripting;


namespace DemoBaoCao.Controllers
{
    public class AccessController : Controller
    {
        private readonly string _connectionString;

        public AccessController() 
        {
            _connectionString = "Data Source=DESKTOP-OVPEISB\\SQLEXPRESS;Initial Catalog=DemoEnd;Persist Security Info=False;User ID=sa;Password=sa;";
        }
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("AllClient", "Client");
            return View();
        }

        

        // Đăng nhập
        [HttpPost]
        public async Task<IActionResult> Login(DemoLogin demoLogin)
        {
            using (var con = new SqlConnection(_connectionString)) 
            {
                con.Open();

                var query = "SELECT * FROM DemoLogin WHERE Email = @Email AND Password = @Password";
                var user = await con.QueryFirstOrDefaultAsync<DemoLogin>(query, new { Email = demoLogin.Email, Password = demoLogin.Password });

                if (user != null)
                {
                    // Tạo danh sách các claim để lưu trữ thông tin người dùng đã xác thực
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, demoLogin.Email),
                        new Claim("OtherProperties", "Example Role")
                    };

                    // Tạo đối tượng ClaimsIdentity từ danh sách claims
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    // Thiết lập các thuộc tính cho đối tượng AuthenticationProperties
                    AuthenticationProperties properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = demoLogin.KeepLoggedIn
                    };

                    // Tạo đối tượng CookieOptions để định cấu hình cho cookie
                    CookieOptions cookieOptions = new CookieOptions()
                    {
                        // Thiết lập ExpiresTime và MaxAge của cookie là null để không thiết lập thời hạn
                        Expires = null,
                        MaxAge = null
                    };

                    // Đăng nhập người dùng bằng CookieAuthentication với các thuộc tính và tùy chọn đã thiết lập
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), properties);

                    // Lưu giá trị Email vào cookie
                    Response.Cookies.Append("userEmail", demoLogin.Email, cookieOptions);

                    // Chuyển hướng đến action AllClient của controller Client
                    return RedirectToAction("AllClient", "Client");
                }

                ViewData["ValidateMessage"] = "Sai tên đăng nhập hoặc mật khẩu";
                return View();
            }
        }



        // Đổi Mật Khẩu
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> ChangePassword(string Password, string newPassword)
        {
            var userEmail = Request.Cookies["UserEmail"];
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();

                // Kiểm tra Password hiện tại của người dùng
                var checkPasswordQuery = "SELECT * FROM DemoLogin WHERE Email = @Email AND Password = @Password";
                var user = await con.QueryFirstOrDefaultAsync<DemoLogin>(checkPasswordQuery, new { Email = userEmail, Password = Password });

                if (user != null)
                {
                    // Nếu Password hiện tại đúng, thực hiện cập nhập Password mới
                    var updatePasswordQuery = "UPDATE DemoLogin SET Password = @NewPassword WHERE Email = @Email";
                    await con.ExecuteAsync(updatePasswordQuery, new { NewPassword = newPassword, Email = userEmail });

                    ViewData["Message"] = "Đổi mật khẩu thành công";
                    return View();
                }

                ViewData["ErrorMessage"] = "Sai mật khẩu hiện tại";
                return View();
            }
        }

        

        // Đổi mã Pin
        [HttpGet]
        public async Task<IActionResult> ChangePin()
        {
            return View();
        }

        

        [HttpPost]
        public async Task<IActionResult> ChangePin(string Pin, string NewPin)
        {
            var userEmail = Request.Cookies["UserEmail"];
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                var checkPinQuery = "SELECT * FROM DemoLogin WHERE Email = @Email AND Pin = @Pin";
                var user = await con.QueryFirstOrDefaultAsync<DemoLogin>(checkPinQuery, new { Email = userEmail, Pin = Pin });

                if (user != null)
                {
                    try
                    {
                        // Nếu mã Pin hiện tại đúng, thực hiện cập nhật mật khẩu mới
                        var updatePinQuery = "UPDATE DemoLogin SET Pin = @NewPin WHERE Email = @Email";
                        await con.ExecuteAsync(updatePinQuery, new { NewPin = NewPin, Email = userEmail });

                        ViewData["Message"] = "Đổi Mã Pin thành công";
                        return View();
                    }
                    catch (Exception ex)
                    {
                        ViewData["ErrorMessage2"] = "Mã pin không hợp lệ. Mã Pin hợp lệ tối đa 6 chữ số";
                        return View();
                    }
                    
                }
                ViewData["ErrorMessage"] = "Mã Pin sai";
                return View();
            }
        }



        // Đăng xuất
        public async Task<IActionResult> LogOut()
        {
            // Xóa cookie userEmail
            Response.Cookies.Delete("userEmail");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Access");
        }



        // Check Email và mã Pin
        [HttpGet]
        public async Task<IActionResult> CheckEmailAndPin()
        {
            return View();
        }

        

        [HttpPost]
        public async Task<IActionResult> CheckEmailAndPin(string Email, string Pin)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();

                // Kiểm tra xem Email và Pin có đúng không
                var query = "SELECT * FROM DemoLogin WHERE Email = @Email AND Pin = @Pin";
                var user = await con.QueryFirstOrDefaultAsync<DemoLogin>(query, new { Email = Email, Pin = Pin });

                if (user != null)
                {
                    // Nếu Email và Pin đúng, chuyển hướng đến action EditPassword và truyền Email của người dùng
                    return RedirectToAction("EditPassword", new { Email = Email });
                }
                ViewData["ErrorMessage"] = "Sai Email hoặc mã Pin";
                return View();
            }
        }
        
        

        // Đổi Password sau khi check xong Email và mã pin
        [HttpGet]
        public async Task<IActionResult> EditPassword(string Email)
        {
            ViewData["Email"] = Email;
            return View();
        }

        

        [HttpPost]
        public async Task<IActionResult> EditPassword(string Email, string NewPassword)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();

                // Cập nhật Password mới 
                var query = "UPDATE DemoLogin SET Password = @NewPassword WHERE Email = @Email";
                await con.ExecuteAsync(query, new { NewPassword = NewPassword, Email = Email });

                ViewData["Message"] = "Đổi mật khẩu thành công";
                return View();
            }
        }



        // Tạo Tài khoản mới
        [HttpGet]
        public async Task<IActionResult> NewAccount ()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> NewAccount (string Email, string Password, string  Password2,string Pin, DemoLogin model)
        {
            if (Password != Password2)
            {
                ViewData["ValidateMessage"] = "Mật khẩu không trùng khớp";
                return View();
            }
            else
            {
                    using (var con = new SqlConnection(_connectionString))
                    {
                        con.Open();

                        var query = "SELECT * FROM DemoLogin WHERE Email = @Email";
                        var result = await con.ExecuteScalarAsync<int>(query, new { Email = Email });

                        if (result > 0)
                        {
                            ViewData["ValidateMessage2"] = "Email đã tồn tại";
                            return View();
                        }
                        else
                        {
                            try
                            {
                                var query2 = "INSERT INTO DemoLogin (Email, Password, Pin) VALUES (@Email, @Password, @Pin)";
                                var result2 = await con.ExecuteAsync(query2, new { Email = Email, Password = Password, Pin = Pin });

                                ViewData["Message"] = "Tạo tài khoản thành công";
                                return View();
                            }
                            catch (Exception ex)
                            {
                                ViewData["ValidateMessage3"] = "Mã pin không hợp lệ. Mã Pin hợp lệ tối đa 6 chữ số";
                                return View();
                            }
                        }
                    }
                }
            }
        }
    }

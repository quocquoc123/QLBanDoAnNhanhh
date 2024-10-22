using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBanDoAnNhanh.Models;

namespace QLBanDoAnNhanh.Controllers
{
    public class UserController : Controller
    {
        private QlbanDoAnNhanhContext db = new QlbanDoAnNhanhContext();
        private readonly QlbanDoAnNhanhContext _context;
        public UserController(QlbanDoAnNhanhContext context)

        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        // View đăng nhập
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear(); // Xóa tất cả session
            return RedirectToAction("TrangChu", "SanPhams");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string matkhau)
        {
            if (ModelState.IsValid)
            {
                NguoiDung check = _context.NguoiDungs.FirstOrDefault(s => s.Username == username);
                if (check == null || check.Matkhau != matkhau)
                {
                    ViewBag.error = "Sai tên đăng nhập hoặc mật khẩu";
                    return View();
                }

                // Lưu thông tin người dùng vào TempData hoặc session
                HttpContext.Session.SetString("hoTen", check.HoTen);
                HttpContext.Session.SetString("email", check.Email);
                HttpContext.Session.SetString("sdt", check.Sdt);

                if (check.RoleId == 1)
                {
                    HttpContext.Session.SetString("userLogin", check.Username);
                }
                else
                {
                    HttpContext.Session.SetString("userLogin", check.Username);
                    HttpContext.Session.SetString("adminLogin", check.Username);
                    return RedirectToAction("sanpham", "Admin");
                }

                return RedirectToAction("TrangChu", "SanPhams");
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(NguoiDung user)
        {
            user.TrangThai = "Chưa mua hàng";
            //user.Role = 0;
            if (user == null)
            {
                ViewBag.error = "Tài khoản đã tồn tại";
            }


            if (_context.NguoiDungs.FirstOrDefault(s => s.Username == user.Username) == null)
            {
                _context.NguoiDungs.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.error = "Tài khoản đã tồn tại";
                return View();
            }

            return View();
        }

        // Quên mật khẩu
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult ForgotPassword(string email)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = _context.NguoiDungs.FirstOrDefault(u => u.Email == email);
        //        if (user != null)
        //        {
        //            // Tạo mã khôi phục
        //            string recoveryCode = Guid.NewGuid().ToString(); // Tạo mã khôi phục

        //            // Gửi mã khôi phục qua email
        //            string subject = "Khôi phục mật khẩu";
        //            string content = $"Mã khôi phục của bạn là: {recoveryCode}";

        //            if (Common.Common.SendMail(user.hoTen, subject, content, user.email))
        //            {
        //                HttpContext.Session.SetString("RecoveryCode", recoveryCode); // Lưu mã khôi phục vào session
        //                HttpContext.Session.SetString("Email", user.email); // Lưu email để khôi phục sau
        //                ViewBag.Message = "Mã khôi phục đã được gửi tới email của bạn.";
        //                return RedirectToAction("VerifyRecoveryCode");
        //            }
        //            else
        //            {
        //                ViewBag.Error = "Có lỗi xảy ra trong việc gửi email.";
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.Error = "Email không tồn tại.";
        //        }
        //    }
        //    return View();
        //}

        // Xác nhận mã khôi phục
        public IActionResult VerifyRecoveryCode()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerifyRecoveryCode(string recoveryCode)
        {
            if (ModelState.IsValid)
            {
                var sessionRecoveryCode = HttpContext.Session.GetString("RecoveryCode");
                var email = HttpContext.Session.GetString("Email");

                if (sessionRecoveryCode == recoveryCode)
                {
                    // Mã khôi phục hợp lệ, chuyển sang trang đặt lại mật khẩu
                    return RedirectToAction("ResetPassword", new { email = email });
                }
                else
                {
                    ViewBag.Error = "Mã khôi phục không hợp lệ.";
                }
            }
            return View();
        }

        // Đặt lại mật khẩu
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(string newPassword, string confirmPassword)
        {
            var email = HttpContext.Session.GetString("Email");
            var user = _context.NguoiDungs.FirstOrDefault(u => u.Email == email);

            if (user != null && newPassword == confirmPassword)
            {
                // Cập nhật mật khẩu mới
                user.Matkhau = newPassword;
                _context.Update(user);
                _context.SaveChanges();

                // Xóa session RecoveryCode và Email
                HttpContext.Session.Remove("RecoveryCode");
                HttpContext.Session.Remove("Email");

                ViewBag.Message = "Mật khẩu đã được đặt lại thành công.";
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Error = "Mật khẩu không khớp hoặc có lỗi xảy ra.";
            }

            return View();
        }
        //public IActionResult Profile()
        //{
        //    if (HttpContext.Session.GetString("userLogin") == null)
        //    {
        //        return RedirectToAction("Login", "User");
        //    }

        //    string username = HttpContext.Session.GetString("userLogin");
        //    var user = _context.NguoiDungs.FirstOrDefault(u => u.Username == username);

        //    if (user == null)
        //    {
        //        ViewBag.Error = "Không tìm thấy người dùng.";
        //        return View();
        //    }

        //    return View(user);
        //}

        //// Phương thức POST: Cập nhật thông tin người dùng không có ràng buộc
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Profile(NguoiDung model)
        //{
        //    if (HttpContext.Session.GetString("userLogin") == null)
        //    {
        //        return RedirectToAction("Login", "User");
        //    }

        //    string username = HttpContext.Session.GetString("userLogin");
        //    var user = _context.NguoiDungs.FirstOrDefault(u => u.Username == username);

        //    if (user != null)
        //    {
        //        // Cập nhật thông tin người dùng
        //        user.HoTen = model.HoTen;
        //        user.Email = model.Email;
        //        user.Sdt = model.Sdt;

        //        // Lưu vào database
        //        _context.Entry(user).State = EntityState.Modified;
        //        _context.SaveChanges();

        //        ViewBag.Message = "Cập nhật thông tin thành công!";
        //    }
        //    else
        //    {
        //        ViewBag.Error = "Không tìm thấy người dùng.";
        //    }

        //    return View(model);
        //}

    }
}


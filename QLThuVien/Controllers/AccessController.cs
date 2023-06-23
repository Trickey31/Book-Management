using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using QLThuVien.Models;
using System.Linq;
using QLThuVien.Models.ThongTinTaiKhoanViewModels;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace QLThuVien.Controllers
{
    public class AccessController : Controller
    {
        private readonly QlthuVienLtwebContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccessController(QlthuVienLtwebContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            TempData["Error"] = "";
            if (ModelState.IsValid)
            {
                var u = _db.Users.FirstOrDefault(x => x.Username.Equals(user.Username) && x.Password.Equals(user.Password));
                if (u != null && u.LoaiUser == null)
                {
                    HttpContext.Session.SetString("Username", u.Username.ToString());
                    var nv = _db.NguoiDocs.FirstOrDefault(x => x.Username == u.Username);
                    HttpContext.Session.SetString("AnhDaiDien", nv.AnhDaiDien.ToString());
                    return RedirectToAction("Index", "Home");
                }
                else if (u != null && u.LoaiUser != null)
                {
                    HttpContext.Session.SetString("Username", u.Username.ToString());
                    HttpContext.Session.SetString("Email", u.EmailDk.ToString());
                    HttpContext.Session.SetString("LoaiUser", u.LoaiUser.ToString());
                    var nv = _db.NhanViens.FirstOrDefault(x => x.Username == u.Username);
                    HttpContext.Session.SetString("AnhDaiDien", nv.AnhDaiDien.ToString());

                    return RedirectToAction("HomeAdmin", "Admin");
                }
                else
                {
                    TempData["Error"] = "Sai tài khoản hoặc mật khẩu!";
                    return View(user); // Trả về view "Login" với thông báo lỗi
                }
            }
            return View(user);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Login", "Access");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Register(ThongTinTaiKhoanViewModel tt)
        {
            TempData["Error"] = "";
            var nguoiDoc = new NguoiDoc();
            var user = new User();
            var checkUser = _db.Users.Where(x => x.Username.Equals(tt.User.Username)).FirstOrDefault();
            var checkEmail = _db.Users.Where(x => x.EmailDk.Equals(tt.User.EmailDk)).FirstOrDefault();
            if (checkUser == null && checkEmail == null)
            {
                nguoiDoc.HoTen = tt.NguoiDoc.HoTen;
                nguoiDoc.MaNguoiDoc = tt.User.Username.ToString() + "123";
                nguoiDoc.Username = tt.User.Username;
                nguoiDoc.AnhDaiDien = "avt-trang.png";

                user.Username = tt.User.Username;
                user.Password = tt.User.Password;
                user.EmailDk = tt.User.EmailDk;

                _db.Users.Add(user);
                _db.NguoiDocs.Add(nguoiDoc);

                _db.SaveChanges();
                TempData["Error"] = "Đăng ký thành công";
                return RedirectToAction("Login", "Access");
            } else
            {
                TempData["Error"] = "Tên tài khoản hoặc Email đã tồn tại";
            }
            return View(tt);
        }
    }
}

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
using System.Security.Claims;
using NuGet.Protocol.Plugins;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.Facebook;

namespace QLThuVien.Controllers
{
    public class AccountController : Controller
    {
        QlthuVienLtwebContext db = new QlthuVienLtwebContext();


        // ============== Google =================
        [HttpPost]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("CallbackLoginGoogle"),
                Items =
                {
                    { "scheme", GoogleDefaults.AuthenticationScheme  }
                }
            };

            return Challenge(properties, "Google");
        }


        [HttpGet]
        public async Task<IActionResult> CallbackLoginGoogle()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            // chưa có check đăng nhập lỗi 
            var accessToken = result.Properties.GetTokenValue("access_token");
            var UserId = result.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var UserName = result.Principal.FindFirstValue(ClaimTypes.Name);
            var UserEmail = result.Principal.FindFirstValue(ClaimTypes.Email);

            // mặc định ko có cái picture nhưng mình đã config thêm nó vào trong program.cs
            var Picture = result.Principal.FindFirstValue("Picture");

            var userExists = db.Users.SingleOrDefault(x => x.EmailDk == UserEmail);

            if (userExists != null)
            {
                // email này đã dki tài khoản rồi thì cho đăng nhập luôn 
                HttpContext.Session.SetString("Username", userExists.Username);
                CurrentUser.initSession(userExists.Username);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                const string defaultPassword = "default123";

                // tạo user mới và add vào db 
                var newUser = new User
                {
                    EmailDk = UserEmail,
                    Username = UserName,
                    Password = defaultPassword,
                };
                var nd = new NguoiDoc
                {
                    MaNguoiDoc = Guid.NewGuid().ToString().Substring(0, 8),
                    HoTen = UserName,
                    Username = UserName,
                    AnhDaiDien = "avt-trang.png"
                };

                db.Users.Add(newUser);
                db.NguoiDocs.Add(nd);
                db.SaveChanges();

                // đăng nhập luôn 
                HttpContext.Session.SetString("Username", newUser.Username);
                CurrentUser.initSession(newUser.Username);
                return RedirectToAction("Index", "Home");

            }
        }

        // ================== Facebook ==================
        [HttpPost]
        [AllowAnonymous]
        public IActionResult FacebookLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("CallbackLoginFb"),
                Items =
                {
                    { "scheme",  FacebookDefaults.AuthenticationScheme }
                }
            };

            return Challenge(properties, "Facebook");
        }

        [HttpGet]
        public async Task<IActionResult> CallbackLoginFb()
        {
            var result = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
            var accessToken = result.Properties.GetTokenValue("access_token");
            var fbUserId = result.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var fbUserName = result.Principal.FindFirstValue(ClaimTypes.Name);
            var fbUserEmail = result.Principal.FindFirstValue(ClaimTypes.Email);

            var userExists = db.Users.SingleOrDefault(x => x.EmailDk == fbUserEmail);

            if (userExists != null)
            {
                // email này đã dki tài khoản rồi thì cho đăng nhập luôn 
                HttpContext.Session.SetString("Username", userExists.Username);
                CurrentUser.initSession(userExists.Username);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // tạo user mới và add vào db 
                const string defaultPassword = "default123";
                var newUser = new User
                {
                    EmailDk = fbUserEmail,
                    Username = fbUserName,
                    Password = defaultPassword
                };
                var nd = new NguoiDoc
                {
                    MaNguoiDoc = Guid.NewGuid().ToString().Substring(0, 8),
                    HoTen = fbUserName,
                    Username = fbUserName,
                    AnhDaiDien = "avt-trang.png"
                };
                db.Users.Add(newUser);
                db.NguoiDocs.Add(nd);
                db.SaveChanges();

                // đăng nhập luôn 
                HttpContext.Session.SetString("Username", newUser.Username);
                CurrentUser.initSession(newUser.Username);
                return RedirectToAction("Index", "Home");

            }
        }

        public IActionResult Logout()
        {
            // xử lý action logout sau đó chuyển về view login 
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Index", "Home");
        }
    }

    
}

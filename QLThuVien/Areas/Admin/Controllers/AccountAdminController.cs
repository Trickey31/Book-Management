using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QLThuVien.Models;
using QLThuVien.Models.DoiMatKhauViewModels;
using QLThuVien.Models.ThongTinTaiKhoanViewModels;
using System.Data.Entity;
using static QLThuVien.Models.ThongTinTaiKhoanViewModels.ThongTinTaiKhoanViewModel;

namespace QLThuVien.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/AccountAdmin")]
    public class AccountAdminController : Controller
    {
        QlthuVienLtwebContext db = new QlthuVienLtwebContext();
        [Route("")]
        [Route("ThongTinTaiKhoan")]
        public IActionResult ThongTinTaiKhoan()
        {
            var u_admin = HttpContext.Session.GetString("Username");
            var nv = db.NhanViens.FirstOrDefault(u => u.Username == u_admin);
            var user = db.Users.FirstOrDefault(u => u.Username == u_admin);

            ViewBag.AnhDaiDien = nv.AnhDaiDien;

            var viewModel = new ThongTinTaiKhoanViewModel
            {
                NhanVien = nv,
                User = user
            };
            return View(viewModel);
        }
        [Route("EditProfile")]
        [HttpGet]
        public IActionResult EditProfile()
        {
            var u_admin = HttpContext.Session.GetString("Username");
            var nv = db.NhanViens.FirstOrDefault(u => u.Username == u_admin);
            var user = db.Users.FirstOrDefault(u => u.Username == u_admin);

            var viewModel = new ThongTinTaiKhoanViewModel
            {
                NhanVien = nv,
                User = user
            };
            return View(viewModel);
        }

        [Route("EditProfile")]
        [HttpPost]
        public IActionResult EditProfile(ThongTinTaiKhoanViewModel thongTin)
        {
            var u = HttpContext.Session.GetString("Username");
            var nv = db.NhanViens.FirstOrDefault(x => x.Username == u); // Lấy thông tin nhân viên từ CSDL
            var user = db.Users.FirstOrDefault(x => x.Username == u); // Lấy thông tin người dùng từ CSDL

            // Cập nhật thông tin từ form vào đối tượng nhân viên
            nv.TenNhanVien = thongTin.NhanVien.TenNhanVien;
            nv.DiaChi = thongTin.NhanVien.DiaChi;
            nv.TenNhanVien = thongTin.NhanVien.TenNhanVien;
            nv.DiaChi = thongTin.NhanVien.DiaChi;
            nv.GioiTinh = thongTin.NhanVien.GioiTinh;
            nv.Que = thongTin.NhanVien.Que;
            nv.Sdt = thongTin.NhanVien.Sdt;
            nv.CaLam = thongTin.NhanVien.CaLam;
            nv.Description = thongTin.NhanVien.Description;

            // Cập nhật thông tin từ form vào đối tượng người dùng
            user.EmailDk = thongTin.User.EmailDk;
            //user.LoaiUser = thongTin.User.LoaiUser;
            // Lưu các thay đổi vào CSDL
            db.SaveChanges();

            return RedirectToAction("ThongTinTaiKhoan");
        }

        [Route("DoiMatKhau")]
        [HttpGet]
        public IActionResult DoiMatKhau()
        {
            var viewmodel = new DoiMatKhauViewModel();
            return View(viewmodel);
        }


        [Route("DoiMatKhau")]
        [HttpPost]
        public IActionResult DoiMatKhau(DoiMatKhauViewModel doiMatKhau)
        {
            TempData["ErrorPass"] = "";

            var currentUser = HttpContext.Session.GetString("Username");
            var user = db.Users.Where(x => x.Username == currentUser).FirstOrDefault();

            if (user != null && user.Password == doiMatKhau.MatKhauCu)
            {
                if (doiMatKhau.MatKhauMoi == doiMatKhau.XacNhanMatKhauMoi)
                {
                    user.Password = doiMatKhau.MatKhauMoi;

                    db.SaveChanges();
                    return RedirectToAction("ThongTinTaiKhoan", "AccountAdmin");
                }
                else
                {
                    TempData["ErrorPass"] = "Xác nhận mật khẩu thất bại!";
                }
            }
            else
            {
                TempData["ErrorPass"] = "Mật khẩu cũ không đúng!";
            }
            return View(doiMatKhau);
        }

        [Route("ThemMoiTaiKhoan")]
        [HttpGet]
        public IActionResult AddAccount()
        {
            return View();
        }

        [Route("ThemMoiTaiKhoan")]
        [HttpPost]
        public IActionResult AddAccount(ThongTinTaiKhoanViewModel tt)
        {
            TempData["Message"] = "";
            TempData["Error"] = "";

            var nv = new NhanVien();
            var user = new User();
            var checkUser = db.Users.Where(x => x.Username.Equals(tt.User.Username)).FirstOrDefault();
            var checkEmail = db.Users.Where(x => x.EmailDk.Equals(tt.User.EmailDk)).FirstOrDefault();
            if (checkUser == null && checkEmail == null)
            {
                nv.TenNhanVien = tt.NhanVien.TenNhanVien;
                nv.MaNhanVien = tt.User.Username.ToString() + "123";
                nv.Username = tt.User.Username;
                nv.AnhDaiDien = "avt-trang.png";

                user.Username = tt.User.Username;
                user.Password = tt.User.Password;
                user.EmailDk = tt.User.EmailDk;
                user.LoaiUser = "1";

                db.Users.Add(user);
                db.NhanViens.Add(nv);

                db.SaveChanges();
                return RedirectToAction("DanhSachNhanVien", "HomeAdmin");
            }

            return View(tt);
        }

        [Route("XoaCauThu")]
        [HttpGet]
        public IActionResult XoaNhanVien(string maNhanVien)
        {
            TempData["Message"] = "";
            var nv = db.NhanViens.FirstOrDefault(x => x.MaNhanVien == maNhanVien);
            if (nv == null)
            {
                TempData["Message"] = "Không thể xóa!";
                return RedirectToAction("DanhSachNhanVien", "HomeAdmin");
            }

            var user = db.Users.FirstOrDefault(x => x.Username == nv.Username);
            if (user != null)
            {
                db.Remove(user);
            }
            db.Remove(db.NhanViens.Find(maNhanVien));
            db.SaveChanges();
            TempData["Message"] = "Xóa thành công!";
            return RedirectToAction("DanhSachNhanVien", "HomeAdmin");
        }
    }
}

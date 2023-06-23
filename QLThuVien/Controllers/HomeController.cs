using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLThuVien.Models;
using QLThuVien.Models.DoiMatKhauViewModels;
using QLThuVien.Models.ThongTinTaiKhoanViewModels;
using QLThuVien.Models.Authentication;
using System.Diagnostics;
using X.PagedList;
//using System.Data.Entity;
using QLThuVien.Models.ProfileAndLichSuViewModels;
using QLThuVien.Models.GioHangViewModels;

namespace QLThuVien.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        QlthuVienLtwebContext db = new QlthuVienLtwebContext();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult HienSanPham(int? page)
        {
            int pagenumber = page == null || page < 1 ? 1 : page.Value;
            int pageSize = 8;

            var lstsanpham = db.Saches.OrderBy(x => x.TenSach);
            PagedList<Sach> lst = new PagedList<Sach>(lstsanpham, pagenumber, pageSize);

			return View(lst);
		}

        public IActionResult SachTheoLoai(string maLoai)
        {
          List<Sach> lstSach = db.Saches.Where(x=>x.MaTheLoai==maLoai).OrderBy(x=>x.TenSach).ToList();
            return View(lstSach);
        }

        public IActionResult ChiTietSach(string maSach, string err)
        {
            var sach = db.Saches.SingleOrDefault(x=>x.MaSach==maSach);
            ViewBag.anhSach = sach.TenFileAnhDd;
            ViewBag.err = err;
            return View(sach);
        }


        public IActionResult Index(int? page)
        {
            var Newest = db.Saches.Take(10).ToList();
            ViewBag.Newest = Newest;

            var Trending = db.Saches.Take(10).ToList();
            ViewBag.Trending = Trending;

            //var lst = db.Saches.Take(8).ToList();
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var listSach = db.Saches.AsNoTracking().OrderBy(x => x.TenSach).ToList();
            PagedList<Sach> lst = new PagedList<Sach>(listSach, pageNumber, pageSize);
            return View(lst);
		}

        public IActionResult Privacy()
        {
            var u_admin = HttpContext.Session.GetString("Username");
            var user = db.Users.FirstOrDefault(u => u.Username == u_admin);
            var nd = db.NguoiDocs.FirstOrDefault(u => u.Username == u_admin);

            ViewBag.AnhDaiDien = nd.AnhDaiDien;

            var gioHang = (from sach in db.Saches
                           join banSao in db.BanSaoSaches on sach.MaSach equals banSao.MaSach
                           join phieuMuon in db.PhieuMuons on banSao.MaBanSao equals phieuMuon.MaBanSao
                           join chiTietPm in db.ChiTietPms on phieuMuon.MaPhieuMuon equals chiTietPm.MaPhieuMuon
                           where phieuMuon.MaNguoiDoc == nd.MaNguoiDoc
                           select new GioHangViewModel
                           {
                               tenSach = sach.TenSach,
                               maPhieuMuon = phieuMuon.MaPhieuMuon,
                               ngayMuon = (DateTime)chiTietPm.NgayMuon,
                               ngayHenTra = (DateTime)chiTietPm.NgayHenTra,
                               daMuon = banSao.DaMuon
                           }).ToList();

            var viewModel = new ProfileAndLichSuViewModel
            {
                ThongTinTaiKhoan = new ThongTinTaiKhoanViewModel
                {
                    User = user,
                    NguoiDoc = nd
                },
                LichSuMuon = gioHang
            };

            return View(viewModel);
        }


        [HttpGet]
        public IActionResult EditProfile()
        {
            var a = HttpContext.Session.GetString("Username");
            var user = db.Users.FirstOrDefault(x => x.Username == a);
            var nd = db.NguoiDocs.FirstOrDefault(x => x.Username == a);

            var s = new ThongTinTaiKhoanViewModel
            {
                User = user,
                NguoiDoc = nd,
            };
            return View(s);
        }

        [HttpPost]
        public IActionResult EditProfile(ThongTinTaiKhoanViewModel thongTin)
        {
            var a = HttpContext.Session.GetString("Username");
            var user = db.Users.FirstOrDefault(x => x.Username == a);
            var nd = db.NguoiDocs.FirstOrDefault(x => x.Username == a);

            // Cập nhật thông tin từ form vào đối tượng người đọc
            nd.HoTen = thongTin.NguoiDoc.HoTen;
            nd.DiaChi = thongTin.NguoiDoc.DiaChi;
            nd.Sdt = thongTin.NguoiDoc.Sdt;

            // Cập nhật thông tin từ form vào đối tượng người dùng
            user.EmailDk = thongTin.User.EmailDk;
            user.LoaiUser = thongTin.User.LoaiUser;

            // Lưu các thay đổi vào CSDL
            db.SaveChanges();

            return RedirectToAction("Privacy");
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
                    return RedirectToAction("Privacy", "Home");
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

        public List<BanSaoSach> banSaoSach(string maBanSao)
        {
            var _maBanSao = db.BanSaoSaches.Where(x => x.MaBanSao == maBanSao).ToList();
            return _maBanSao;
        }

        [Authentication]
        [HttpPost]
        public IActionResult MuonSach(Sach sach)
        {
            TempData["Err"] = "";
            Random random = new Random();
            var user = HttpContext.Session.GetString("Username");
            var _sach = db.Saches.FirstOrDefault(x => x.MaSach == sach.MaSach);
            var mbssachList = db.BanSaoSaches.Where(x => x.MaSach == sach.MaSach).Select(x => x.MaBanSao).ToList();
            var nd = db.NguoiDocs.FirstOrDefault(x => x.Username == user);
            if (_sach != null)
            {
                var phieuMuon = new PhieuMuon();
                var CTPhieuMuon = new ChiTietPm();

                for (int i = 0; i < mbssachList.Count;)
                {
                    var bssach = db.BanSaoSaches.FirstOrDefault(x => x.MaBanSao == mbssachList[i]);
                    if (bssach.DaMuon == "1")
                    {
                        i++;
                    }
                    else
                    {
                        bssach.DaMuon = "1";

                        phieuMuon.MaPhieuMuon = Guid.NewGuid().ToString().Substring(0, 8);
                        phieuMuon.MaBanSao = mbssachList[i];
                        phieuMuon.MaNguoiDoc = nd.MaNguoiDoc;

                        int randomDays = random.Next(0, 5);
                        CTPhieuMuon.MaPhieuMuon = phieuMuon.MaPhieuMuon;
                        var ngayMuon = DateTime.Now.AddDays(randomDays);
                        CTPhieuMuon.NgayMuon = ngayMuon;
                        CTPhieuMuon.ThoiGianCho = randomDays;
                        int tgMuon = randomDays + 7;
                        CTPhieuMuon.NgayHenTra = ngayMuon.AddDays(tgMuon);
                        CTPhieuMuon.TinhTrang = "Chưa trả";

                        TempData["Err"] = "Muon Sach Thanh Cong";

                        db.PhieuMuons.Add(phieuMuon);
                        db.ChiTietPms.Add(CTPhieuMuon);
                        db.SaveChanges();

                        return RedirectToAction("ChiTietSach", new { masach = _sach.MaSach });
                    }
                    if (i == mbssachList.Count)
                    {
                        TempData["Err"] = "Het Sach";
                        return RedirectToAction("ChiTietSach", new { masach = _sach.MaSach });
                    }
                }
            }
            return RedirectToAction("ChiTietSach", new { masach = _sach.MaSach });
        }

        //[Authentication]
        //[HttpPost]
        //public IActionResult MuonSach(Sach sach)
        //{
        //    TempData["Err"] = "";
        //    Random random = new Random();
        //    var user = HttpContext.Session.GetString("Username");
        //    var _sach = db.Saches.FirstOrDefault(x => x.MaSach == sach.MaSach);
        //    var bssach = db.BanSaoSaches.FirstOrDefault(x => x.MaSach == sach.MaSach);
        //    var mbssachList = db.BanSaoSaches.Where(x => x.MaSach == sach.MaSach).Select(x => x.MaBanSao).ToList();
        //    var nd = db.NguoiDocs.FirstOrDefault(x => x.Username == user);
        //    if (_sach != null && bssach !=null)
        //    {
        //        var phieuMuon = new PhieuMuon();
        //        var CTPhieuMuon = new ChiTietPm();

        //        phieuMuon.MaPhieuMuon = bssach.MaBanSao.ToString() + "123";
        //        phieuMuon.MaBanSao = bssach.MaBanSao;
        //        phieuMuon.MaNguoiDoc = nd.MaNguoiDoc;

        //        int randomDays = random.Next(0, 5);
        //        CTPhieuMuon.MaPhieuMuon = bssach.MaBanSao.ToString() + "123";
        //        var ngayMuon = DateTime.Now.AddDays(randomDays);
        //        CTPhieuMuon.NgayMuon = ngayMuon;
        //        CTPhieuMuon.ThoiGianCho = randomDays;
        //        int tgMuon = randomDays + 7;
        //        CTPhieuMuon.NgayHenTra = ngayMuon.AddDays(tgMuon);

        //        TempData["Err"] = "Muon Sach Thanh Cong";

        //        db.PhieuMuons.Add(phieuMuon);
        //        db.ChiTietPms.Add(CTPhieuMuon);
        //        db.BanSaoSaches.Remove(bssach);
        //        db.SaveChanges();

        //        return RedirectToAction("ChiTietSach", new { masach = _sach.MaSach });

        //    } else
        //    {
        //        TempData["Err"] = "Het Sach";
        //        return RedirectToAction("ChiTietSach", new { masach = _sach.MaSach });
        //    }
        //    return RedirectToAction("ChiTietSach", new { masach = _sach.MaSach });
        //}


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

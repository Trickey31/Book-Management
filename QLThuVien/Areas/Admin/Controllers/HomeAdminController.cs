using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLThuVien.Models;
using QLThuVien.Models.Authentication;
using QLThuVien.Models.GioHangViewModels;
using QLThuVien.Models.ProfileAndLichSuViewModels;
using X.PagedList;

namespace QLThuVien.Areas.Admin.Controllers
{
    [Authentication]
    [Area("Admin")]
    [Route("admin")]
    [Route("admin/homeadmin")]
    public class HomeAdminController : Controller
    {
        QlthuVienLtwebContext db = new QlthuVienLtwebContext();

        [Route("")]
/*        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }*/
        [Route("danhmucsach")]
        public IActionResult DanhMucSach(int? page)
        {
            int pageNumber = page == null || page < 1 ? 1 : page.Value;
            int pageSize = 12;

            var lstsanpham = db.Saches.AsNoTracking().Include(s => s.MaTheLoaiNavigation).OrderBy(s => s.TenSach);


            PagedList<Sach> lst = new PagedList<Sach>(lstsanpham, pageNumber, pageSize);
            return View(lst);
        }
        [Route("timkiem")]
        [HttpPost]
        public IActionResult TimKiemSach(string tenSach)
        {
            var lstsanpham = db.Saches.AsNoTracking()
                                     .Include(s => s.MaTheLoaiNavigation)
                                     .Where(s => s.TenSach.Contains(tenSach))
                                     .OrderBy(s => s.TenSach)
                                     .ToList();
            ViewBag.TenCanTim = tenSach;
            return View(lstsanpham);
        }

        [Route("SuaSanPham")]
        [HttpGet]
        public IActionResult SuaSanPham(string maSach)
        {
            ViewBag.MaNxb = new SelectList(db.NhaXbs.ToList(), "MaNxb", "TenNxb");
            ViewBag.MaTheLoai = new SelectList(db.TheLoais.ToList(), "MaTheLoai", "TenTheLoai");
            ViewBag.MaNgonNgu = new SelectList(db.NgonNgus.ToList(), "MaNgonNgu", "TenNgonNgu");
            var sach = db.Saches.Find(maSach);
            return View(sach);
        }

        [Route("SuaSanPham")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaSanPham(Sach sach)
        {
            TempData["Message"] = "";
            if (ModelState.IsValid)
            {
                TempData["Message"] = "Sửa thông tin thành công";
                db.Entry(sach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DanhMucSach", "HomeAdmin");
            }
            TempData["Message"] = "Sửa thông tin thất bại";
            return RedirectToAction("DanhMucSach", "HomeAdmin");

        }

        [Route("ThemMoiSach")]
        [HttpGet]
        public IActionResult ThemMoiSach()
        {
            ViewBag.MaNxb = new SelectList(db.NhaXbs.ToList(), "MaNxb", "TenNxb");
            ViewBag.MaTheLoai = new SelectList(db.TheLoais.ToList(), "MaTheLoai", "TenTheLoai");
            ViewBag.MaNgonNgu = new SelectList(db.NgonNgus.ToList(), "MaNgonNgu", "TenNgonNgu");
            return View();
        }

        [Route("ThemMoiSach")]
        [HttpPost]
        public IActionResult ThemMoiSach(Sach newSach)
        {
            TempData["Message"] = "";
            TempData["Error"] = "";
            var masach = db.Saches.Where(x => x.MaSach == newSach.MaSach);
            if(masach.Any())
            {
                TempData["Error"] = "Đã tồn tại mã sách này.Vui lòng ghi mã sách khác.";
                return View(newSach);
            }
            db.Saches.Add(newSach);
            db.SaveChanges();
            TempData["Message"] = "Thêm mới thành công";
            return RedirectToAction("DanhMucSach");

        }

        //[Route("XoaSach")]
        //[HttpGet]
        //public IActionResult XoaSach(string maSach)
        //{
        //    TempData["Message"] = "";
        //    var chiTietHSM = db.ChiTietPms.Where(x => x.MaSach == maSach).ToList();
        //    var chiTietHST = db.ChiTietPts.Where(x => x.MaSach == maSach).ToList();
        //    if (chiTietHSM.Count() > 0 && chiTietHST.Count() > 0)
        //    {
        //        TempData["Message"] = "Không thể xóa!";
        //        return RedirectToAction("DanhMucSach", "HomeAdmin");
        //    }
        //    //var anhSach = db.AnhDdsaches.Where(x => x.MaSach == maSach);
        //    //if (anhSach.Any())
        //    //{
        //    //    db.Remove(anhSach);
        //    //}
        //    db.Remove(db.Saches.Find(maSach));
        //    db.SaveChanges();
        //    TempData["Message"] = "Xóa thành công!";
        //    return RedirectToAction("DanhMucSach", "HomeAdmin");
        //}

        [Route("DanhSachNhanVien")]
        [HttpGet]
        public IActionResult DanhSachNhanVien()
        {
            var dsnv = db.NhanViens.AsNoTracking().Include(x=>x.UsernameNavigation).OrderBy(x => x.TenNhanVien).ToList();
            return View(dsnv);
        }

        [Route("DanhSachPhieuMuon")]
        public IActionResult DanhSachPhieuMuon(int? page)
        {
            var PhieuMuon = (from sach in db.Saches
                           join banSao in db.BanSaoSaches on sach.MaSach equals banSao.MaSach
                           join phieuMuon in db.PhieuMuons on banSao.MaBanSao equals phieuMuon.MaBanSao
                           join chiTietPm in db.ChiTietPms on phieuMuon.MaPhieuMuon equals chiTietPm.MaPhieuMuon
                           join nguoiDoc in db.NguoiDocs on phieuMuon.MaNguoiDoc equals nguoiDoc.MaNguoiDoc
                           select new GioHangViewModel
                           {
                               tenSach = sach.TenSach,
                               maPhieuMuon = phieuMuon.MaPhieuMuon,
                               ngayMuon = (DateTime)chiTietPm.NgayMuon,
                               ngayHenTra = (DateTime)chiTietPm.NgayHenTra,
                               tinhTrang = chiTietPm.TinhTrang,
                               nguoiMuon = nguoiDoc.MaNguoiDoc
                           }).OrderByDescending(x => x.ngayMuon).ToList();

            var viewModel = new ProfileAndLichSuViewModel
            {
                LichSuMuon = PhieuMuon
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult TraSach(string maPhieuMuon)
        {
            var user = HttpContext.Session.GetString("Username");
            var nv = db.NhanViens.FirstOrDefault(x => x.Username == user);
            var phieuMuon = db.PhieuMuons.FirstOrDefault(x => x.MaPhieuMuon == maPhieuMuon);
            var ctPhieuMuon = db.ChiTietPms.FirstOrDefault(x => x.MaPhieuMuon == phieuMuon.MaPhieuMuon);

            // Cập nhật bản ghi tương ứng trong bảng BanSaoSach
            var banSaoSach = db.BanSaoSaches.FirstOrDefault(x => x.MaBanSao == phieuMuon.MaBanSao);
            banSaoSach.DaMuon = "0";

            // Cập nhật thuộc tính TinhTrang trong bảng ChiTietPM
            ctPhieuMuon.TinhTrang = "Đã Trả";

            // Tính số ngày trễ trả sách

            //var ngayHenTra = ctPhieuMuon.NgayHenTra;
            //var ngayTra = DateTime.Now;
            //var soNgayTra = ngayHenTra.Da;

            // Cập nhật thuộc tính NhanXet trong bảng ChiTietPT
            var chiTietPT = new ChiTietPt
            {
                MaPhieuTra = Guid.NewGuid().ToString().Substring(0, 8),
                NgayTra = DateTime.Now,
                //NhanXet = soNgayTra > 0 ? "Trả muộn (" + soNgayTra + " ngày)" : ""
                NhanXet = ""
            };
            db.ChiTietPts.Add(chiTietPT);

            // Thêm mới bản ghi vào bảng PhieuTra
            var phieuTra = new PhieuTra
            {
                MaPhieuTra = chiTietPT.MaPhieuTra,
                MaNhanVien = nv.MaNhanVien,
                MaNguoiDoc = phieuMuon.MaNguoiDoc,
                MaBanSao = phieuMuon.MaBanSao,
            };
            db.PhieuTras.Add(phieuTra);

            db.SaveChanges();

            return RedirectToAction("DanhSachPhieuMuon", "HomeAdmin");
        }
    }
}

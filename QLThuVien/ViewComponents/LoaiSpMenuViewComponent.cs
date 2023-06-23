using QLThuVien.Models;
using Microsoft.AspNetCore.Mvc;
using QLThuVien.Repository;

namespace QLThuVien.ViewComponents
{
	public class LoaiSpMenuViewComponent: ViewComponent
	{
		private readonly ILoaiSpRepository _loaiSpRepository;
		public LoaiSpMenuViewComponent(ILoaiSpRepository loaiSpRepository)
		{
			_loaiSpRepository = loaiSpRepository;

		}
		public IViewComponentResult Invoke()
		{
			var loaiSp = _loaiSpRepository.GetAllLoai().ToList().DistinctBy(x => x.MaTheLoai);
			return View(loaiSp);

		}
	}
}

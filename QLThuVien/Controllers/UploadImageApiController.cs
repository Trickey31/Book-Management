using Microsoft.AspNetCore.Mvc;
using QLThuVien.Models;

namespace QLThuVien.Controllers
{
    [Route("api/[controller]")]
    public class UploadImageApiController : ControllerBase
    {
        QlthuVienLtwebContext db = new QlthuVienLtwebContext();

        [HttpPost("uploadAdmin")]
        public async Task<IActionResult> uploadAdmin(IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ImagesAdmin", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                var user = HttpContext.Session.GetString("Username");
                var nv = db.NhanViens.FirstOrDefault(u=>u.Username == user);

                nv.AnhDaiDien = fileName.ToString();
                db.SaveChanges();
                
                // Lưu đường dẫn của file ảnh vào cơ sở dữ liệu hoặc xử lý logic khác
                // ...

                return Ok(new { message = "Upload successful", imagePath = "/ImagesAdmin/" + fileName });
            }

            // Xử lý lỗi hoặc thông báo khi không có file được chọn
            // ...

            return BadRequest(new { message = "No file selected for upload" });
        }


        [HttpPost("uploadUser")]
        public async Task<IActionResult> uploadUser(IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ImagesUser", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                var user = HttpContext.Session.GetString("Username");
                var nd = db.NguoiDocs.FirstOrDefault(u => u.Username == user);

                nd.AnhDaiDien = fileName.ToString();
                db.SaveChanges();

                // Lưu đường dẫn của file ảnh vào cơ sở dữ liệu hoặc xử lý logic khác
                // ...

                return Ok(new { message = "Upload successful", imagePath = "/ImagesUser/" + fileName });
            }

            // Xử lý lỗi hoặc thông báo khi không có file được chọn
            // ...

            return BadRequest(new { message = "No file selected for upload" });
        }
    }
}
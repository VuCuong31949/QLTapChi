using QLTapChi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLTapChi.Areas.Admin.Controllers
{
    public class PhanCongController : Controller
    {
        // GET: Admin/PhanCong
        QLTapChiEntities db = new QLTapChiEntities();

        public ActionResult BaiVietChoDuyet()
        {
            // Kiểm tra đăng nhập và loại biên tập viên
            if (Session["idUser"] == null || Session["LoaiNguoiDung"] == null || Session["LoaiBienTapVien"].ToString() != "TongBienTap")
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            int idBTV = (int)Session["idUser"];
            var bienTapVien = db.BienTapViens.FirstOrDefault(b => b.IDBienTapVien == idBTV);

            if (bienTapVien == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            string chuyenNganh = bienTapVien.ChuyenNganh;

            // Lấy bài viết chờ duyệt theo chuyên ngành của biên tập viên
            var baiChoDuyet = db.TapChiBaiViets
                                .Where(b => b.TrangThai == 0 && b.LinhVuc.TenLinhVuc == chuyenNganh)
                                .OrderByDescending(b => b.NgayGui)
                                .ToList();
            int soBaiChoDuyet = baiChoDuyet.Count;
    ViewBag.SoBaiChoDuyet = soBaiChoDuyet;
            // Danh sách biên tập viên thuộc cùng chuyên ngành để phân công
            var bienTapVienTheoChuyenNganh = db.BienTapViens
                                               .Where(btv => btv.ChuyenNganh == chuyenNganh && btv.LoaiBienTapVien != "TongBienTap")
                                               .ToList();

            ViewBag.BienTapVien = new SelectList(bienTapVienTheoChuyenNganh, "IDBienTapVien", "HoTen");
            return View(baiChoDuyet);
        }

        // POST: Phân công biên tập viên chịu trách nhiệm
 
        [HttpPost]
        public ActionResult PhanCong(int idBaiViet, int idBienTapVien)
        {
            // Kiểm tra đăng nhập
            if (Session["idUser"] == null || Session["LoaiBienTapVien"] == null)
            {
                TempData["Error"] = "Bạn chưa đăng nhập hoặc không có quyền truy cập.";
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            var userId = (int)Session["idUser"];
            var vaiTro = Session["LoaiBienTapVien"].ToString();

            // Kiểm tra vai trò Tổng Biên Tập
            if (vaiTro != "TongBienTap")
            {
                TempData["Error"] = "Bạn không có quyền phân công biên tập viên!";
                return RedirectToAction("BaiVietChoDuyet");
            }

            // Tìm bài viết
            var baiViet = db.TapChiBaiViets.Find(idBaiViet);
            if (baiViet == null)
            {
                TempData["Error"] = "Không tìm thấy bài viết!";
                return RedirectToAction("BaiVietChoDuyet");
            }

            // Kiểm tra đã duyệt chưa
            if (baiViet.TrangThai != 0)
            {
                TempData["Error"] = "Bài viết này đã được duyệt hoặc xử lý trước đó!";
                return RedirectToAction("BaiVietChoDuyet");
            }

            // Kiểm tra đã phân công chưa
            bool daPhanCong = db.PhanCongBienTaps.Any(p => p.IDTapChiBaiViet == idBaiViet);
            if (daPhanCong)
            {
                TempData["Error"] = "Bài viết đã được phân công cho biên tập viên khác.";
                return RedirectToAction("BaiVietChoDuyet");
            }

            // Cập nhật trạng thái bài viết
            baiViet.TrangThai = 1; // Đã duyệt
            db.SaveChanges();

            // Thêm bản ghi phân công
            var phanCong = new PhanCongBienTap
            {
                IDTapChiBaiViet = baiViet.IDTapChiBaiViet,
                IDBienTapVien = idBienTapVien,
                NgayPhanCong = DateTime.Now
            };
            db.PhanCongBienTaps.Add(phanCong);
            db.SaveChanges();

            TempData["Success"] = "Phân công biên tập viên thành công.";
            return RedirectToAction("BaiVietChoDuyet");
        }
        public ActionResult BaiVietChoPhanBien()
        {
            int idBTV = (int)Session["idUser"];
            var bienTapVien = db.BienTapViens.FirstOrDefault(b => b.IDBienTapVien == idBTV);

            if (bienTapVien == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            string chuyenNganh = bienTapVien.ChuyenNganh;

            // Lấy bài viết chờ duyệt theo chuyên ngành của biên tập viên
            var baiChoPhanBien = db.TapChiBaiViets
                                .Where(b => b.TrangThai == 1 && b.LinhVuc.TenLinhVuc == chuyenNganh)
                                .OrderByDescending(b => b.NgayGui)
                                .ToList();
            int soBaiChoPB = baiChoPhanBien.Count;
            ViewBag.SoBaiChoPB = soBaiChoPB;
            // Danh sách biên tập viên thuộc cùng chuyên ngành để phân công
            var PhanBien = db.NguoiDungs.Where(pb => pb.LinhVuc.TenLinhVuc == chuyenNganh && pb.PhanBien == true).ToList();

            ViewBag.PhanBien = new SelectList(PhanBien, "IDNguoiDung", "HoTen");
          
            return View( baiChoPhanBien);
        }
        [HttpPost]
        public ActionResult PhanCongPhanBien(int idBaiViet, int idPhanBien)
        {
            // Kiểm tra đăng nhập
            if (Session["idUser"] == null || Session["LoaiBienTapVien"] == null)
            {
                TempData["Error"] = "Bạn chưa đăng nhập hoặc không có quyền truy cập.";
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            var userId = (int)Session["idUser"];
            var vaiTro = Session["LoaiBienTapVien"].ToString();

            // Tìm bài viết
            var baiViet = db.TapChiBaiViets.Find(idBaiViet);
            if (baiViet == null)
            {
                TempData["Error"] = "Không tìm thấy bài viết!";
                return RedirectToAction("BaiVietChoDuyet");
            }

            // Kiểm tra đã duyệt chưa
            if (baiViet.TrangThai != 1)
            {
                TempData["Error"] = "Bài viết này đã được duyệt hoặc xử lý trước đó!";
                return RedirectToAction("BaiVietChoDuyet");
            }

            // Kiểm tra đã phân công chưa
            bool daPhanCong = db.PhanCongs.Any(p => p.IDTapChiBaiViet == idBaiViet);
            if (daPhanCong)
            {
                TempData["Error"] = "Bài viết đã được phân công cho biên tập viên khác.";
                return RedirectToAction("BaiVietChoPhanBien");
            }

            // Cập nhật trạng thái bài viết
            baiViet.TrangThai = 2; // Đã duyệt
            db.SaveChanges();

            // Thêm bản ghi phân công
            var phanCong = new PhanCong
            {
                IDTapChiBaiViet = baiViet.IDTapChiBaiViet,
                IDNguoiPhanBien = userId,
                NgayPhanCong = DateTime.Now,
                NgayKetThuc = DateTime.Now
            };
            db.PhanCongs.Add(phanCong);
            db.SaveChanges();

            TempData["Success"] = "Phân công phản biện thành công.";
            return RedirectToAction("BaiVietChoPhanBien");
        }
        public ActionResult DownloadFile(int id)
        {
            // Tìm bài báo theo ID
            var baiBao = db.TapChiBaiViets.FirstOrDefault(b => b.IDTapChiBaiViet == id);

            // Nếu không tìm thấy bài báo, trả về 404
            if (baiBao == null)
            {
                return HttpNotFound();
            }

            // Lấy đường dẫn tới file (trong trường hợp là "NoiDung" là đường dẫn file)
            string filePath = Server.MapPath("~/" + baiBao.NoiDung);

            // Kiểm tra file có tồn tại không
            if (!System.IO.File.Exists(filePath))
            {
                return HttpNotFound();
            }

            // Lấy tên file
            string fileName = Path.GetFileName(filePath);

            // Trả về file dưới dạng download
            return File(filePath, "application/octet-stream", fileName);
        }
    }
}
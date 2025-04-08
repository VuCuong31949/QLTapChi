using QLTapChi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLTapChi.Areas.Admin.Controllers
{
    public class PhanCongController : Controller
    {
        // GET: Admin/PhanCong
        QLTapChiEntities db = new QLTapChiEntities();

        // GET: Xem các bài viết chờ duyệt
        public ActionResult BaiVietChoDuyet()
        {
            var baiChoDuyet = db.TapChiBaiViets
                                .Where(b => b.TrangThai == 0) // Chờ duyệt
                                .OrderByDescending(b => b.NgayGui)
                                .ToList();
            // Lấy danh sách lĩnh vực của các bài viết chờ duyệt
            var linhVucCuaBaiViet = baiChoDuyet.Select(b => b.LinhVuc.TenLinhVuc).Distinct().ToList();

            // Lọc biên tập viên có chuyên ngành phù hợp với lĩnh vực của bài viết
            var bienTapVienTheoChuyenNganh = db.BienTapViens
                                                .Where(btv => linhVucCuaBaiViet.Contains(btv.ChuyenNganh))
                                                .ToList();

            ViewBag.BienTapVien = new SelectList(db.BienTapViens, "IDBienTapVien", "HoTen");
            return View(baiChoDuyet);
        }
        public ActionResult BaiVietChoDuyet(int id)
        {
            // Kiểm tra đăng nhập và loại biên tập viên
            if (Session["idUser"] == null || Session["LoaiNguoiDung"] == null || Session["LoaiNguoiDung"].ToString() != "Tong")
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

            // Danh sách biên tập viên thuộc cùng chuyên ngành để phân công
            var bienTapVienTheoChuyenNganh = db.BienTapViens
                                               .Where(btv => btv.ChuyenNganh == chuyenNganh)
                                               .ToList();

            ViewBag.BienTapVien = new SelectList(bienTapVienTheoChuyenNganh, "IDBienTapVien", "HoTen");
            return View(baiChoDuyet);
        }

        // POST: Phân công biên tập viên chịu trách nhiệm
        public ActionResult PhanCong()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PhanCong(int idBaiViet, int idBienTapVien)
        {
            // Lấy thông tin bài viết
            var baiViet = db.TapChiBaiViets.Find(idBaiViet);
            if (baiViet != null)
            {
                // Kiểm tra xem người dùng hiện tại có phải là Tổng Biên Tập viên không
                var userId = (int)Session["UserID"]; // Giả sử ID người dùng đang đăng nhập được lưu trong Session
                var VaiTro = Session["LoaiNguoiDung"];

                // Nếu người dùng là Tổng Biên Tập viên
                if (VaiTro != null)
                {
                    // Cập nhật trạng thái bài viết đã duyệt
                    baiViet.TrangThai = 1; // Trạng thái đã duyệt
                    db.SaveChanges();

                    // Phân công biên tập viên chịu trách nhiệm
                    var phanCong = new PhanCongBienTap
                    {
                        IDTapChiBaiViet = baiViet.IDTapChiBaiViet,
                        IDBienTapVien = idBienTapVien,
                        NgayPhanCong = DateTime.Now
                    };

                    // Thêm phân công vào cơ sở dữ liệu
                    db.PhanCongBienTaps.Add(phanCong);
                    db.SaveChanges();

                    return RedirectToAction("BaiVietChoDuyet");
                }
                else
                {
                    // Nếu không phải Tổng Biên Tập viên, hiển thị thông báo lỗi
                    TempData["Error"] = "Bạn không có quyền phân công biên tập viên!";
                    return RedirectToAction("BaiVietChoDuyet");
                }
            }

            // Nếu bài viết không tồn tại
            return View();
        }
    }
}
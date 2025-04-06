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
        private QLTapChiEntities db = new QLTapChiEntities();
        public ActionResult DanhSachPhanCong()
        {
            var phancong = db.PhanCongs.ToList();
            return View(phancong);
        }
        public ActionResult phancong()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemTaiKhoan(NguoiDung _user, string MatKhauLai)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra trùng lặp
                bool checkTenDangNhap = db.NguoiDungs.Any(s => s.HoTen == _user.HoTen);
                bool checkEmail = db.NguoiDungs.Any(s => s.Email == _user.Email);
                bool checkSdt = db.NguoiDungs.Any(s => s.SDT == _user.SDT);
                if (checkTenDangNhap || checkEmail || checkSdt)
                {
                    if (checkTenDangNhap)
                    {
                        ViewBag.errorTenDangNhap = "* Tên đăng nhập đã tồn tại!";
                    }
                    if (checkEmail)
                    {
                        ViewBag.erroremail = "* Email đã tồn tại!";
                    }
                    if (checkSdt)
                    {
                        ViewBag.errorsdt = "* Số điện thoại đã được đăng ký!";
                    }
                    return View();
                }
                // Kiểm tra mật khẩu khớp
                if (_user.MatKhau != MatKhauLai)
                {
                    ViewBag.errorMatKhauLai = "* Mật khẩu nhập lại không khớp!";
                    return View();
                }

                // Mã hóa mật khẩu
                _user.MatKhau = Hashing.ToSHA256(_user.MatKhau);
                db.Configuration.ValidateOnSaveEnabled = false;
                db.NguoiDungs.Add(_user);
                db.SaveChanges();

                return RedirectToAction("TKCaNhan", "TaiKhoan");
            }
            return View();
        }
    }
}
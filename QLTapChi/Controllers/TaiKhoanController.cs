using QLTapChi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLTapChi.Controllers
{
    public class TaiKhoanController : Controller
    {
        // GET: TaiKhoan
        QLTapChiEntities db = new QLTapChiEntities();
        public ActionResult DanhSachTaiKhoan()
        {
            var tk = db.NguoiDungs.ToList();
            return View(tk);

        }
        public ActionResult ThemTaiKhoan()
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

        public ActionResult TKCaNhan(int id)
        {
            NguoiDung timkiemUser = db.NguoiDungs.Find(id);
            return View(timkiemUser);
        }
        public ActionResult CapNhatTaiKhoan(int id)
        {
            NguoiDung timkiemUser = db.NguoiDungs.Find(id);
            return View(timkiemUser);
        }
        [HttpPost]
        public ActionResult CapNhatTaiKhoan(NguoiDung model)
        {
            NguoiDung EditUser = db.NguoiDungs.Find(model.IDNguoiDung);
            // Kiểm tra tên đăng nhập trùng lặp
            var checkTenDangNhap = db.NguoiDungs.Any(u => u.HoTen == model.HoTen && u.IDNguoiDung != model.IDNguoiDung);
            if (checkTenDangNhap)
            {
                ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã tồn tại.");
                return View(EditUser);
            }
            // Kiểm tra số điện thoại trùng lặp
            var checkSDT = db.NguoiDungs.Any(u => u.SDT == model.SDT && u.IDNguoiDung != model.IDNguoiDung);
            if (checkSDT)
            {
                ModelState.AddModelError("SDT", "Số điện thoại đã được sử dụng.");
                return View(EditUser);
            }
            EditUser.HoTen = model.HoTen;
            // Kiểm tra nếu mật khẩu đã được thay đổi, sau đó mã hóa mật khẩu
            if (!string.IsNullOrEmpty(model.MatKhau) && EditUser.MatKhau != model.MatKhau)
            {
                EditUser.MatKhau = Hashing.ToSHA256(model.MatKhau); // Mã hóa mật khẩu trước khi lưu
            }
           EditUser.DiaChi = model.DiaChi;
            EditUser.Email = model.Email;
            EditUser.SDT = model.SDT;
            EditUser.IDLinhVuc = model.IDLinhVuc;
            EditUser.ChucDanh = model.ChucDanh;
            EditUser.GioiTinh = model.GioiTinh;
            EditUser.QuocGia = model.QuocGia;
            EditUser.ToChuc = model.ToChuc;
            EditUser.PhanBien = model.PhanBien;
            db.SaveChanges();
            return RedirectToAction("TKCaNhan", "TaiKhoan");
        }
        public ActionResult XoaTaiKhoan(int id)
        {
            // Tìm đối tượng xóa
            var deleteUser = db.NguoiDungs.Find(id);
            // Xóa
            if (deleteUser != null)
            {
                db.NguoiDungs.Remove(deleteUser);
                db.SaveChanges();
            }
            return RedirectToAction("DanhSachTaiKhoan");
        }

        public ActionResult DangNhap()
        {

            if (Session["idUser"] != null)
            {
                ViewBag.ShowSuccessMessage = true;
                return RedirectToAction("DangNhap_ThanhCong", "ThongBao");
            }
            return View("DangNhap");
        }
        [HttpPost]
        public ActionResult DangNhap(string email, string matkhau)
        {
            if (ModelState.IsValid)
            {
                var f_matkhau = Hashing.ToSHA256(matkhau);
                var data = db.NguoiDungs.Where(s => s.HoTen.Equals(email) && s.MatKhau.Equals(f_matkhau)).ToList();
                if (data.Count() > 0)
                {
                    Session["UserName"] = data.FirstOrDefault().HoTen;
                    Session["idUser"] = data.FirstOrDefault().IDNguoiDung;
                    ViewBag.ShowSuccessMessage = true;

                    // Lưu thông tin người dùng vào Cookie
                    //HttpCookie userCookie = new HttpCookie("UserInfo");
                    //userCookie["UserName"] = data.FirstOrDefault().TenDangNhap;
                    //userCookie["idUser"] = data.FirstOrDefault().ID_TaiKhoan.ToString();
                    //userCookie.Expires = DateTime.Now.AddDays(7); // Cookie hết hạn sau 7 ngày
                    //Response.Cookies.Add(userCookie);

                    return RedirectToAction("DangNhap_ThanhCong", "ThongBao");
                }
                else
                {
                    ViewBag.Error = "* Tài khoản hoặc mật khẩu không đúng !";
                    return View();
                }
            }
            return View();
        }

        //public ActionResult DangXuat()
        //{
        //    Session.Clear();
        //    return View("DangNhap");
        //}
        //public ActionResult ThongTinTaiKhoan()
        //{
        //    if (Session["idUser"] == null)
        //    {
        //        return View("DangNhap");
        //    }

        //    int idUser = (int)Session["idUser"];
        //    var user = db.TaiKhoans.Find(idUser);

        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(user);
        //}
        //public ActionResult CapNhat(int id)
        //{
        //    TaiKhoan timkiemUser = db.TaiKhoans.Find(id);
        //    return View(timkiemUser);
        //}
    }
}
using QLTapChi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLTapChi.Controllers
{
    public class PhanBienController : Controller
    {
        QLTapChiEntities db = new QLTapChiEntities();
        // GET: PhanBien
        public ActionResult PhanBien()
        {
            int idPB = (int)Session["idUser"];
            var bienTapVien = db.PhanBiens.FirstOrDefault(b => b.IDPhanBien == idPB);

            if (bienTapVien == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            //string chuyenNganh = bienTapVien.ChuyenNganh;

            var baiPhanBien = (from b in db.TapChiBaiViets
                                  join p in db.PhanCongs on b.IDTapChiBaiViet equals p.IDTapChiBaiViet
                                  where b.TrangThai == 2 && p.IDNguoiPhanBien == idPB
                                  orderby b.NgayGui descending
                                  select b).ToList();
            //int soBaiChoPB = baiPhanBien.Count;          
            //ViewBag.PhanBien = new SelectList(baiPhanBien, "IDNguoiDung", "HoTen");

            return View(baiPhanBien);
        }
    }
}
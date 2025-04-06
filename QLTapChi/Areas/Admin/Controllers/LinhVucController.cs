using QLTapChi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLTapChi.Areas.Admin.Controllers
{
    public class LinhVucController : Controller
    {
        // GET: Admin/LinhVuc

        QLTapChiEntities db = new QLTapChiEntities();
        public ActionResult DanhSachLinhVuc()
        {
            var danhSach = db.LinhVucs.ToList();
            return View(danhSach);
        }
        public ActionResult createLV()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult createLV(LinhVuc model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    db.LinhVucs.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("DanhSachLinhVuc");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi thêm Linh Vuc: " + ex.Message);
                }
            }
            return View(model);
            
        }
    }
}
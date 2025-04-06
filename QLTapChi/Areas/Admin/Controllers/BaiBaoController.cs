﻿using QLTapChi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLTapChi.Areas.Admin.Controllers
{
    public class BaiBaoController : Controller
    {
        // GET: Admin/BaiBao
        QLTapChiEntities db = new QLTapChiEntities();
        public ActionResult Index()
        {
            var BaiBao = db.TapChiBaiViets.OrderByDescending(x =>x.IDTapChiBaiViet).ToList();
            return View(BaiBao);
        }
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(TapChiBaiViet model, HttpPostedFileBase File)
        {
            model.TrangThai = 0;
            model.NgayGui = DateTime.Now;
            if (File != null && File.ContentLength > 0)
            {
                string rootFolder = Server.MapPath("/Content/BaiViet/");
                string pathImage = rootFolder + File.FileName;
                File.SaveAs(pathImage);
                //Lưu thuộc tính url
                model.NoiDung = "Content/BaiViet/" + File.FileName;
                db.TapChiBaiViets.Add(model);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult CapNhatBaiBao(int id)
        {
            var baiBao = db.TapChiBaiViets.FirstOrDefault(s => s.IDTapChiBaiViet == id);
            if (baiBao == null)
            {
                return HttpNotFound();
            }

            return View(baiBao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhatBaiBao(TapChiBaiViet model, HttpPostedFileBase File)
        {
            var updateModel = db.TapChiBaiViets.Find(model.IDTapChiBaiViet);
            //2.Gán Giá Trị cho đối tượng
            updateModel.TieuDe = model.TieuDe;
            updateModel.TrangThai = model.TrangThai;
            updateModel.LinhVuc = model.LinhVuc;
            updateModel.GhiChu = model.GhiChu;
           
            
            if (File != null && File.ContentLength > 0)
            {
                string rootFolder = Server.MapPath("/Content/BaiViet/");
                string pathImage = rootFolder + File.FileName;
                File.SaveAs(pathImage);
                // Lưu thuộc tính url
                updateModel.NoiDung = "Content/BaiViet/" + File.FileName;

            }

             db.SaveChanges();
                return RedirectToAction("Index");
           
        }
        public ActionResult XoaBaiBao(int id)
        {
            var model = db.TapChiBaiViets.Find(id);
            if (model != null)
            {
                db.TapChiBaiViets.Remove(model);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
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

        //public ActionResult ChiTietSanPham(int id)
        //{
        //    // Tìm sản phẩm theo ID
        //    var sanPham = db.SanPhams
        //    .Include(s => s.HangSX)         // Lấy đầy đủ thông tin từ HangSX
        //    .Include(s => s.LoaiSanPham)     // Lấy đầy đủ thông tin từ LoaiSanPham
        //    .FirstOrDefault(s => s.IDSanPham == id);
        //    if (sanPham == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    // Lấy thông tin chi tiết sản phẩm từ bảng ThongTinChiTietSP
        //    var thongTinChiTiet = db.ThongTinChiTietSPs.FirstOrDefault(c => c.id_sanpham == id);

        //    // Tạo ViewModel để chuyển thông tin sản phẩm và chi tiết sản phẩm vào View
        //    // ViewModel này đã được thêm tại Folder (Models)
        //    var viewModel = new SanPhamViewModel
        //    {
        //        SanPham = sanPham,
        //        ThongTinChiTiet = thongTinChiTiet,
        //        HangSX = sanPham.HangSX,
        //        LoaiSanPham = sanPham.LoaiSanPham
        //    };

        //    return View(viewModel); // Trả về View hiển thị chi tiết sản phẩm
        //}
    }
}
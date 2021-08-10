using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class QuanLySanPhamController : BaseController
    {   
        
        QuanLyBanHang6Entities db = new QuanLyBanHang6Entities();
        // GET: QuanLySanPham
        
        public ActionResult Index()
        {
            return View(db.products.Where(n=>n.deleteproduct!=true));
        }
        [HttpGet]
       
        public ActionResult TaoMoi()
        {
            //ViewBag.LoaiSP = new SelectList(db.Categories.OrderBy(n => n.name), "Id", "name");
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoMoi(product sp,HttpPostedFileBase image1,HttpPostedFileBase image2)
        {
            //ViewBag.LoaiSP = new SelectList(db.Categories.OrderBy(n => n.name), "Id", "name");
            if (image1.ContentLength > 0&& image2.ContentLength>0)
            {
               // Lấy tên hình
                var fileName = Path.GetFileName(image1.FileName);
                var fileName2 = Path.GetFileName(image2.FileName);
                // Lấy hình ảnh chèn vào thư mục hình ảnh
                var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP/"), fileName);
                var path2 = Path.Combine(Server.MapPath("~/Content/HinhAnhSP/"), fileName2);
                //Nếu thư mục chứa hình ảnh rồi thì xuất ra thông báo

                image1.SaveAs(path);
                image2.SaveAs(path2);
                sp.image1 = fileName;
                sp.image2 = fileName2;
            
            }
            db.products.Add(sp);
            db.SaveChanges();
            SetAlert("Thêm sản phẩm thành công", "success");
            return RedirectToAction("Index","QuanLySanPham");
        }
        [ValidateInput(false)]
        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            product sp = db.products.SingleOrDefault(n => n.Id == id);
            if (sp == null)
            {
                return HttpNotFound();
            }

            return View(sp);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(product model,HttpPostedFileBase image1, HttpPostedFileBase image2)
        {
            if (image1.ContentLength > 0 && image2.ContentLength > 0 || image1!=null && image2!=null)
            {
                // Lấy tên hình
                var fileName = Path.GetFileName(image1.FileName);
                var fileName2 = Path.GetFileName(image2.FileName);
                // Lấy hình ảnh chèn vào thư mục hình ảnh
                var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP/"), fileName);
                var path2 = Path.Combine(Server.MapPath("~/Content/HinhAnhSP/"), fileName2);
                //Nếu thư mục chứa hình ảnh rồi thì xuất ra thông báo

                image1.SaveAs(path);
                image2.SaveAs(path2);
                model.image1 = fileName;
                model.image2 = fileName2;

            }else if(image1.ContentLength ==0 && image2.ContentLength == 0 || image1 != null && image2 != null)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            SetAlert("Chỉnh sửa sản phẩm thành công", "success");
            return RedirectToAction("Index");



            // Su dung neu cai dat valid ben meta data day du
            //if (ModelState.IsValid)
            //{
            //    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(model);
        }
        [HttpGet]
        public ActionResult Xoa(int? id)
        {

            
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            product sp = db.products.SingleOrDefault(n => n.Id == id);
            if (sp == null)
            {
                return HttpNotFound();
            }

            
            ViewBag.LoaiSP = new SelectList(db.Categories.OrderBy(n => n.name), "Id", "name", sp.categories_id);
            return View(sp);
        }
        [HttpPost]
        public ActionResult Xoa(int? id, FormCollection f)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product model = db.products.SingleOrDefault(n => n.Id == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            model.deleteproduct = true;
            //db.products.Remove(model);
            db.SaveChanges();
            SetAlert("Xóa sản phẩm thành công", "success");
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}
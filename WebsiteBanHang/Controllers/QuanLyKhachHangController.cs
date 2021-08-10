using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
namespace WebsiteBanHang.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class QuanLyKhachHangController : BaseController
    {
        QuanLyBanHang6Entities db = new QuanLyBanHang6Entities();
        // GET: QuanLyKhachHang
        public ActionResult DSKhachHang()
        {
            return View(db.users.Where(n =>n.deleteUser!=true&&n.Id!=3));
        }
        public ActionResult Xoa(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user model = db.users.SingleOrDefault(n => n.Id == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            model.deleteUser = true;
            //db.users.Remove(model);
            db.SaveChanges();
            SetAlert("Xóa tài khoản thành công", "success");
            return RedirectToAction("DSKhachHang");
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    public class DonHangController : BaseController
    {

        QuanLyBanHang6Entities db = new QuanLyBanHang6Entities();
        // GET: DonHang
        public ActionResult DangXacNhan()
        {
            user tv = Session["TaiKhoan"] as user;
            var lst = db.Bills.Where(n => n.status == false&&n.buyer_id==tv.Id).OrderBy(n => n.buy_date);
            return View(lst);
           
        }
        public ActionResult DaXacNhan()
        {
            user tv = Session["TaiKhoan"] as user;
            var lst = db.Bills.Where(n => n.status == true && n.buyer_id == tv.Id).OrderBy(n => n.buy_date);
            return View(lst);

        }
        public ActionResult DaGiao()
        {
            user tv = Session["TaiKhoan"] as user;
            var lst = db.Bills.Where(n => n.transport_status== true&& n.buy_status == false && n.buyer_id == tv.Id).OrderBy(n => n.buy_date);
            return View(lst);

        }
        public ActionResult DaHoanThanh()
        {
            user tv = Session["TaiKhoan"] as user;
            var lst = db.Bills.Where(n => n.buy_status == true && n.buyer_id == tv.Id).OrderBy(n => n.buy_date);
            return View(lst);

        }
        [HttpGet]
        public ActionResult XemDonHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill model = db.Bills.SingleOrDefault(n => n.Id == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            // Lấy ds chi tiết đơn hàng để hiển thị cho người dùng thấy
            var lstChiTietDH = db.billproducts.Where(n => n.bill_id == id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            return View(model);
        }
        public ActionResult XemDonHangHoanThanh(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill model = db.Bills.SingleOrDefault(n => n.Id == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            // Lấy ds chi tiết đơn hàng để hiển thị cho người dùng thấy
            var lstChiTietDH = db.billproducts.Where(n => n.bill_id == id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            return View(model);
        }
        [HttpPost]
        public ActionResult NhanXet(int MaSP,FormCollection f)
        {
            string nhanxet= f["txtNhanXet"].ToString();
            user tv = Session["TaiKhoan"] as user;
            comment bl = new comment();
            bl.comment1 = nhanxet;
            bl.comment_date = DateTime.Now;
            bl.commenter_id = tv.Id;
            bl.product_id = MaSP;
            db.comments.Add(bl);
            db.SaveChanges();
            SetAlert("Nhận xét sản phẩm thành công", "success");
            return RedirectToAction("DaHoanThanh");
        }
    }
}
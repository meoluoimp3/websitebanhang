using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
namespace WebsiteBanHang.Controllers
{
    public class GioHangController : BaseController
    {
        QuanLyBanHang6Entities db = new QuanLyBanHang6Entities();
        public List<ItemGioHang> LayGioHang()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;
                
            }
            return lstGioHang;
        }
        public ActionResult ThemGioHang(int MaSP,string strURL)
        {
            product sp = db.products.SingleOrDefault(n => n.Id == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<ItemGioHang> lstGioHang = LayGioHang();
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            var productcheck = from sl in db.billproducts
                               where sl.product_id == MaSP
                               select sl;
            int? slban = 0;
            slban += productcheck.Sum(n => n.quantity);
            if (spCheck != null)
            {
                if (sp.quantity-slban < spCheck.SoLuong)
                {
                    return View("ThongBao");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);
            }
            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (sp.quantity < itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            lstGioHang.Add(itemGH);
            return Redirect(strURL);
        }
        public int TinhTongSoLuong()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.SoLuong);
        }
        public int TinhTongTien()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.ThanhTien);
        }
        public ActionResult GioHangPartial()
        {
            if (TinhTongSoLuong() == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }
        // GET: GioHang
        public ActionResult XemGioHang()
        {
            List<ItemGioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        [HttpGet]
        public ActionResult SuaGioHang(int MaSP)
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            product sp = db.products.SingleOrDefault(n => n.Id == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<ItemGioHang> lstGioHang = LayGioHang();
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.GioHang = lstGioHang;
            return View(spCheck);
        }
        [HttpPost]
        public ActionResult CapNhatGioHang(ItemGioHang itemGH)
        {
            product spCheck = db.products.Single(n => n.Id == itemGH.MaSP);
            var productcheck = from sl in db.billproducts
                               where sl.product_id == itemGH.MaSP
                               select sl;
            int? slban = 0;
            slban += productcheck.Sum(n => n.quantity);
            
            if (spCheck.quantity-slban < itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            List<ItemGioHang> lstGH = LayGioHang();
            ItemGioHang itemGHUpdate = lstGH.Find(n => n.MaSP == itemGH.MaSP);
            itemGHUpdate.SoLuong = itemGH.SoLuong;
            itemGHUpdate.ThanhTien = itemGHUpdate.SoLuong * itemGHUpdate.DonGia;
            return RedirectToAction("XemGioHang");
        }
        public ActionResult XoaGioHang(int MaSP)
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            product sp = db.products.SingleOrDefault(n => n.Id == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<ItemGioHang> lstGioHang = LayGioHang();
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            lstGioHang.Remove(spCheck);
            return RedirectToAction("XemGioHang");
        }
        public ActionResult DatHang()
        {

            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (Session["TaiKhoan"] == null)
            {
                return RedirectToAction("DangKy", "Home");
            }
            
            user tv = Session["TaiKhoan"] as user;
            Bill ddh = new Bill();
            ddh.buy_date = DateTime.Now;
            ddh.transport_status = false;
            ddh.buy_status = false;
            ddh.status = false;
            ddh.discount_percent = 0;
            ddh.buyer_id = tv.Id;
            ddh.price_total = TinhTongTien();
            ddh.total = TinhTongTien() + 30000;
            db.Bills.Add(ddh);
            db.SaveChanges();
            infobill ifb = new infobill();
            ifb.bill_id = ddh.Id;
            ifb.address = tv.address;
            ifb.phone_number = tv.phone;
            db.infobills.Add(ifb);
            db.SaveChanges();
            //theem chi tieets don dat hang
            List<ItemGioHang> lstGH = LayGioHang();
            foreach(var item in lstGH)
            {
                billproduct ctdh = new billproduct();
                ctdh.bill_id = ddh.Id;
                ctdh.product_id = item.MaSP;
                ctdh.quantity = item.SoLuong;
                ctdh.unit_price = item.DonGia;
                db.billproducts.Add(ctdh);

            }
            db.SaveChanges();
            SetAlert("Đặt hàng thành công", "success");
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class ThongKeController : Controller
    {
        QuanLyBanHang6Entities db = new QuanLyBanHang6Entities();
        
        // GET: ThongKe
        public ActionResult Index()
        {
            ViewBag.TongDoanhThu = ThongKeDoanhThu();
            ViewBag.TongDDH = ThongKeDonHang();
            ViewBag.TongThanhVien = TongThanhVien();
            ViewBag.TongDoanhThuTheoThang = ThongKeDoanhThuTheoThang(5, 2021);
            return View();
            
        }
        public int? ThongKeDoanhThu()
        {
            int? TongDoanhThu = 0;
            TongDoanhThu += db.billproducts.Sum(n => n.quantity*n.unit_price);
            return TongDoanhThu;
        }

        public int ThongKeDonHang()
        {
            //Đếm đơn đặt hàng
            int sl = db.Bills.Count();
            return sl;
        }
        public int? ThongKeDoanhThuTheoThang(int Thang, int Nam)
        {
            var lstDDH = db.Bills.Where(n => n.buy_date.Value.Month == Thang && n.buy_date.Value.Year == Nam);
            int? TongTien = 0;
            //Duyệt chi tiết đơn hàng theo điều kiện
            foreach (var item in lstDDH)
            {
                TongTien += item.billproducts.Sum(n => n.unit_price*n.quantity);
            }
            return TongTien;
        }
        
        [HttpGet]
        public JsonResult ThongKeThang(string txtThang, string txtNam)
        {
            int Thang = Convert.ToInt32(txtThang);
            int Nam = Convert.ToInt32(txtNam);
            int? tongtien = ThongKeDoanhThuTheoThang(Thang, Nam);
            return Json(new
            {
                tongtien=tongtien
            },
            JsonRequestBehavior.AllowGet);
        }
        public int TongThanhVien()
        {   
            int slTV = db.users.Count();
            return slTV;
        }
    }
}
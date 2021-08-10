using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
using PagedList;
namespace WebsiteBanHang.Controllers
{
    public class TimKiemController : Controller
    {
        // GET: TimKiem
        QuanLyBanHang6Entities db = new QuanLyBanHang6Entities();
        [HttpGet]
        public ActionResult KetQuaTimKiem( string sTuKhoa,int? Page)
        {
            if (Request.HttpMethod != "GET")
            {
                Page = 1;
            }
            //Tạo biến số sp trên trang
            int PageSize = 4;
            //Tạo biến thứ 2 : Số trang hiện tại
            int PageNumber = (Page ?? 1);
            ViewBag.TuKhoa = sTuKhoa;
            var lstSP = db.products.Where(n => n.name.Contains(sTuKhoa));
            return View(lstSP.OrderBy(n=>n.name).ToPagedList(PageNumber,PageSize));
        }
        [HttpPost]
        public ActionResult LayTuKhoa(string sTuKhoa)
        {
            return RedirectToAction("KetQuaTimKiem", new { @sTuKhoa = sTuKhoa });
        }
    }
}
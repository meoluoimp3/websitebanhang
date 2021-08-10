using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
using PagedList;
namespace WebsiteBanHang.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        QuanLyBanHang6Entities db = new QuanLyBanHang6Entities();
        [ChildActionOnly]
        public ActionResult ProductStylePartial()
        {
            return PartialView();
        }
        public ActionResult XemChiTiet(int? MaSP)
        {
            if (MaSP == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            }
            product sp = db.products.SingleOrDefault(n => n.Id == MaSP);
            if (sp == null)
            {
                return HttpNotFound();
            }
            var lstBinhLuan = db.comments.Where(n => n.product_id == MaSP);
            ViewBag.lstBinhLuan = lstBinhLuan;
            return View(sp);
        }
        

     


        public ActionResult Product(int? MaLoaiSP,int? page)
        {
            var LstSP = db.products.Where(n => n.categories_id == MaLoaiSP && n.deleteproduct!=true);
            if (LstSP.Count() == 0)
            {
                return HttpNotFound();
            }
            int PageSize = 12;
            int PageNumber = (page ?? 1);
            ViewBag.MaLoaiSP = MaLoaiSP;
            return View(LstSP.OrderBy(n=>n.Id).ToPagedList(PageNumber,PageSize));
        }
    }
    
}
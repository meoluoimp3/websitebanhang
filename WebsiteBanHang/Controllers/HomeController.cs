using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Models;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
using System.Web.Security;

namespace WebsiteBanHang.Controllers
{
    public class HomeController : BaseController
    {
        QuanLyBanHang6Entities db = new QuanLyBanHang6Entities();

        public ActionResult Index()
        {
            var lstSPM = db.products.Where(n => n.@new == true&&n.deleteproduct!=true);
            //var lstSPM = db.products.Where(n=>n.categories_id==1);
            ViewBag.lstSPM = lstSPM;
            return View();
        }
        public ActionResult MenuPartial()
        {
            var lstSP = db.products;
            return PartialView(lstSP);
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();

        }
        [HttpGet]
        public ActionResult DangKy1()
        {
            return View();

        }
        [HttpPost]
        public ActionResult DangKy1(user tv)
        {
            return View();

        }
        [HttpPost]
        public ActionResult DangKy(user tv)
        {   
            if(this.IsCaptchaValid("Captcha is not valid"))
            {
                if (ModelState.IsValid)
                {
                    SetAlert("Đăng ký tài khoản thành công", "success");
                    //ViewBag.thongbao = "thêm thành công";
                    db.users.Add(tv);
                    db.SaveChanges();
                }
                else
                {
                    SetAlert("Đăng ký tài khoản thất bại", "error");
                    //ViewBag.ThongBao = "thêm thất bại";
                }
                return View();
            }
            ViewBag.thongbao = "sai mã captcha";
            return View();

        }
        public ActionResult DangNhap(FormCollection f)
        {
            string sTaiKhoan = f["txtTenDangNhap"].ToString();
            string sMatKhau = f["txtMatKhau"].ToString();
            user tv = db.users.SingleOrDefault(n => n.username == sTaiKhoan && n.password == sMatKhau);
            if (tv != null && tv.deleteUser!=true)
            {
                FormsAuthentication.SetAuthCookie(tv.username, false);
              
                Session["TaiKhoan"] = tv;
                SetAlert("Đăng nhập tài khoản thành công", "success");
                return RedirectToAction("Index");
            }
            else
            {
                SetAlert("Tài khoản hoặc mật khẩu không chính xác", "error");
                return RedirectToAction("Index");
                //return Content("Tài khoản hoặc mật khẩu không đúng!");
                //ModelState.AddModelError("thongbao", "Tên đăng nhập hoặc mật khẩu sai");
            }
            return View();
        }
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using Rotativa;
using System.Web.Mvc;
using Magnum.FileSystem;

using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class QuanLyDonHangController : BaseController
    {
        QuanLyBanHang6Entities db = new QuanLyBanHang6Entities();
        // GET: QuanLyDonHang
        public ActionResult ChuaThanhToan()
        {
            var lst = db.Bills.Where(n => n.status == false).OrderBy(n => n.buy_date);
            return View(lst);
        }
        public ActionResult ChuaGiao()
        {
            var lst = db.Bills.Where(n => n.transport_status == false).OrderBy(n => n.buy_date);
            return View(lst);
        }
        public ActionResult ChuaHoanThanh()
        {
            var lst = db.Bills.Where(n => n.buy_status == false).OrderByDescending(n => n.buy_date);
            return View(lst);
        }

        public ActionResult DaGiaoDaThanhToan()
        {
            var lst = db.Bills.Where(n => n.buy_status == true && n.transport_status == true).OrderByDescending(n => n.buy_date);
            return View(lst);
        }
        [HttpGet]
        public ActionResult DuyetDonHang(int? id)
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
        public ActionResult DuyetDonHang(Bill ddh)
        {
            // Lấy dữ liệu của đơn hàng đó
            Bill ddhUpdate = db.Bills.Single(n => n.Id == ddh.Id);
            //ddhUpdate.status = ddh.status;
            ddhUpdate.buy_status = ddh.buy_status;
            ddhUpdate.transport_status = ddh.transport_status;
            var lstChiTietDH = db.billproducts.Where(n => n.bill_id == ddh.Id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            
            db.SaveChanges();
            SetAlert("Duyệt đơn hàng thành công", "success");
            return View(ddhUpdate);
        }
        public ActionResult InDonHang(int? id)
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
        public ActionResult InDonHang1(int? id)
        {
            Bill model = db.Bills.SingleOrDefault(n => n.Id == id);
            var lstChiTietDH = db.billproducts.Where(n => n.bill_id == id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            SetAlert("In đơn hàng thành công", "success");
            return new PartialViewAsPdf("InDonHang",model)
            {
                FileName = Server.MapPath("~/content/hoadon.pdf")
            };
        }
        [HttpGet]
        public ActionResult xacnhan(int? id)
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
        public ActionResult xacnhan(Bill ddh)
        {
            Bill model = db.Bills.SingleOrDefault(n => n.Id == ddh.Id);
            var lstChiTietDH = db.billproducts.Where(n => n.bill_id == ddh.Id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            user kh = db.users.Single(n => n.Id == model.buyer_id);
            string gmailKH = kh.email;
            GuiEmail("Xác nhận đơn hàng",gmailKH, "txcbrand1234", "chieutxcbrand", "Đơn hàng của bạn đã được đặt thành công");
            Bill ddhUpdate = db.Bills.Single(n => n.Id == ddh.Id);
            ddhUpdate.status = true;
            db.SaveChanges();
            SetAlert("Xác nhận đơn hàng thành công", "success");
            return View(ddhUpdate);

        }

        public void GuiEmail(string Title, string ToEmail, string FromEmail, string PassWord, string Content)
        {
            // goi email
            MailMessage mail = new MailMessage();
            mail.To.Add(ToEmail); // Địa chỉ nhận
            mail.From = new MailAddress(ToEmail); // Địa chửi gửi
            mail.Subject = Title;  // tiêu đề gửi
            mail.Body = Content;                 // Nội dung
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; // host gửi của Gmail
            smtp.Port = 587;               //port của Gmail
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
            (FromEmail, PassWord);//Tài khoản password người gửi
            smtp.EnableSsl = true;   //kích hoạt giao tiếp an toàn SSL
            smtp.Send(mail);   //Gửi mail đi
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
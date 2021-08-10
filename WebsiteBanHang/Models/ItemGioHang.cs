using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteBanHang.Models
{
    public class ItemGioHang
    {
        public int MaSP  { get; set; }
        public string TenSP { get; set; }
        public int SoLuong { get; set; }
        public int DonGia { get; set; }
        public int ThanhTien { get; set; }
        public string HinhAnh { get; set; }
        public ItemGioHang(int iMaSP)
        {
            using (QuanLyBanHang6Entities db = new QuanLyBanHang6Entities())
            {
                this.MaSP = iMaSP;
                product sp = db.products.Single(n => n.Id == iMaSP);
                this.TenSP = sp.name;
                this.HinhAnh = sp.image1;
                this.DonGia = sp.price.Value;
                this.SoLuong = 1;
                this.ThanhTien = DonGia * SoLuong;
            }
        }
        public ItemGioHang(int iMaSP,int sl)
        {
            using (QuanLyBanHang6Entities db=new QuanLyBanHang6Entities())
            {
                this.MaSP = iMaSP;
                product sp = db.products.Single(n => n.Id == iMaSP);
                this.TenSP = sp.name;
                this.HinhAnh = sp.image1;
                this.DonGia = sp.price.Value;
                this.SoLuong = sl;
                this.ThanhTien = DonGia * SoLuong;
            }
        }
        public ItemGioHang()
        {

        }
    }
}
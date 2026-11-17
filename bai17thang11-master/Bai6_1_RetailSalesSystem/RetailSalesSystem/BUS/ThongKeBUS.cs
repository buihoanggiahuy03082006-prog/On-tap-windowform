using System;
using System.Collections.Generic;
using RetailSalesSystem.DAL;
using RetailSalesSystem.DTO;

namespace RetailSalesSystem.BUS
{
    public class ThongKeBUS
    {
        private readonly ThongKeDAL _dal = new();

        public decimal GetDoanhThuNgay(DateTime ngay) => _dal.GetDoanhThuNgay(ngay);

        public decimal GetDoanhThuThang(int thang, int nam) => _dal.GetDoanhThuThang(thang, nam);

        public int GetTongSoHoaDon() => _dal.GetTongSoHoaDon();

        public List<TopSanPhamDTO> GetTop5SanPhamBanChay() => _dal.GetTop5SanPhamBanChay();
    }
}

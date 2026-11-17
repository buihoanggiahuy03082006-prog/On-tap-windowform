using System;

namespace RetailSalesSystem.DTO
{
    public class HoaDonDTO
    {
        public int MaHD { get; set; }
        public DateTime NgayLap { get; set; } = DateTime.Now;
        public int MaNV { get; set; }
        public int? MaKH { get; set; }
        public decimal TongTien { get; set; }

        // Extra display properties
        public string? TenNV { get; set; }
        public string? TenKH { get; set; }
    }
}

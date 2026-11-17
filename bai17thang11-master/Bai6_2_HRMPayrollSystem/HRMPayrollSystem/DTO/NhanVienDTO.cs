using System;

namespace HRMPayrollSystem.DTO
{
    public class NhanVienDTO
    {
        public int MaNV { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public DateTime? NgaySinh { get; set; }
        public string CCCD { get; set; } = string.Empty;
        public int MaPB { get; set; }
        public string? TenPB { get; set; }
        public decimal LuongCoBan { get; set; }
        public double HeSoLuong { get; set; } = 1.0;

        public override string ToString() => $"[{MaNV}] {HoTen} - {TenPB}";
    }
}

namespace HRMPayrollSystem.DTO
{
    public class BangLuongDTO
    {
        public int MaLuong { get; set; }
        public int MaNV { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string? TenPB { get; set; }
        public string ThangNam { get; set; } = string.Empty;
        public decimal LuongCoBan { get; set; }
        public double HeSoLuong { get; set; }
        public double SoNgayCong { get; set; }
        public double SoGioOT { get; set; }
        public decimal ThucLinh { get; set; }
    }
}

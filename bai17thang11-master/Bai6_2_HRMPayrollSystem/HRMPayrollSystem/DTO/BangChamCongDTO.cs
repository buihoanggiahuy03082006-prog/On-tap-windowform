namespace HRMPayrollSystem.DTO
{
    public class BangChamCongDTO
    {
        public int MaCong { get; set; }
        public int MaNV { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string? TenPB { get; set; }
        public string ThangNam { get; set; } = string.Empty;
        public double SoNgayCong { get; set; }
        public double SoGioOT { get; set; }
        public bool DaKhoa { get; set; }
    }
}

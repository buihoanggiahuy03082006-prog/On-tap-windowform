namespace HRMPayrollSystem.DTO
{
    public class PhongBanDTO
    {
        public int MaPB { get; set; }
        public string TenPB { get; set; } = string.Empty;
        public string? Mota { get; set; }

        public override string ToString() => TenPB;
    }
}

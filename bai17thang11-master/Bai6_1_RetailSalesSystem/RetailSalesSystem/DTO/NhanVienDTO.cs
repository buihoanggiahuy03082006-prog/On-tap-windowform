namespace RetailSalesSystem.DTO
{
    public class NhanVienDTO
    {
        public int MaNV { get; set; }
        public string TenNV { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string ChucVu { get; set; } = string.Empty;

        public override string ToString() => $"{MaNV} - {TenNV} ({ChucVu})";
    }
}

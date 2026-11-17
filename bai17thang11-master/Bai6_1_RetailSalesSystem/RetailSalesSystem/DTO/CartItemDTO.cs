namespace RetailSalesSystem.DTO
{
    public class CartItemDTO
    {
        public string MaSP { get; set; } = string.Empty;
        public string TenSP { get; set; } = string.Empty;
        public decimal DonGia { get; set; }
        public int SoLuong { get; set; }
        public decimal ThanhTien => SoLuong * DonGia;
        public int SoLuongTon { get; set; }
    }
}

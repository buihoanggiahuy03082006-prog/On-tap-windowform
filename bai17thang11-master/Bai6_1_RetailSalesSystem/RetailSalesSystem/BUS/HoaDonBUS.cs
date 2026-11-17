using System;
using System.Collections.Generic;
using RetailSalesSystem.DAL;
using RetailSalesSystem.DTO;

namespace RetailSalesSystem.BUS
{
    public class HoaDonBUS
    {
        private readonly HoaDonDAL _dal = new();

        public List<HoaDonDTO> GetAllHoaDon() => _dal.GetAllHoaDon();

        public List<ChiTietHoaDonDTO> GetChiTietByMaHD(int maHD) => _dal.GetChiTietByMaHD(maHD);

        public (bool Success, string Message, int MaHD) ThanhToan(HoaDonDTO hoaDon, List<CartItemDTO> cartItems)
        {
            if (hoaDon.MaNV <= 0)
                return (false, "Vui lòng chọn nhân viên lập hóa đơn!", 0);

            if (cartItems == null || cartItems.Count == 0)
                return (false, "Giỏ hàng không có sản phẩm nào để thanh toán!", 0);

            foreach (var item in cartItems)
            {
                if (item.SoLuong <= 0)
                    return (false, $"Số lượng của sản phẩm '{item.TenSP}' phải lớn hơn 0!", 0);
            }

            return _dal.ThanhToanHoaDon(hoaDon, cartItems);
        }
    }
}

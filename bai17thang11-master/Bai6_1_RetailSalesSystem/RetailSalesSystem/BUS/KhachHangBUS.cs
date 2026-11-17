using System;
using System.Collections.Generic;
using RetailSalesSystem.DAL;
using RetailSalesSystem.DTO;

namespace RetailSalesSystem.BUS
{
    public class KhachHangBUS
    {
        private readonly KhachHangDAL _dal = new();

        public List<KhachHangDTO> GetAll() => _dal.GetAll();

        public KhachHangDTO? GetById(int maKH) => _dal.GetById(maKH);

        public List<KhachHangDTO> Search(string keyword) => _dal.Search(keyword);

        public (bool Success, string Message) Add(KhachHangDTO kh)
        {
            if (string.IsNullOrWhiteSpace(kh.TenKH))
                return (false, "Tên khách hàng không được để trống!");

            if (string.IsNullOrWhiteSpace(kh.DienThoai))
                return (false, "Số điện thoại không được để trống!");

            if (kh.DiemTichLuy < 0)
                kh.DiemTichLuy = 0;

            bool ok = _dal.Insert(kh);
            return ok ? (true, "Thêm khách hàng thành công!") : (false, "Thêm khách hàng thất bại!");
        }

        public (bool Success, string Message) Update(KhachHangDTO kh)
        {
            if (kh.MaKH <= 0)
                return (false, "Mã khách hàng không hợp lệ!");

            if (string.IsNullOrWhiteSpace(kh.TenKH))
                return (false, "Tên khách hàng không được để trống!");

            bool ok = _dal.Update(kh);
            return ok ? (true, "Cập nhật khách hàng thành công!") : (false, "Cập nhật khách hàng thất bại!");
        }

        public (bool Success, string Message) Delete(int maKH)
        {
            if (maKH <= 0)
                return (false, "Vui lòng chọn khách hàng cần xóa!");

            try
            {
                bool ok = _dal.Delete(maKH);
                return ok ? (true, "Xóa khách hàng thành công!") : (false, "Không tìm thấy khách hàng cần xóa!");
            }
            catch (Exception ex)
            {
                return (false, "Không thể xóa khách hàng do đã phát sinh hóa đơn mua hàng: " + ex.Message);
            }
        }
    }
}

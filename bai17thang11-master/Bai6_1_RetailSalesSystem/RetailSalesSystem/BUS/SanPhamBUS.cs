using System;
using System.Collections.Generic;
using RetailSalesSystem.DAL;
using RetailSalesSystem.DTO;

namespace RetailSalesSystem.BUS
{
    public class SanPhamBUS
    {
        private readonly SanPhamDAL _dal = new();

        public List<SanPhamDTO> GetAll() => _dal.GetAll();

        public SanPhamDTO? GetById(string maSP) => _dal.GetById(maSP);

        public List<SanPhamDTO> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return _dal.GetAll();
            return _dal.Search(keyword.Trim());
        }

        public (bool Success, string Message) Add(SanPhamDTO sp)
        {
            if (string.IsNullOrWhiteSpace(sp.MaSP))
                return (false, "Mã sản phẩm không được để trống!");

            if (string.IsNullOrWhiteSpace(sp.TenSP))
                return (false, "Tên sản phẩm không được để trống!");

            if (sp.GiaBan <= 0)
                return (false, "Giá bán sản phẩm phải lớn hơn 0!");

            if (sp.SoLuongTon < 0)
                return (false, "Số lượng tồn kho không được nhỏ hơn 0!");

            if (_dal.GetById(sp.MaSP.Trim()) != null)
                return (false, $"Mã sản phẩm '{sp.MaSP}' đã tồn tại trong hệ thống!");

            bool ok = _dal.Insert(sp);
            return ok ? (true, "Thêm sản phẩm thành công!") : (false, "Thêm sản phẩm thất bại!");
        }

        public (bool Success, string Message) Update(SanPhamDTO sp)
        {
            if (string.IsNullOrWhiteSpace(sp.MaSP))
                return (false, "Mã sản phẩm không được để trống!");

            if (string.IsNullOrWhiteSpace(sp.TenSP))
                return (false, "Tên sản phẩm không được để trống!");

            if (sp.GiaBan <= 0)
                return (false, "Giá bán sản phẩm phải lớn hơn 0!");

            if (sp.SoLuongTon < 0)
                return (false, "Số lượng tồn kho không được nhỏ hơn 0!");

            bool ok = _dal.Update(sp);
            return ok ? (true, "Cập nhật sản phẩm thành công!") : (false, "Cập nhật sản phẩm thất bại!");
        }

        public (bool Success, string Message) Delete(string maSP)
        {
            if (string.IsNullOrWhiteSpace(maSP))
                return (false, "Vui lòng chọn sản phẩm cần xóa!");

            try
            {
                bool ok = _dal.Delete(maSP);
                return ok ? (true, "Xóa sản phẩm thành công!") : (false, "Không tìm thấy sản phẩm cần xóa!");
            }
            catch (Exception ex)
            {
                return (false, "Không thể xóa sản phẩm do đã có trong chi tiết hóa đơn: " + ex.Message);
            }
        }
    }
}

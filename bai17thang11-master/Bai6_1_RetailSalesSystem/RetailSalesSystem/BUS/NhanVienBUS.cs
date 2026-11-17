using System;
using System.Collections.Generic;
using RetailSalesSystem.DAL;
using RetailSalesSystem.DTO;

namespace RetailSalesSystem.BUS
{
    public class NhanVienBUS
    {
        private readonly NhanVienDAL _dal = new();

        public List<NhanVienDTO> GetAll() => _dal.GetAll();

        public NhanVienDTO? GetById(int maNV) => _dal.GetById(maNV);

        public (bool Success, string Message) Add(NhanVienDTO nv)
        {
            if (string.IsNullOrWhiteSpace(nv.TenNV))
                return (false, "Tên nhân viên không được để trống!");

            if (string.IsNullOrWhiteSpace(nv.SoDienThoai))
                return (false, "Số điện thoại không được để trống!");

            bool ok = _dal.Insert(nv);
            return ok ? (true, "Thêm nhân viên thành công!") : (false, "Thêm nhân viên thất bại!");
        }

        public (bool Success, string Message) Update(NhanVienDTO nv)
        {
            if (nv.MaNV <= 0)
                return (false, "Mã nhân viên không hợp lệ!");

            if (string.IsNullOrWhiteSpace(nv.TenNV))
                return (false, "Tên nhân viên không được để trống!");

            bool ok = _dal.Update(nv);
            return ok ? (true, "Cập nhật nhân viên thành công!") : (false, "Cập nhật nhân viên thất bại!");
        }

        public (bool Success, string Message) Delete(int maNV)
        {
            if (maNV <= 0)
                return (false, "Vui lòng chọn nhân viên cần xóa!");

            try
            {
                bool ok = _dal.Delete(maNV);
                return ok ? (true, "Xóa nhân viên thành công!") : (false, "Không tìm thấy nhân viên cần xóa!");
            }
            catch (Exception ex)
            {
                return (false, "Không thể xóa nhân viên do đã có hóa đơn liên quan: " + ex.Message);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using HRMPayrollSystem.DAL;
using HRMPayrollSystem.DTO;

namespace HRMPayrollSystem.BUS
{
    public class NhanVienBUS
    {
        private readonly NhanVienDAL _dal = new();

        public List<NhanVienDTO> GetAll() => _dal.GetAll();

        public NhanVienDTO? GetById(int maNV) => _dal.GetById(maNV);

        public List<NhanVienDTO> Search(string keyword, int? maPB) => _dal.Search(keyword.Trim(), maPB);

        public (bool Success, string Message) Add(NhanVienDTO nv)
        {
            if (string.IsNullOrWhiteSpace(nv.HoTen))
                return (false, "Họ và tên nhân viên không được để trống!");

            if (string.IsNullOrWhiteSpace(nv.CCCD))
                return (false, "Số CCCD không được để trống!");

            if (nv.MaPB <= 0)
                return (false, "Vui lòng chọn phòng ban trực thuộc!");

            if (nv.LuongCoBan <= 0)
                return (false, "Lương cơ bản phải lớn hơn 0!");

            if (nv.HeSoLuong <= 0)
                return (false, "Hệ số lương phải lớn hơn 0!");

            try
            {
                bool ok = _dal.Insert(nv);
                return ok ? (true, "Thêm nhân viên thành công!") : (false, "Thêm nhân viên thất bại!");
            }
            catch (Exception ex)
            {
                return (false, "Lỗi thêm nhân viên (CCCD có thể đã tồn tại): " + ex.Message);
            }
        }

        public (bool Success, string Message) Update(NhanVienDTO nv)
        {
            if (nv.MaNV <= 0)
                return (false, "Mã nhân viên không hợp lệ!");

            if (string.IsNullOrWhiteSpace(nv.HoTen))
                return (false, "Họ và tên nhân viên không được để trống!");

            if (string.IsNullOrWhiteSpace(nv.CCCD))
                return (false, "Số CCCD không được để trống!");

            if (nv.MaPB <= 0)
                return (false, "Vui lòng chọn phòng ban trực thuộc!");

            if (nv.LuongCoBan <= 0)
                return (false, "Lương cơ bản phải lớn hơn 0!");

            if (nv.HeSoLuong <= 0)
                return (false, "Hệ số lương phải lớn hơn 0!");

            try
            {
                bool ok = _dal.Update(nv);
                return ok ? (true, "Cập nhật nhân viên thành công!") : (false, "Cập nhật nhân viên thất bại!");
            }
            catch (Exception ex)
            {
                return (false, "Lỗi cập nhật nhân viên: " + ex.Message);
            }
        }

        public (bool Success, string Message) ChuyenPhongBan(int maNV, int maPBMoi)
        {
            if (maNV <= 0 || maPBMoi <= 0)
                return (false, "Thông tin chuyển phòng ban không hợp lệ!");

            bool ok = _dal.ChuyenPhongBan(maNV, maPBMoi);
            return ok ? (true, "Chuyển phòng ban thành công!") : (false, "Chuyển phòng ban thất bại!");
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
                return (false, "Không thể xóa nhân viên do đã có dữ liệu chấm công / tính lương: " + ex.Message);
            }
        }
    }
}

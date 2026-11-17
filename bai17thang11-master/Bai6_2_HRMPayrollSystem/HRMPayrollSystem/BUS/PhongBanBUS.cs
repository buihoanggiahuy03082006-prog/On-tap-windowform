using System;
using System.Collections.Generic;
using HRMPayrollSystem.DAL;
using HRMPayrollSystem.DTO;

namespace HRMPayrollSystem.BUS
{
    public class PhongBanBUS
    {
        private readonly PhongBanDAL _dal = new();

        public List<PhongBanDTO> GetAll() => _dal.GetAll();

        public (bool Success, string Message) Add(PhongBanDTO pb)
        {
            if (string.IsNullOrWhiteSpace(pb.TenPB))
                return (false, "Tên phòng ban không được để trống!");

            bool ok = _dal.Insert(pb);
            return ok ? (true, "Thêm phòng ban thành công!") : (false, "Thêm phòng ban thất bại!");
        }

        public (bool Success, string Message) Update(PhongBanDTO pb)
        {
            if (pb.MaPB <= 0)
                return (false, "Mã phòng ban không hợp lệ!");

            if (string.IsNullOrWhiteSpace(pb.TenPB))
                return (false, "Tên phòng ban không được để trống!");

            bool ok = _dal.Update(pb);
            return ok ? (true, "Cập nhật phòng ban thành công!") : (false, "Cập nhật phòng ban thất bại!");
        }

        public (bool Success, string Message) Delete(int maPB)
        {
            if (maPB <= 0)
                return (false, "Vui lòng chọn phòng ban cần xóa!");

            try
            {
                bool ok = _dal.Delete(maPB);
                return ok ? (true, "Xóa phòng ban thành công!") : (false, "Không tìm thấy phòng ban cần xóa!");
            }
            catch (Exception ex)
            {
                return (false, "Không thể xóa phòng ban do đang có nhân viên trực thuộc: " + ex.Message);
            }
        }
    }
}

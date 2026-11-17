using System.Collections.Generic;
using HRMPayrollSystem.DAL;
using HRMPayrollSystem.DTO;

namespace HRMPayrollSystem.BUS
{
    public class TinhLuongBUS
    {
        private readonly TinhLuongDAL _dal = new();

        public List<BangLuongDTO> TinhLuongDuKien(string thangNam) => _dal.TinhLuongDuKien(thangNam);

        public (bool Success, string Message) ChotLuongThang(string thangNam, List<BangLuongDTO> danhSachLuong)
        {
            return _dal.ChotLuongThang(thangNam, danhSachLuong);
        }

        public List<BangLuongDTO> GetBangLuongDaChot(string thangNam) => _dal.GetBangLuongDaChot(thangNam);
    }
}

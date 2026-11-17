using System.Data;
using Microsoft.Data.SqlClient;
using HRMPayrollSystem.DAL;

namespace HRMPayrollSystem.BUS
{
    public class ChamCongBUS
    {
        private readonly ChamCongDAL _dal = new();

        public (SqlDataAdapter Adapter, DataTable Table, SqlConnection Connection) GetChamCongDisconnected(string thangNam)
        {
            return _dal.GetChamCongDisconnected(thangNam);
        }

        public (bool Success, string Message, int RowsAffected) LuuChamCong(SqlDataAdapter adapter, DataTable table, string thangNam)
        {
            if (_dal.KiemTraDaKhoa(thangNam))
            {
                return (false, $"Dữ liệu chấm công tháng {thangNam} ĐÃ BỊ KHÓA do đã chốt lương! Không thể chỉnh sửa.", 0);
            }

            try
            {
                int rows = _dal.LuuChamCong(adapter, table);
                return (true, $"Đã lưu và đồng bộ thành công dữ liệu chấm công tháng {thangNam} ({rows} dòng cập nhật)!", rows);
            }
            catch (System.Exception ex)
            {
                return (false, "Lỗi đồng bộ dữ liệu chấm công: " + ex.Message, 0);
            }
        }

        public bool KiemTraDaKhoa(string thangNam) => _dal.KiemTraDaKhoa(thangNam);
    }
}

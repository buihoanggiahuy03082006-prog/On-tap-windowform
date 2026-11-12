using System;

namespace Bai5_4_QuanLyNhanSu
{
    public class Employee
    {
        public string EmployeeId { get; set; }     // Mã NV
        public string FullName { get; set; }       // Họ tên
        public string Position { get; set; }       // Chức vụ
        public DateTime StartDate { get; set; }    // Ngày vào làm
        public string UnitKey { get; set; }        // Khóa node (phòng ban/nhóm) mà nhân viên thuộc về

        public Employee(string id, string name, string position, DateTime startDate, string unitKey)
        {
            EmployeeId = id;
            FullName = name;
            Position = position;
            StartDate = startDate;
            UnitKey = unitKey;
        }
    }
}

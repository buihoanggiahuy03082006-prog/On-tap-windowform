using System;
using System.Windows.Forms;

namespace Bai5_4_QuanLyNhanSu
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FrmEmployees());
        }
    }
}

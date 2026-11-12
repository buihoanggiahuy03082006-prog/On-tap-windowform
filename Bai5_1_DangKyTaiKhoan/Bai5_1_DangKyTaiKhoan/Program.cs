namespace Bai5_1_DangKyTaiKhoan;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new FormDangKy());
    }
}

namespace Bai5_2_TinhTienDichVu;
internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new FormTinhTien());
    }
}

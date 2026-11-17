using System;
using System.Linq;
using System.Windows.Forms;
using RetailSalesSystem.GUI;

namespace RetailSalesSystem
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();

            if (args != null && args.Contains("--capture"))
            {
                CaptureHelper.CaptureAll();
                return;
            }

            Application.Run(new FormMain());
        }
    }
}

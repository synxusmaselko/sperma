using System;
using System.Windows.Forms;

namespace Nebula_Spoofer
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form2()); // Replace YourMainForm with your main form
        }
    }
}

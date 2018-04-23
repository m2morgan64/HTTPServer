using System;
using System.Windows.Forms;

namespace Visualizer
{
    // ReSharper disable once ArrangeTypeModifiers
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        // ReSharper disable once ArrangeTypeMemberModifiers
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmVisualizer());
        }
    }
}

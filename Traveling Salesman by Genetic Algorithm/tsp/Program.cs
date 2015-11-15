
using System;
using System.Windows.Forms;

namespace Tsp
{
    /// <summary>
    /// Содержит основу, с которой начинается этот вид.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// Главная тоска входа.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TspForm());
        }
    }
}

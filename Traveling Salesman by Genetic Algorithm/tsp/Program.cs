
using System;
using System.Windows.Forms;

namespace Tsp
{
    /// <summary>
    /// �������� ������, � ������� ���������� ���� ���.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// ������� ����� �����.
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

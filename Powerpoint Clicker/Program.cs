using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PptHotkey
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                using (var h = new KeyboardHooker())
                {
                    Application.Run(new Form1(h));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "PPT Hotkey Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
    }
}
